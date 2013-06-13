using System.Diagnostics;
using AutoMapper;
using ExifLib;
using PhotoServer.DataAccessLayer;
using PhotoServer.DataAccessLayer.Storage;
using PhotoServer.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using PhotoServer.Models;

namespace PhotoServer.Controllers
{
    public class PhotosController : ApiController
    {
		
        private IPhotoDataSource _db;
	    private IStorageProvider _storage;
	    private string appHome = string.Empty;
	    private HttpContextBase _context;
		public HttpContextBase context {
			get
			{

				if (HttpContext.Current != null)
					return new HttpContextWrapper(HttpContext.Current);
				return _context;
			}
			set { _context = value; }
		}


        public PhotosController(IPhotoDataSource Db, IStorageProvider storageProvider)
        {
            _db = Db;
	        _storage = storageProvider;
			Trace.TraceInformation("Created PhotosController with {0}", storageProvider.GetType().ToString());
        }
        // GET api/photos
		[Authorize(Roles = "Administrator")]
        public IEnumerable<Models.PhotoData> Get()
        {
	
				var data = _db.Photos.FindAll().ToList();
				return data.Select( Mapper.Map<Domain.Photo, Models.PhotoData>).ToList();
        }

        // GET api/photos/5
        public HttpResponseMessage Get(Guid id)
        {
            var returnMsg = new HttpResponseMessage();
	        var record = _db.Photos.FindById(id);
			if (record == null)
			{
				returnMsg.StatusCode = HttpStatusCode.NotFound;
			}

			else
			{
				if (!_storage.FileExists(record.Path))
					returnMsg.StatusCode = HttpStatusCode.NotFound;
				else
				{
					using (var fileStream = _storage.GetStream(record.Path))
					{
						returnMsg.Content = new StreamContent(fileStream);
						returnMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
						returnMsg.Content.Headers.ContentLength = fileStream.Length;
					}
				}
			}
			return returnMsg;
        }

        // POST api/photos
        
