using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace StaticVoid.Core.Repository
{
    public class InMemoryRepositoryDataSource<T> : IRepositoryDataSource<T>
    {
        private static List<T> m_EntitySet = new List<T>();
        private static List<T> m_TemporaryAddSet = new List<T>();
        private static List<T> m_TemporaryRemoveSet = new List<T>();

        public InMemoryRepositoryDataSource(List<T> existing = null)
        {
            m_EntitySet = existing ?? new List<T>();
        }

        public IQueryable<T> GetAll()
        {
            return m_EntitySet.AsQueryable<T>();
        }

        public void AddOnSave(T entity)
        {
            m_TemporaryAddSet.Add(entity);
        }

        public void RemoveOnSave(T entity)
        {
            m_TemporaryRemoveSet.Add(entity);
        }

        public void UpdateOnSave(T entity)
        {
        }

        public void SaveChanges()
        {
            m_EntitySet.AddRange(m_TemporaryAddSet);
            foreach (T item in m_TemporaryRemoveSet)
            {
                m_EntitySet.Remove(item);
            }
            m_TemporaryAddSet.Clear();
            m_TemporaryRemoveSet.Clear();
        }

        public void Dispose()
        {
        }


        public T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return m_EntitySet.AsQueryable().SingleOrDefault(predicate);
        }
    }
}
