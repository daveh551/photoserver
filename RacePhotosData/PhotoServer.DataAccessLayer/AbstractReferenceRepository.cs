using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.Domain;

namespace PhotoServer.DataAccessLayer
{
	public abstract class AbstractReferenceRepository<T, TKey> : IReferenceRepository<T, TKey> where T: class, IEntity<TKey>
	{
		private DbSet<T> _data;
		public virtual DbSet<T> Data { get { return _data; } }
		
		protected AbstractReferenceRepository(DbSet<T> data)
		{
			_data = data;
		}
	

		public  virtual IQueryable<T> FindAll()
		{
			return Data;
		}

		public virtual T FindById(TKey id)
		{
			return Data.SingleOrDefault(item => item.Id.Equals(id));
		}

		public virtual IQueryable<T> Find(Func<T, bool> predicate)
		{
			return Data.Where(x => predicate(x));
		}

	}
}
