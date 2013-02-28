using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public class GuidRepository<T> : AbstractRepository<T, Guid> where T : IEntity<Guid>
	{

		public override void SaveChanges()
		{
			throw new NotImplementedException();
		}
	}
}
