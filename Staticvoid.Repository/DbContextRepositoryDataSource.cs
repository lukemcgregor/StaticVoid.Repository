using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Reflection;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Data;

namespace StaticVoid.Core.Repository
{
    public class DbContextRepositoryDataSource<T> : IRepositoryDataSource<T> where T : class
    {
        private DbContext m_Context;
        private DbSet<T> m_Set;
        public DbContextRepositoryDataSource(DbContext context)
        {
            m_Context = context;
            m_Set = context.Set<T>();
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
            var e = m_Context.Entry(entity);
            if (e.State == EntityState.Detached)
            {
                m_Context.Set<T>().Attach(entity);
                e = m_Context.Entry(entity);
            }
            e.State = EntityState.Deleted;
        }

        public void UpdateOnSave(T entity)
        {
            var e = m_Context.Entry(entity);
            if (e.State == EntityState.Detached)
            {
                m_Context.Set<T>().Attach(entity);
                e = m_Context.Entry(entity);
            }
            e.State = EntityState.Modified;
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
