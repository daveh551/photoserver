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
        private FakeRepository<Photo>  _photoData;
        public IRepository<Photo, Guid> photoData { get { return _photoData; } }

        public FakeDataSource()
        {
            _photoData = new FakeRepository<Photo>();
        }

	    public int SaveChanges()
	    {
		    return _photoData.SaveChanges();
	    }
    }

   
}
