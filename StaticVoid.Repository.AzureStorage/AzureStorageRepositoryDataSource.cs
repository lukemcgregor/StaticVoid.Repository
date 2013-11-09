using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository.AzureStorage
{
	public class AzureStorageRepositoryDataSource<T> : IRepositoryDataSource<T> where T : class, new()
	{
        private readonly AzureStorageContext _context;

		public AzureStorageRepositoryDataSource(AzureStorageContext context)
		{
            _context = context;
		}
		public void AddOnSave(T entity)
		{
			GetStorageContainer().Add(entity);
		}

		public IQueryable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
		{

            return GetStorageContainer().AsQueryable();
		}

        private IStorageContainer<T> GetStorageContainer()
        {
            return _context
                .GetType()
                .GetProperties()
                .Single(p => p.PropertyType == typeof(IStorageContainer<T>) || p.PropertyType == typeof(StorageContainer<T>))
                .GetValue(_context) as IStorageContainer<T>;
        }

		public T GetBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
		{
			return GetAll().SingleOrDefault(predicate);
		}

		public void RemoveOnSave(T entity)
        {
            GetStorageContainer().RemoveEntity(entity);
		}

		public void SaveChanges()
        {
            _context.SaveChanges();
		}

		public void UpdateOnSave(T entity)
		{
            GetStorageContainer().Add(entity);
		}

		public void Dispose()
		{
		}
	}
}
