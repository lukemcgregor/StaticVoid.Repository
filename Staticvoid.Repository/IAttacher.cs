using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public interface IAttacher<T> where T : class
    {
        void EnsureAttached(T entity);
    }
}
