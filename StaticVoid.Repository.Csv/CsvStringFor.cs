using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository.Csv
{
	public interface ICsvStringFor<T> where T : class
	{
		string CsvString { get; set; }
	}

	public class CsvStringFor<T> : ICsvStringFor<T> where T : class
	{
		public string CsvString { get; set; }
	}
}
