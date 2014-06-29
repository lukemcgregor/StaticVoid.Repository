using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
	public interface ICsvFileFor<T> where T : class
	{
		string CsvPath { get; set; }
	}

	public class CsvFileFor<T> where T:class
	{
		public string CsvPath { get; set; }
	}
}
