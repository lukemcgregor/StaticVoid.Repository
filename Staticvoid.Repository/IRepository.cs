using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace StaticVoid.Repository
{
	public interface IRepository<T> : IReadonlyRepository<T> where T : class
    {
        void Create(T entity);

        void Delete(T entity);

        void Update(T entity);
    }
}
