using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using PhotoServer.Controllers;

namespace PhotoServer_Tests.Controllers.PhotosController_Tests
{
    [TestFixture]
    public class PostAction_Tests
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Test]
        public void Post_WithNewPhoto_ShouldAddPhotosDataItemToDB()
        {
            //Arrange
            var target = new PhotosController();
            //Act
            //Assert
        }
        
        [Test]
        public void Post_WithNewPhoto_ShoulAddPhotosDataItemToDB()
        {
            //Arrange
            string expected = "ShoulAddPhotosDataItemToDB";
            var target = new PhotosController();
            var path = "/Test/FinishLine/1/1";
            //Act
            target.Post(path);
            //Assert
            //Assert.AreEqual(expected, result, "failure message");
        }
        #endregion
    }
}
