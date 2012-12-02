using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace StaticVoid.Core.Repository
{
    public interface IRepository<T> : IDisposable
    {
        void Create(T entity);

        void Delete(T entity);

        void Update(T entity);

        IQueryable<T> GetAll();

        T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    }
}
