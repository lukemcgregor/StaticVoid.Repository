using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StaticVoid.Repository.AzureStorage.Test
{
    [TestClass]
    public class ContextTests
    {
        [TestMethod]
        public void TestCollectionsCreated()
        {
            var sut = new TestContext();

            Assert.AreEqual(typeof(StorageContainer<Entity1>), sut.Entity1s.GetType());
            Assert.AreEqual(typeof(StorageContainer<Entity2>), sut.Entity2s.GetType());
        }
    }

    class TestContext : AzureStorageContext
    {
        public IStorageContainer<Entity1> Entity1s { get; set; }
        public IStorageContainer<Entity2> Entity2s { get; set; }
    }

    class Entity1
    {
    }
    class Entity2
    {
    }
}
