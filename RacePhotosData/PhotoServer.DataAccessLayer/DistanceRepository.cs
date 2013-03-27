using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	class DistanceRepository : AbstractRepository<Distance, int>
	{
		public DistanceRepository(DbSet<Distance> data) : base(data)
		{
			
		}
	}
}
