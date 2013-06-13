using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using PhotoServer.Controllers;
using PhotoServer.DataAccessLayer;
using PhotoServer.DataAccessLayer.Storage;
using PhotoServer.Domain;
using PhotoServer.Models;
using RacePhotosTestSupport;

namespace PhotoServer_Tests.Controllers.PhotosController_Tests
{
	[TestFixture]
	public class PutAction_Tests
	{
		#region SetUp / TearDown

		private PhotosController target;
		private IPhotoDataSource fakeDataSource;
	    protected IStorageProvider provider;
		[TestFixtureSetUp]
		public void SetupFixture()
		{
			PhotoServer.App_Start.InitializeMapper.MapClasses();
			provider = new AzureStorageProvider(@"UseDevelopmentStorage=true", "images");	
		}

		[SetUp]
		public void Init()
		{
			fakeDataSource = new FakeDataSource();
			var photoRecordList = ObjectMother.ReturnPhotoDataRecord(3);
			foreach (var photo in photoRecordList)
			{
				fakeDataSource.Photos.Add(photo);
			}
			fakeDataSource.SaveChanges();
			target = new PhotosController(fakeDataSource, provider);
		}


		[TearDown]
		public void Dispose()
		{ }

		#endregion

		#region Tests



		[Test]
		public void Put_PassingRaceData_FillsInDataFields()
		{
			//Arrange
			string expected = "FillsInDataFields";
			var photoRecord = fakeDataSource.Photos.FindAll().FirstOrDefault();
			var photoData = AutoMapper.Mapper.Map<Photo, PhotoData>(photoRecord);
			photoData.Event = "Test";
			photoData.Station = "FinishLine";
			photoData.Card = "1";
			photoData.Sequence = 1;
			//Act

			target.Put(photoRecord.Id, photoData);

			//Assert
			var resultPhotoRecord = fakeDataSource.Photos.FindById(photoRecord.Id);
			Assert.IsNotNull(resultPhotoRecord);
			Assert.AreEqual(1, resultPhotoRecord.EventId,  "Event");
			Assert.AreEqual("FinishLine", resultPhotoRecord.Station, "Station");
			Assert.AreEqual("1", resultPhotoRecord.Card, "Card");
			Assert.AreEqual(1, resultPhotoRecord.Sequence, "Sequence");

		}

		#endregion
	}
}
