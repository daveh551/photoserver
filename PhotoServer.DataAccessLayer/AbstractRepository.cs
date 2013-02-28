using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public abstract class AbstractRepository<T, TKey> : IRepository<T,TKey> where T:IEntity<TKey>
	{
		
		public virtual void Add(T item)
		{
			throw new NotImplementedException();
		}

		public virtual void Remove(T item)
		{
			throw new NotImplementedException();
		}

		public  virtual IEnumerable<T> FindAll()
		{
			throw new NotImplementedException();
		}

		public virtual T FindById(TKey id)
		{
			throw new NotImplementedException();
		}

		public virtual IEnumerable<T> Find(Func<T, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public abstract void SaveChanges();

	}
}
