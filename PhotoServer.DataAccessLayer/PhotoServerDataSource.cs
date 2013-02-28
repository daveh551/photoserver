using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.DataAccessLayer
{
	public class PhotoServerDataSource : IPhotoDataSource
	{
		private IRepository<Domain.PhotoData,Guid> _photoData;

		public IRepository<Domain.PhotoData, Guid> photoData
		{
			get { return _photoData; }
		}

		public PhotoServerDataSource(IRepository<Domain.PhotoData, Guid> photoData )
		{
			_photoData = photoData;
		}
	}
}
