using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace StaticVoid.Core.Repository.Test
{
    [TestClass]
    public class EntityFrameworkTests
    {
        public IRepository<TestEntity> _testRepo;
        public TestContext _context;

        [TestInitialize]
        public void Initalize()
        {
            _context = new TestContext();
            if (_context.Database.Exists())
            {
                _context.Database.Delete();
            }
            _context.Database.CreateIfNotExists();
            _testRepo = new SimpleRepository<TestEntity>(new DbContextRepositoryDataSource<TestEntity>(_context));
        }

        [TestCleanup]
        public void TearDown()
        {
            _testRepo.Dispose();
            _context.Dispose();
        }

        [TestMethod]
        public void CreateEntity()
        {
            Assert.AreEqual(0,_testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.Count());

            TestEntity t = new TestEntity { Something = "zzz" };
            _testRepo.Create(t);

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("zzz", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.Count());
            Assert.AreEqual("zzz", _context.TestEntities.First().Something);
        }


        [TestMethod]
        public void DeleteEntity()
        {
            Assert.AreEqual(0, _testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.AsNoTracking().Count());

            TestEntity t = new TestEntity { Something = "zzz" };
            using (var ctx = new TestContext())
            {
                ctx.TestEntities.Add(t);
                ctx.SaveChanges();
            }

            t = new TestEntity { Id = t.Id, Something = "zzz" };

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("zzz", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("zzz", _context.TestEntities.AsNoTracking().First().Something);

            _testRepo.Delete(t);

            Assert.AreEqual(0, _testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.AsNoTracking().Count());
        }

        [TestMethod]
        public void UpdateEntity()
        {
            Assert.AreEqual(0, _testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.AsNoTracking().Count());

            TestEntity t = new TestEntity { Something = "zzz" };
            using (var ctx = new TestContext())
            {
                ctx.TestEntities.Add(t);
                ctx.SaveChanges();
            }

            t = new TestEntity { Id = t.Id, Something = "zzz" };

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("zzz", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("zzz", _context.TestEntities.AsNoTracking().First().Something);

            _testRepo.Delete(t);

            Assert.AreEqual(0, _testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.AsNoTracking().Count());
        }
    }

    public class TestContext : DbContext
    {
        public IDbSet<TestEntity> TestEntities { get; set; }
    }

    public class TestEntity{
        public int Id { get; set; }
        public string Something { get; set; }
    }
}
