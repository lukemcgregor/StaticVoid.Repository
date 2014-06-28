using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StaticVoid.Repository.Csv.Test
{
	[TestClass]
	public class CsvTests
	{
		[TestMethod]
		public void ReadSingleStringColumnFromCsv()
		{
			var csvString = new CsvStringFor<TestObject1>{
				CsvString = 
@"Property1
aaaa
bbbb"
			};

			var sut = new CsvStringRepositoryDataSource<TestObject1>(csvString);

			var result = sut.GetAll();

			Assert.AreEqual(2, result.Count());
			Assert.AreEqual("aaaa", result.ToArray()[0].Property1);
			Assert.AreEqual("bbbb", result.ToArray()[1].Property1);
		}

		[TestMethod]
		public void ReadMultipleStringColumnsFromCsv()
		{
			var csvString = new CsvStringFor<TestObject2>
			{
				CsvString =
@"Property1,Property2
aaaa,bbbb
bbbb,cccc"
			};

			var sut = new CsvStringRepositoryDataSource<TestObject2>(csvString);

			var result = sut.GetAll();

			Assert.AreEqual(2, result.Count());
			Assert.AreEqual("aaaa", result.ToArray()[0].Property1);
			Assert.AreEqual("bbbb", result.ToArray()[0].Property2);
			Assert.AreEqual("bbbb", result.ToArray()[1].Property1);
			Assert.AreEqual("cccc", result.ToArray()[1].Property2);
		}


		[TestMethod]
		public void ReadOutOfOrderColumnsFromCsv()
		{
			var csvString = new CsvStringFor<TestObject2>
			{
				CsvString =
@"Property2,Property1
aaaa,bbbb
bbbb,cccc"
			};

			var sut = new CsvStringRepositoryDataSource<TestObject2>(csvString);

			var result = sut.GetAll();

			Assert.AreEqual(2, result.Count());
			Assert.AreEqual("aaaa", result.ToArray()[0].Property2);
			Assert.AreEqual("bbbb", result.ToArray()[0].Property1);
			Assert.AreEqual("bbbb", result.ToArray()[1].Property2);
			Assert.AreEqual("cccc", result.ToArray()[1].Property1);
		}
	}

	public class TestObject1
	{
		public string Property1 { get; set; }
	}
	public class TestObject2
	{
		public string Property1 { get; set; }
		public string Property2 { get; set; }
	}
}
