using NUnit.Framework;
using PhotoServer.Controllers;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;
using PhotoServer_Tests.Support;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;

namespace PhotoServer_Tests.Controllers.PhotosController_Tests
{
	[TestFixture]
	public class PostAction_Tests
	{
		#region SetUp / TearDown

		private PhotosController target;
		private string pathArgument = @"Test\FinishLine\1\1.jpg";
		[TestFixtureSetUp]
		public void InitFixture()
		{
				
		}
		private IPhotoDataSource fakeDataSource;
		[SetUp]
		public void Init()
		{
			var fileName = @"..\..\TestFiles\Run For The Next Generation 2011 001.JPG";
			fakeDataSource =   new FakeDataSource();
			target = new PhotosController(fakeDataSource);
			target.ControllerContext = new FakeControllerContext();
			target.ControllerContext.Request = SetupContent(fileName);
			target.context = new  FakeHttpContext();
		}
		private HttpRequestMessage SetupContent(string fileName)
		{

			var req = new HttpRequestMessage(HttpMethod.Post, "http://localhost:6471/api/Photos/?path=" + pathArgument);
			using (var file = new BinaryReader(new FileStream(fileName, FileMode.Open)))
			{
				var fileSize = new FileInfo(fileName).Length;
				var image = file.ReadBytes((int) fileSize);
				req.Content = new ByteArrayContent(image);
				req.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			}

			return req;
		}

		[TearDown]
		public void Dispose()
		{ }

		#endregion

		#region Tests
		
		[Test]
		[Ignore("Authentication has to go through the dispatch.")]
		public void Post_WithoutAuthentication_ShouldReturn401Error()
		{
			//Arrange
			HttpStatusCode expected = HttpStatusCode.Forbidden;
			//Act
			HttpResponseMessage result = target.Post("abc");
			//Assert
			Assert.AreEqual(expected, result.StatusCode, "failure message");
		}
		[Test]
		public void Post_WithNewPhoto_ShouldAddPhotosDataItemToDB()
		{
			//Arrange
			//Act
			var result = target.Post(pathArgument);
			//Assert
			Assert.AreEqual(1, fakeDataSource.photoData.FindAll().Count((item) =>true));
		}

		
		[Test]
		public void Post_WithNewPhoto_ReturnsLocationHeaderWithGuid()
		{
			//Arrange
			//Act
			var result = target.Post(pathArgument);
			var hdrs = result.Headers;
			
			//Assert
			Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Did not return Created status code");
			Assert.IsNotNull(hdrs.Location, "Location Header is null");
			Assert.That(hdrs.Location.OriginalString.StartsWith("/api/Photos/"));
		}

		
		[Test]
		public  void  Post_WithNewData_ReturnsPhotoDataItemInMessageBody()
		{
			//Arrange
			//Act
			var result = target.Post(pathArgument);
			var dataItem = fakeDataSource.photoData.FindAll().FirstOrDefault();
			var body = result.Content;
			var bodyString = body.ReadAsStringAsync().Result;
			var resultData = Json.Decode<PhotoData>(bodyString);
			//Assert
			Assert.IsNotNull(dataItem, "returned null dataItem");
			Assert.AreEqual(dataItem.Id, resultData.Id, "Item Id in HttpContent not equal to data Item Id");
			Assert.AreEqual(dataItem.Path, resultData.Path, "Item Path in HttpContent not equal to data Item Path");
		}

		
		[Test]
		public void Post_WithAttachedPhoto_ResultsInPhotoInDirectory()
		{
			//Arrange
			ClearDirectory();

			//Act
			var result = target.Post(pathArgument);
			//Assert
			var resultPath = Path.Combine( @"..\..\..\PhotoServer\Photos" , pathArgument);
			Assert.That(File.Exists(resultPath));
		}


		private void ClearDirectory()
		{
			var photoPath = @"..\..\..\PhotoServer\Photos\Test";
			var directoryInfo = new DirectoryInfo(photoPath);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(true);
			}

		}

		#endregion
	}

	
}
