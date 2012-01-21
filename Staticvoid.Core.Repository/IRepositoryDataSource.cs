using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace StaticVoid.Core.Repository
{
    public interface IRepositoryDataSource<T> : IDisposable
    {
        IQueryable<T> GetAll();
        void AddOnSave(T entity);
        void RemoveOnSave(T entity);
        void SaveChanges();
        T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
