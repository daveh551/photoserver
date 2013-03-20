using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class EFPhotoServerDataSource : DbContext, IPhotoDataSource
	{
		private IRepository<Domain.Photo,Guid> _photoData;
		private DbSet<Photo> _photoDataSet;

		public DbSet<Photo> PhotoDataSet { get { return _photoDataSet; } }
		public IRepository<Domain.Photo, Guid> photoData
		{
			get { return _photoData; }
		}

		public EFPhotoServerDataSource( ) : this ("DefaultConnection")
		{
		
		}
		
		public EFPhotoServerDataSource(string connectionString) : base(connectionString)
		{
			_photoDataSet = Set<Photo>();
			_photoData = new PhotoDataRepository(_photoDataSet);
		}

	}
}
