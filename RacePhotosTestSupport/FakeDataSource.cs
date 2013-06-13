using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;

namespace RacePhotosTestSupport
{
    public class FakeDataSource : IPhotoDataSource
    {
        private FakeGuidRepository<Photo>  _photoData;
        public IRepository<Photo, Guid> Photos { get { return _photoData; } }
	    private FakeIntRepository<Event> _eventData;
		public IRepository<Event, int> Events { get { return _eventData; } }
	    private FakeIntRepository<Distance> _distanceData;
		public IRepository<Distance, int> Distances { get { return _distanceData; } }
	    private FakeIntReferenceRepository<Photographer> _photographerData;
		public IRepository<Photographer, int> Photographers { get { return _photographerData; } } 
 		

        public FakeDataSource()
        {
            _photoData = new FakeGuidRepository<Photo>();
	        InitPhotoData();
			_distanceData = new FakeIntRepository<Distance>();
	        InitDistanceData();
			_eventData = new FakeIntRepository<Event>();
	        InitEventData();
	        InitRaceData();
			_photographerData = new FakeIntReferenceRepository<Photographer>();
	        InitPhotographerData();
        }

	    private void InitPhotographerData()
	    {
		    _photographerData.Add(new Photographer {Id = 1, Initials = "dwh", Name = "Dave Hanna", PhoneNumber = "214-641-9986"});
	    }

	    private void InitRaceData()
	    {
		    var distance = _distanceData.Find(d => d.RaceDistance == "5K").SingleOrDefault();
			var _event = _eventData.Find(e => e.EventName == "Test").SingleOrDefault();
			if (distance == null || _event == null)
				throw new InvalidOperationException("Cannont initialize Races - Distance or Event cannot be found");
	    }

	    private void InitEventData()
	    {
		    _eventData.Add(new Event() { EventName = "Test"});
		    _eventData.SaveChanges();
	    }

	    private void InitDistanceData()
	    {
		    _distanceData.Add(new Distance(){RaceDistance = "1 Mile"});
			_distanceData.Add(new Distance() {RaceDistance = "2 Mile"});
			_distanceData.Add(new Distance() {RaceDistance = "5K"}) ;
		    _distanceData.SaveChanges();
	    }

	    private void InitPhotoData()
	    {
		    return;
	    }

	    public int SaveChanges()
	    {
		    int numChanges = _distanceData.SaveChanges() + _eventData.SaveChanges() + 
		                     _photoData.SaveChanges();

		    return numChanges;

	    }



		public void Dispose()
		{
			
		}


		public void Update(object item)
		{
			throw new NotImplementedException();
		}
	}

   
}
