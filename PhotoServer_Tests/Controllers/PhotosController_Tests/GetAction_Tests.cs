using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using PhotoServer.Controllers;
using PhotoServer_Tests.Support;

namespace PhotoServer_Tests.Controllers.PhotosController_Tests
{
	[TestFixture]
	public class GetAction_Tests
	{
		#region SetUp / TearDown

		private PhotosController target;

		[TestFixtureSetUp]
		public void InitFixture()
		{
			PhotoServer.App_Start.InitializeMapper.MapClasses();
			ObjectMother.ClearDirectory();
			ObjectMother.CopyTestFiles();
		}
		[SetUp]
		public void Init()
		{
			var db = new FakeDataSource();
			var testRecords = ObjectMother.ReturnPhotoDataRecord(3);
			testRecords.ForEach( r => db.photoData.Add(r));
			db.SaveChanges();
			target = new PhotosController(db);
			target.context = new FakeHttpContext();

		}

		[TearDown]
		public void Dispose()
		{ }

		#endregion

		
		[Test]
		public void Get_CalledWithNoArgument_ReturnsListOf3PhotoDatas()
		{
			//Arrange
			var expectedCount = 3;
			//Act
			var result = target.Get();
			var count = result.ToList().Count;
			//Assert

			Assert.AreEqual(expectedCount, count, "Return record count");
		}

		
		[Test]
		public void Get_CalledWithNonExistentGuid_ReturnsNotFound()
		{
			//Arrange
			var expectedStatus = HttpStatusCode.NotFound;
			Guid recordID = new Guid();
			//Act
			HttpResponseMessage result = target.Get(recordID);
			//Assert
			Assert.AreEqual(expectedStatus, result.StatusCode, "Status Code");
		}

		
		[Test]
		public void Get_CalledWithValidGuid_ReturnsOKWithExpectedFile()
		{
			//Arrange
			var expectedStatus = HttpStatusCode.OK;
			Guid recordId = ObjectMother.ReturnPhotoDataRecord(1)[0].Id;
			//Act
			var result = target.Get(recordId);
			//Assert
			Assert.AreEqual(expectedStatus, result.StatusCode, "Status Code");
			Assert.IsNotNull(result.Content, "Content is null");
		}
	}
}