        [Authorize(Roles="Administrator")]
        public HttpResponseMessage Post()
        {
	        var returnCode = HttpStatusCode.Created;
			Trace.TraceInformation("Got request to upload photo" );
	        var data = new Photo();
	        data.Id = Guid.NewGuid();
	        data.Path = "originals/" + data.Id.ToString();
			Trace.TraceInformation("PhotoStoragePath = {0}", data.Path);
	        var request = ControllerContext.Request;
			if (request.Content == null || request.Content.Headers == null || request.Content.Headers.ContentType == null)
			{ return new HttpResponseMessage(HttpStatusCode.BadRequest);}
	        var contentType = request.Content.Headers.ContentType.MediaType;
			if (contentType == "image/jpeg" || contentType == "image/jpg")
			{
				var imageSize = request.Content.Headers.ContentLength;
				Trace.TraceInformation("ContentLength is {0} bytes", imageSize);
				byte[] imageArray;
				using (var image = request.Content.ReadAsStreamAsync().Result)
				{
					if (!imageSize.HasValue)
					{
						imageSize = (int) image.Length;
					}
					var imageSz = (int) imageSize.Value;
					imageArray = new byte[imageSz];
					int bytesRead = 0;
					while (bytesRead < imageSz)
					{
						bytesRead += image.Read(imageArray, bytesRead, imageSz - bytesRead);
					}
					data.FileSize = bytesRead;
					data.LastAccessed = DateTime.Now;
					Trace.TraceInformation("Getting Exif Data");
					GetExifData(image, data);


					image.Close();
				}


				

				Trace.TraceInformation("Writing photo to storage file {0}", data.Path);
				_storage.WriteFile(data.Path, imageArray);

				data.Server = request.RequestUri.Host;
				data.CreatedBy = context.User.Identity.Name;
				data.CreatedDate = DateTime.Now;
				if (returnCode == HttpStatusCode.Created)
				{
					Trace.TraceInformation("Adding photo data to database");
					_db.Photos.Add(data);
					_db.SaveChanges();
				}
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
            var response = new HttpResponseMessage(returnCode);
	        if (returnCode == HttpStatusCode.Created)
	        {
		        var uri = new Uri("/api/Photos/" + data.Id.ToString(), UriKind.Relative);
		        response.Headers.Location = uri;
		        var modelData = Mapper.Map<PhotoServer.Domain.Photo, Models.PhotoData>(data);
		        response.Content = new ObjectContent<Models.PhotoData>(modelData, new JsonMediaTypeFormatter());
	        }
	        return response;
        }

	    private static void GetExifData(Stream image, Photo data)
	    {
		    image.Seek(0, SeekOrigin.Begin);
		    using (var reader = new ExifLib.ExifReader(image))
		    {
			    String timeString;
			    if (reader.GetTagValue(ExifTags.DateTimeOriginal, out timeString))
			    {
				    DateTime timeStamp;
				    if (DateTime.TryParseExact(timeString, "yyyy:MM:dd hh:mm:ss",
				                               CultureInfo.InvariantCulture,
				                               DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
				                               out timeStamp))
					    data.TimeStamp = timeStamp;
			    }
			    UInt16 hres, vres;
			    object tagVal;
			    if (reader.GetTagValue(ExifTags.PixelXDimension, out hres))
				    data.Hres = (int) hres;
			    if (reader.GetTagValue(ExifTags.PixelYDimension, out vres))
				    data.Vres = (int) vres;
			    if (reader.GetTagValue(ExifTags.FNumber, out tagVal))
				    data.FStop = string.Format("f/{0:g2}", tagVal.ToString());
			    if (reader.GetTagValue(ExifTags.ExposureTime, out tagVal))
				    data.ShutterSpeed = string.Format("1/{0:g0}", 1/(double) tagVal);
				if (reader.GetTagValue(ExifTags.ISOSpeedRatings, out tagVal))
					data.ISOspeed = (short) (ushort) tagVal;
			    if (reader.GetTagValue(ExifTags.FocalLength, out tagVal))
				    data.FocalLength = (short) (double) tagVal;
				
			    if (reader.GetTagValue(ExifTags.SubsecTimeOriginal, out timeString))
			    {
				    int msec;
				    if (int.TryParse(timeString, out msec))
				    {
					    data.TimeStamp += new TimeSpan(0, 0, 0, 0, msec);
				    }
			    }
		    }
	    }



	    private int GetMaxSeq(int eventId, string station, string card)
	    {
			    var list = _db.Photos.Find(pd => pd.EventId == eventId && pd.Station == station && pd.Card == card && pd.Sequence.HasValue)
			       .Select(pd => pd.Sequence.Value).ToList<int>();
		    if (list.Count > 0) return list.Max();
		    return 0;
	    }

	    // PUT api/photos/5
        public void Put(Guid id, PhotoData newValues)
        {
	        var existingValues = _db.Photos.FindById(id);
			if (existingValues == null) 
				throw new HttpResponseException(HttpStatusCode.NotFound);
	        CopyWriteableValues(newValues, existingValues);
	        _db.SaveChanges();

        }

	    private void CopyWriteableValues(PhotoData newValues, Photo existingValues)
	    {
		    existingValues.Card = newValues.Card;
		    var photographer = _db.Photographers.Find(ph => ph.Initials == newValues.PhotographerInitials).FirstOrDefault();
		    if (photographer != null)
			    existingValues.Photographer = photographer;
		    existingValues.Event = _db.Events.Find(e => e.EventName == newValues.Event).SingleOrDefault();
		    existingValues.Sequence = newValues.Sequence;
		    existingValues.Station = newValues.Station;

	    }

	    // DELETE api/photos/5
        public void Delete(int id)
        {
			throw new NotImplementedException("DELETE operation to /api/Photos is not yet implemented");
        }
    }

	
}
