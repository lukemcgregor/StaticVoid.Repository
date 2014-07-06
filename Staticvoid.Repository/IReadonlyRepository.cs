using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
	public interface IReadonlyRepository<T> : IDisposable where T : class
	{
		IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

		T GetBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
	}
}
