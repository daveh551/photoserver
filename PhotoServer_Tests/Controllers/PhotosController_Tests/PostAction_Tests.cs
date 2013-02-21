using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using NUnit.Framework;
using PhotoServer.Controllers;
using PhotoServer.DataAccessLayer;
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
            var target = new PhotosController();
            //Act
            var result = target.Post(pathargument);
            //Assert
            Assert.AreEqual(1, fakeDataSource.photoData.FindAll().Count);
        }
        
   
        #endregion
    }
}
