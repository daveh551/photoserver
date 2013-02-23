using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;

namespace PhotoServer_Tests.Support
{
    public class FakeDataSource : IPhotoDataSource
    {
        private FakeRepository<PhotoData>  _photoData;
        public IRepository<PhotoData, Guid> photoData { get { return _photoData; } }

        public FakeDataSource()
        {
            _photoData = new FakeRepository<PhotoData>();
        }
    }

   
}
