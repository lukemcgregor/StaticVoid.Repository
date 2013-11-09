using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository.AzureStorage
{
	public interface IStorageItem
	{
		/// <summary>
		/// The Azure container this type is stored in, this must be avaliable on a new instance of
		/// this object
		/// </summary>
		string Container { get; }
        string Name { get; set; }
	}
}
