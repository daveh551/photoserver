using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using PhotoServer.Controllers;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;
using PhotoServer_Tests.Support;

namespace PhotoServer_Tests.Controllers.PhotosController_Tests
{
	[TestFixture]
	public class PostAction_Tests
	{
		#region SetUp / TearDown

		private IPhotoDataSource fakeDataSource;
		[SetUp]
		public void Init()
		{
			fakeDataSource =   new FakeDataSource();
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
			var target = new PhotosController(fakeDataSource);
			target.ControllerContext =  new FakeControllerContext();
			//Act
			HttpResponseMessage result = target.Post("abc");
			//Assert
			Assert.AreEqual(expected, result.StatusCode, "failure message");
		}
		[Test]
		public void Post_WithNewPhoto_ShouldAddPhotosDataItemToDB()
		{
			//Arrange
			string pathargument = @"Photos\Test\1\1.jpg";
			var target = new PhotosController(fakeDataSource);
			//Act
			var result = target.Post(pathargument);
			//Assert
			Assert.AreEqual(1, fakeDataSource.photoData.FindAll().Count((item) =>true));
		}

		
		[Test]
		public void Post_WithNewPhoto_ReturnsLocationHeaderWithGuid()
		{
			//Arrange
			string pathargument = @"Photos\Test\FinishLine\1\1.jpg";
			var target = new PhotosController(fakeDataSource);
			//Act
			var result = target.Post(pathargument);
			var hdrs = result.Headers;
			
			//Assert
			Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Did not return Created status code");
			Assert.IsNotNull(hdrs.Location, "Location Header is null");
			Assert.That(hdrs.Location.OriginalString.StartsWith("/api/Photos/"));
		}

		
		[Test]
		public async void  Post_WithNewData_ReturnsPhotoDataItemInMessageBody()
		{
			//Arrange
			var pathargument = @"\Photos\Test\FinishLine\1\1.jpg";
			var target = new PhotosController(fakeDataSource);
			//Act
			var result = target.Post(pathargument);
			var dataItem = fakeDataSource.photoData.FindAll().FirstOrDefault();
			var body = result.Content;
			var bodyString = await body.ReadAsStringAsync();
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
			var fileName = @"..\..\TestFiles\Run For The Next Generation 2011 001.JPG";
			var file = new BinaryReader(new FileStream(fileName, FileMode.Open));
			var fileSize = new FileInfo(fileName).Length;
			var image = file.ReadBytes((int) fileSize);
			var pathargument = @"\Photos\Testi\FinishLine\1\1.jpg";
			var target = new PhotosController(fakeDataSource);
			target.ControllerContext = new FakeControllerContext();
			var req = new HttpRequestMessage();
			req.Content = new ByteArrayContent(image);
			req.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			target.ControllerContext.Request = req;

			//Act
			var result = target.Post(pathargument);
			//Assert
			var resultPath = @"..\..\..\PhotoServer" + pathargument;
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
