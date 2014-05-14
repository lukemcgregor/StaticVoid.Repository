using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public interface IAsyncRepositoryDataSource<T> : IRepositoryDataSource<T> where T : class
    {
        Task SaveChangesAsync();
    }
}
