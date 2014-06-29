using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StaticVoid.Repository
{
	public class CsvFileRepositoryDataSource<T> : CsvStringRepositoryDataSource<T> where T:class
	{
		private readonly ICsvFileFor<T> m_CsvFile;

		public CsvFileRepositoryDataSource(ICsvFileFor<T> csvFile)
		{
			m_CsvFile = csvFile;
			using (var reader = new StreamReader(m_CsvFile.CsvPath))
			{
				m_Items = new CsvReader(reader).GetRecords<T>().ToList();
			}
		}

		public override void SaveChanges()
		{
			using (var streamWriter = new StreamWriter(m_CsvFile.CsvPath))
			{
				new CsvWriter(streamWriter).WriteRecords(m_Items);
			}
		}
	}
}
