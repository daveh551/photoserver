using System.Linq;
using PhotoServer.Domain;
using System;
using System.Collections.Generic;

namespace PhotoServer.DataAccessLayer
{
    public interface IRepository<T, TKey> where T : IEntity<TKey>
    {
        void Add(T item);
        void Remove(T item);
        IQueryable<T> FindAll();
        T FindById(TKey id);
        IQueryable<T> Find(Func<T, bool> predicate);
       

    }
}
