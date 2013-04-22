using NUnit.Framework;
using PhotoServer.Controllers;
using PhotoServer.DataAccessLayer;
using PhotoServer.DataAccessLayer.Storage;
using RacePhotosTestSupport;
using System;
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
		private string pathArgument = @"Test.5K\FinishLine\1\001.jpg";
		private int raceArgument = 1;
		private string eventName = "Test";
		private const string raceName = "Test.5K";
		private string stationArgument = "FinishLine";
		private string cardArgument = "1";
		private int seqArgument = 1;

	    protected IStorageProvider provider;
		[TestFixtureSetUp]
		public virtual void InitFixture()
		{
			PhotoServer.App_Start.InitializeMapper.MapClasses();
			provider = new AzureStorageProvider(@"UseDevelopmentStorage=true", "images");
			ObjectMother.SetPhotoPath();
		}
		private IPhotoDataSource fakeDataSource;
		[SetUp]
		public void Init()
		{
			var fileName = @"..\..\TestFiles\Run For The Next Generation 2011 001.JPG";
			fakeDataSource =   new FakeDataSource();
			target = new PhotosController(fakeDataSource, provider);
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
		public void Post_WithNewPhoto_ShouldAddPhotosDataItemToDB()
		{
			//Arrange
			//Act
			var result = target.Post(raceArgument, stationArgument, cardArgument, seqArgument);
			//Assert
			Assert.AreEqual(1, fakeDataSource.Photos.FindAll().Count((item) =>true));
		}

		
		[Test]
		public void Post_WithNewPhoto_ReturnsLocationHeaderWithGuid()
		{
			//Arrange
			//Act
			var result = target.Post(raceArgument, stationArgument, cardArgument, seqArgument);
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
			var result = target.Post(raceArgument, stationArgument, cardArgument, seqArgument);
			var dataItem = fakeDataSource.Photos.FindAll().FirstOrDefault();
			var body = result.Content;
			var bodyString = body.ReadAsStringAsync().Result;
			var resultData = Json.Decode<PhotoServer.Models.PhotoData>(bodyString);
			//Assert
			Assert.IsNotNull(dataItem, "returned null dataItem");
			Assert.AreEqual(dataItem.Id, resultData.Id, "Item Id in HttpContent not equal to data Item Id");
			Assert.AreEqual(raceName, resultData.Race, "RaceName");
			Assert.AreEqual(stationArgument, resultData.Station, "Station");
		}

		
		[Test]
		public void Post_WithAttachedPhoto_ResultsInPhotoInDirectory()
		{
			//Arrange
			ObjectMother.ClearDirectory(provider);

			//Act
			var result = target.Post(raceArgument, stationArgument, cardArgument, seqArgument);
			//Assert
			var resultPath = Path.Combine( ObjectMother.photoPath , pathArgument);
			Assert.That(provider.FileExists(resultPath));
		}

		
		[Test]
		public void Post_WithAttachedPhoto_ReturnsDataWithTimeAndResolution()
		{
			//Arrange
			
			DateTime expectedTimeStamp = new DateTime(2011, 10, 22, 8, 28, 59, 60);
			int expectedHres = 3008;
			int expectedVres = 2000;
			//Act
			var result = target.Post(raceArgument, stationArgument, cardArgument, seqArgument);
			var bodyString = result.Content.ReadAsStringAsync().Result;
			var resultData = Json.Decode<PhotoServer.Models.PhotoData>(bodyString);

			//Assert
			Assert.AreEqual(expectedTimeStamp, resultData.TimeStamp, "TimeStamp");
			Assert.AreEqual(expectedHres, resultData.Hres, "Hres");
			Assert.AreEqual(expectedVres, resultData.Vres, "Vres");
		}


		#endregion
	}

	
}
