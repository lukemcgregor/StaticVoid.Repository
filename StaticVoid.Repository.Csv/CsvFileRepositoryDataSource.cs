using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository.Csv
{
	public class CsvFileRepositoryDataSource<T> : CsvStringRepositoryDataSource<T> where T:class
	{
		private readonly ICsvFileFor<T> m_CsvFile;

		public CsvFileRepositoryDataSource(ICsvFileFor<T> csvFile)
		{
			m_CsvFile = csvFile;
		}
	}
}
