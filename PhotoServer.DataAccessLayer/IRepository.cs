using PhotoServer.Domain;
using System;
using System.Collections.Generic;

namespace PhotoServer.DataAccessLayer
{
    public interface IRepository<T, TKey> where T : IEntity<TKey>
    {
        void Add(T item);
        void Remove(T item);
        List<T> FindAll();
        T FindById(TKey id);
        List<T> Find(Func<T, bool> predicate);

    }
}
