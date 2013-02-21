using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoServer.DataAccessLayer;

namespace PhotoServer_Tests.Support
{
    class FakeRepository<T,TKey> : IRepository<T, TKey> where T : PhotoServer.Domain.IEntity<TKey>
    {
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Remove(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> FindAll()
        {
            throw new NotImplementedException();
        }

        public T FindById(TKey id)
        {
            throw new NotImplementedException();
        }

        public List<T> Find(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
