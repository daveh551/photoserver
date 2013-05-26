using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.DataAccessLayer;
using PhotoServer.Domain;

namespace RacePhotosTestSupport
{
	public class FakeIntReferenceRepository<T> :AbstractFakeRepository<T, int>, IReferenceRepository<T,int> where T: IEntity<int>
	{

		public override int SaveChanges()
		{
			throw new NotImplementedException();
		}

		public override T FindById(int id)
		{
			return data.FirstOrDefault(r => r.Id == id);
		}

	}
}
