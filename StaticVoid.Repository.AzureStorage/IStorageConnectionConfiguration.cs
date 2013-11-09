using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository.AzureStorage
{
	public interface IStorageConnectionConfiguration
	{
		string ConnectionStringName { get; }
	}
}
