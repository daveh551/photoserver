using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class RaceRepository : AbstractRepository<Race, int>
	{
		public RaceRepository(DbSet<Race> data) : base(data)
		{
			
		}

		public override IQueryable<Race> FindAll()
		{
			return Data.Include("Event").Include("Distance");
		}

		public override Race FindById(int id)
		{
			return Data.Include("Event").Include("Distance").SingleOrDefault(item => item.Id == id);
		}

		public override IQueryable<Race> Find(Func<Race, bool> predicate)
		{
			return base.Find(predicate).Include("Event").Include("Distance");
		}
	}
}
