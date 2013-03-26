using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Security;
using AutoMapper;
using Elmah;
using ExifLib;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;
using PhotoServer.Models;

namespace PhotoServer.Controllers
{
    public class PhotosController : ApiController
    {
        private IPhotoDataSource _db;
	    private string appHome = string.Empty;
		public HttpContextBase context { get; set; }
	    private string physicalPhotosPath;

        public PhotosController(IPhotoDataSource Db)
        {
            _db = Db;
			physicalPhotosPath = WebConfigurationManager.AppSettings["PhotosPhysicalDirectory"];
			if (string.IsNullOrWhiteSpace(physicalPhotosPath))
				throw new ConfigurationErrorsException("No configuration for PhotosPhysicalDirectory.  Set path where photo images are to be stored.");
        }
        // GET api/photos
		[Authorize(Roles = "Administrator")]
        public IEnumerable<Models.PhotoData> Get()
        {
	
				var data = _db.photoData.FindAll().ToList();
				return data.Select( Mapper.Map<Domain.Photo, Models.PhotoData>).ToList();
        }

        // GET api/photos/5
        public HttpResponseMessage Get(Guid id)
        {
            var returnMsg = new HttpResponseMessage();
	        var record = _db.photoData.FindById(id);
			if (record == null)
			{
				returnMsg.StatusCode = HttpStatusCode.NotFound;
			}

			else
			{
				var path = GetLocalPath(record.Path);
				if (!File.Exists(path))
					returnMsg.StatusCode = HttpStatusCode.NotFound;
				else
				{
					using (var fileStream = new FileStream(path, FileMode.Open))
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
        public HttpResponseMessage Post(string race, string station, string card, int? seq)
        {
	        int Sequence = seq ?? GetMaxSeq(race, station, card) + 1;
	        var data = new Photo(race, station, card, Sequence);
	        var path = data.Path;
	        var request = ControllerContext.Request;
			if (request.Content == null || request.Content.Headers == null || request.Content.Headers.ContentType == null)
			{ return new HttpResponseMessage(HttpStatusCode.BadRequest);}
	        var contentType = request.Content.Headers.ContentType.MediaType;
			if (contentType == "image/jpeg" || contentType == "image/jpg")
			{
				var imageSize = request.Content.Headers.ContentLength;
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
					image.Seek(0, SeekOrigin.Begin);
					using (var reader = new ExifLib.ExifReader(image))
					{
						String timeString;
						if (reader.GetTagValue(ExifTags.DateTimeOriginal, out timeString))
						{
							DateTime timeStamp;
							if (DateTime.TryParseExact(timeString,"yyyy:MM:dd hh:mm:ss", 
								CultureInfo.InvariantCulture, 
								DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
								out timeStamp))
								data.TimeStamp = timeStamp;
						}
						UInt16 hres, vres;
						if (reader.GetTagValue(ExifTags.PixelXDimension, out hres))
							data.Hres = (int) hres;
						if (reader.GetTagValue(ExifTags.PixelYDimension, out vres))
							data.Vres = (int) vres;
						if (reader.GetTagValue(ExifTags.SubsecTimeOriginal, out timeString))
						{
							int msec;
							if (int.TryParse(timeString, out msec))
							{
								data.TimeStamp += new TimeSpan(0, 0, 0, 0, msec);
							}
						}

					}

					
					image.Close();
				}


				path = GetLocalPath(path);

				if (!File.Exists(path))
				{
					var dir = Path.GetDirectoryName(path);
					if (! Directory.Exists(dir))
						Directory.CreateDirectory(dir);
					
					var newImage = new BinaryWriter(new FileStream(path, FileMode.CreateNew));
					newImage.Write(imageArray);
				}
				_db.photoData.Add(data);
				_db.SaveChanges();
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
            var response = new HttpResponseMessage(HttpStatusCode.Created);
	        var uri =new Uri("/api/Photos/" + data.Id.ToString(), UriKind.Relative);
	        response.Headers.Location = uri;
	        var modelData = Mapper.Map<PhotoServer.Domain.Photo, Models.PhotoData>(data);
	        response.Content = new ObjectContent<Models.PhotoData>(modelData, new JsonMediaTypeFormatter());
            return response;
        }

	    private string GetLocalPath(string virtualPath)
	    {
		    return Path.Combine(physicalPhotosPath, virtualPath);
	    }

	    private int GetMaxSeq(string race, string station, string card)
	    {
			    var list = _db.photoData.FindAll()
			       .Where(pd => pd.Race == race && pd.Station == station && pd.Card == card)
			       .Select(pd => pd.Sequence).ToList<int>();
		    if (list.Count > 0) return list.Max();
		    return 0;
	    }

	    // PUT api/photos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/photos/5
        public void Delete(int id)
        {
        }
    }
}
