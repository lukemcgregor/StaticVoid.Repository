using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository.AzureStorage
{
    public class StorageContainer<T> : List<T>, IStorageContainer<T> 
    {
        public StorageContainer()
        {
            ToRemove = new List<T>();
			Public = false;
        }

		public bool Public { get; set; }

        internal List<T> ToRemove { get; set; }

        public void RemoveEntity(T entity)
        {
            ToRemove.Add(entity);
        }
    }
}
