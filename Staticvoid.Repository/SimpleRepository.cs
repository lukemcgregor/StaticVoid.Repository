using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public class SimpleRepository<T> : IAsyncRepository<T> where T : class
    {
        protected IRepositoryDataSource<T> Context{ get; set; }

        public SimpleRepository(IRepositoryDataSource<T> context) 
        {
            Context = context;
        }

        public virtual void Create(T entity)
        {
            Context.AddOnSave(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            Context.RemoveOnSave(entity);
            Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            Context.UpdateOnSave(entity);
            Context.SaveChanges();
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return Context.GetAll(includes);
        }

        public virtual void Dispose()
        {
            Context.Dispose();
        }

        public T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return Context.GetBy(predicate, includes);
        }

		public System.Threading.Tasks.Task CreateAsync(T entity)
		{
			Context.AddOnSave(entity);
			if (Context is IAsyncRepositoryDataSource<T>)
			{
				return (Context as IAsyncRepositoryDataSource<T>).SaveChangesAsync();
			}
			return Task.Run(() =>
			{
				Context.SaveChanges();
			});
		}

		public System.Threading.Tasks.Task DeleteAsync(T entity)
		{
			Context.RemoveOnSave(entity);
			if (Context is IAsyncRepositoryDataSource<T>)
			{
				return (Context as IAsyncRepositoryDataSource<T>).SaveChangesAsync();
			}
			return Task.Run(() =>
			{
				Context.SaveChanges();
			});
		}

		public System.Threading.Tasks.Task UpdateAsync(T entity)
		{
			Context.UpdateOnSave(entity);
			if (Context is IAsyncRepositoryDataSource<T>)
			{
				return (Context as IAsyncRepositoryDataSource<T>).SaveChangesAsync();
			}
			return Task.Run(() =>
			{
				Context.SaveChanges();
			});
		}
	}
}
