using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public interface IReferenceRepository<T, TKey> where T: IEntity<TKey>
	{
        IQueryable<T> FindAll();
        T FindById(TKey id);
        IQueryable<T> Find(Func<T, bool> predicate);
	}
}
