using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public interface IStorageContainer<T> : ICollection<T>
    {
        void RemoveEntity(T entity);
		bool Public { get; set; }
    }
}
