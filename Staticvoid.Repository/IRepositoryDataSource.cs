using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace StaticVoid.Repository
{
    public interface IRepositoryDataSource<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        void AddOnSave(T entity);
        void RemoveOnSave(T entity);
        void UpdateOnSave(T entity);
        void SaveChanges();
        T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
