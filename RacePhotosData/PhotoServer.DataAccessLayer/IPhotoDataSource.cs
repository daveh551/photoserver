using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
    public interface IPhotoDataSource
    {
        IRepository<Photo, Guid> Photos { get; }
		IRepository<Event, int> Events { get; }
		IRepository<Distance, int> Distances { get; }
		IRepository<Race, int> Races { get; }
	    int SaveChanges();
    }
}
