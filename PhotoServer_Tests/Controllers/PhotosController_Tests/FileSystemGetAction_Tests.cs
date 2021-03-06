﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PhotoServer.DataAccessLayer.Storage;
using RacePhotosTestSupport;

namespace PhotoServer_Tests.Controllers.PhotosController_Tests
{
    class FileSystemGetAction_Tests : GetAction_Tests
    {
        [TestFixtureSetUp]
        public override void InitFixture()
        {
            PhotoServer.App_Start.InitializeMapper.MapClasses();
            ObjectMother.SetPhotoPath();
            provider = new FileStorageProvider(ObjectMother.photoPath);
            ObjectMother.ClearDirectory(provider);
            ObjectMother.CopyTestFiles(provider);
 
        }
    }
}
