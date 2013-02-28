using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;

namespace PhotoServer.Controllers
{
    public class PhotosController : ApiController
    {
        private IPhotoDataSource _db;
	    private string appHome = string.Empty;
		public HttpContextBase context { get; set; }

        public PhotosController(IPhotoDataSource Db)
        {
	        appHome = Path.GetDirectoryName(this.GetType().Assembly.Location);
	        appHome = Path.GetDirectoryName(appHome);

            _db = Db;
        }
        // GET api/photos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/photos/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/photos
        
        [Authorize(Roles="admin")]
        public HttpResponseMessage Post(string race, string station, string card, int? seq)
        {
	        int Sequence = seq ?? GetMaxSeq(race, station, card) + 1;
	        var data = new PhotoData(race, station, card, Sequence);
	        var path = data.Path;
	        var request = ControllerContext.Request;
			if (request.Content == null || request.Content.Headers == null || request.Content.Headers.ContentType == null)
			{ return new HttpResponseMessage(HttpStatusCode.BadRequest);}
	        var contentType = request.Content.Headers.ContentType.MediaType;
			if (contentType == "image/jpeg" || contentType == "image/jpg")
			{
				var imageSize = request.Content.Headers.ContentLength;
				var image = request.Content.ReadAsStreamAsync().Result;
				byte[] imageArray;
				if (!imageSize.HasValue)
				{
					imageSize = (int) image.Length;
				}
				var imageSz = (int) imageSize.Value;
				imageArray = new byte[imageSz];
				int bytesRead = 0;
				while (bytesRead < imageSz)
				{
					bytesRead +=image.Read(imageArray, bytesRead,  imageSz - bytesRead);
				}


				HttpContextBase localContext = HttpContext.Current == null ? context : new HttpContextWrapper(HttpContext.Current);
				path =localContext.Server.MapPath(Path.Combine("~/Photos",path));
				
				if (!File.Exists(path))
				{
					var dir = Path.GetDirectoryName(path);
					if (! Directory.Exists(dir))
						Directory.CreateDirectory(dir);
					
					var newImage = new BinaryWriter(new FileStream(path, FileMode.CreateNew));
					newImage.Write(imageArray);
				}
				_db.photoData.Add(data);
				_db.photoData.SaveChanges();
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
            var response = new HttpResponseMessage(HttpStatusCode.Created);
	        var uri =new Uri("/api/Photos/" + data.Id.ToString(), UriKind.Relative);
	        response.Headers.Location = uri;
	        response.Content = new ObjectContent<PhotoData>(data, new JsonMediaTypeFormatter());
            return response;
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
