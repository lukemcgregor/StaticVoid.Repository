using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
	public class CsvStringRepositoryDataSource<T> : IRepositoryDataSource<T> where T :class
	{
		private readonly ICsvStringFor<T> m_CsvString;
		protected IEnumerable<T> m_Items;

		protected CsvStringRepositoryDataSource() { }

		public CsvStringRepositoryDataSource(ICsvStringFor<T> csvString)
		{
			m_CsvString = csvString;
			m_Items = new CsvReader(new StringReader(m_CsvString.CsvString)).GetRecords<T>().ToList();
		}

		private List<T> m_ToAdd = new List<T>();
	
		public void AddOnSave(T entity)
		{
			m_ToAdd.Add(entity);
		}

		public IQueryable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
		{
			return m_Items.AsQueryable();
		}

		public T GetBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
		{
			return m_Items.SingleOrDefault(predicate.Compile());
		}

		private List<T> m_ToRemove = new List<T>();
	
		public void RemoveOnSave(T entity)
		{
			m_ToRemove.Add(entity);
		}

		public virtual void SaveChanges()
		{
			var sw = new StringWriter();
			new CsvWriter(sw).WriteRecords(m_Items);
			m_CsvString.CsvString = sw.ToString();
			
		}

		public void UpdateOnSave(T entity)
		{
			//nothing
		}

		public void Dispose()
		{
			//nothing
		}
	}
}
