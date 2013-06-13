using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class PhotographerRepository : AbstractRepository<Photographer,int>
	{
		public PhotographerRepository(DbSet<Photographer> data) : base(data)
		{
			
		}
	}
}
