using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Reflection;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Data;

namespace StaticVoid.Repository
{
    public class DbContextRepositoryDataSource<T> : IRepositoryDataSource<T> where T : class
    {
        private DbContext m_Context;
        private DbSet<T> m_Set;
        private EntityFrameworkAttacher<T> m_Attacher;
        public DbContextRepositoryDataSource(DbContext context)
        {
            m_Context = context;
            m_Set = context.Set<T>();
            m_Attacher = new EntityFrameworkAttacher<T>(context);
        }

        public IQueryable<T> GetAll()
        {
            return m_Set.AsNoTracking().AsQueryable<T>();
        }

        public void AddOnSave(T entity)
        {
            m_Set.Add(entity);
        }

        public void RemoveOnSave(T entity)
        {
            try
            {
                var e = m_Attacher.EnsureAttachedEF(entity);
                e.State = EntityState.Deleted;
            }
            catch (InvalidOperationException ex)
            {
                throw new RepositoryTrackingException(
                    "An attempt was made to delete an entity you are already modifying, this may happen if you are trying to update using the same repository instance in two place", ex);
            }
        }

        public void UpdateOnSave(T entity)
        {
            try
            {
                var e = m_Attacher.EnsureAttachedEF(entity);
                e.State = EntityState.Modified;
            }
            catch (InvalidOperationException ex)
            {
                throw new RepositoryTrackingException(
                    "An attempt was made to update an entity you are already modifying, this may happen if you are trying to update using the same repository instance in two place", ex);
            }
        }

        public void SaveChanges()
        {
            m_Context.SaveChanges();
        }

        public void Dispose()
        {
            m_Context.Dispose();
        }


        public T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = GetAll();
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    result = result.Include(include);
                }
            }
            return result.FirstOrDefault(predicate);
        }
    }
}
