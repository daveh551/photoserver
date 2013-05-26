using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class PhotographerReferenceRepository : AbstractReferenceRepository<Photographer, int>
	{
		public PhotographerReferenceRepository(DbSet<Photographer> data) : base(data) 
		{
			
		}
	}
}
