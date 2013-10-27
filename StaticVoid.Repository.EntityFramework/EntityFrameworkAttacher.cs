using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public class EntityFrameworkAttacher<T> : IAttacher<T> where T: class
    {
        private DbContext m_Context;

        public EntityFrameworkAttacher(DbContext context)
        {
            m_Context = context;
        }

        public void EnsureAttached(T entity)
        {
            EnsureAttachedEF(entity);
        }

        public DbEntityEntry<T> EnsureAttachedEF(T entity)
        {
            var e = m_Context.Entry(entity);
            if (e.State == EntityState.Detached)
            {
                m_Context.Set<T>().Attach(entity);
                e = m_Context.Entry(entity);
            }

            return e;
        }
    }
}
