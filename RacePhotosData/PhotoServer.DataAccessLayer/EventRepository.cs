using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class EventRepository : AbstractRepository<Event, int>
	{
		public EventRepository(DbSet<Event> data) : base(data)
		{
			
		}
	}
}
