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
		private DbSet<Event> _eventDataSet;
		private IRepository<Domain.Event, int> _eventData; 
		private DbSet<Distance> _distanceDataSet;
		private IRepository<Domain.Distance, int> _distanceData; 
		private DbSet<Race> _raceDataSet;
		private IRepository<Race, int> _raceData;
		private DbSet<Photographer> _photographerDataSet;
		private IReferenceRepository<Photographer, int> _photographerData; 

		public DbSet<Photo> PhotoDataSet { get { return _photoDataSet; } }
		public DbSet<Event> EventDataSet { get { return _eventDataSet; } }
		public DbSet<Distance> DistanceDataSet { get { return _distanceDataSet; } }
		public DbSet<Race> RaceDataSet { get { return _raceDataSet; } }
		public DbSet<Photographer> PhotographerDataSet { get { return _photographerDataSet; } }
		public IRepository<Domain.Photo, Guid> Photos
		{
			get { return _photoData; }
		}
		public IRepository<Event, int> Events { get { return _eventData; } }
		public IRepository<Distance, int> Distances { get { return _distanceData; } }
		public IRepository<Race, int> Races { get { return _raceData; } }
		public IReferenceRepository<Photographer, int> Photographers { get { return _photographerData; } }

		public EFPhotoServerDataSource( ) : this ("DefaultConnection")
		{
		
		}
		
		public EFPhotoServerDataSource(string connectionString) : base(connectionString)
		{
			_photoDataSet = Set<Photo>();
			_photoData = new PhotoDataRepository(_photoDataSet);
			_eventDataSet = Set<Event>();
			_eventData = new EventRepository(_eventDataSet);
			_distanceDataSet = Set<Distance>();
			_distanceData = new DistanceRepository(_distanceDataSet);
			_raceDataSet = Set<Race>();
			_raceData = new RaceRepository(_raceDataSet);
			_photographerDataSet = Set<Photographer>();
			_photographerData = new PhotographerReferenceRepository(_photographerDataSet);

		}

	}
}
