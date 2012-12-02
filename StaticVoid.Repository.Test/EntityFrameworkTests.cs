using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace StaticVoid.Repository.Test
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
            var ds = new DbContextRepositoryDataSource<TestEntity>(_context);
            _testRepo = new SimpleRepository<TestEntity>(ds);
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

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("zzz", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("zzz", _context.TestEntities.AsNoTracking().First().Something);

            t = new TestEntity { Id = t.Id, Something = "aaa" };

            _testRepo.Update(t);

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("aaa", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("aaa", _context.TestEntities.AsNoTracking().First().Something);
        }

        [TestMethod, ExpectedException(typeof(RepositoryTrackingException))]
        public void UpdateEntityTwiceWithDifferentVersionsShouldThrowAnException()
        {
            Assert.AreEqual(0, _testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.AsNoTracking().Count());

            TestEntity t = new TestEntity { Something = "zzz" };
            using (var ctx = new TestContext())
            {
                ctx.TestEntities.Add(t);
                ctx.SaveChanges();
            }

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("zzz", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("zzz", _context.TestEntities.AsNoTracking().First().Something);

            var t1 = new TestEntity { Id = t.Id, Something = "aaa" };
            var t2 = new TestEntity { Id = t.Id, Something = "bbb" };

            _testRepo.Update(t1);
            _testRepo.Update(t2);//exception
        }

        [TestMethod]
        public void SequentialOperationsOnTheSameObject()
        {
            Assert.AreEqual(0, _testRepo.GetAll().Count());
            Assert.AreEqual(0, _context.TestEntities.AsNoTracking().Count());

            TestEntity t = new TestEntity { Something = "zzz" };
            using (var ctx = new TestContext())
            {
                ctx.TestEntities.Add(t);
                ctx.SaveChanges();
            }

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("zzz", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("zzz", _context.TestEntities.AsNoTracking().First().Something);

            t.Something = "aaa";

            _testRepo.Update(t);

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("aaa", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("aaa", _context.TestEntities.AsNoTracking().First().Something);

            t.Something = "bbb";

            _testRepo.Update(t);

            Assert.AreEqual(1, _testRepo.GetAll().Count());
            Assert.AreEqual("bbb", _testRepo.GetAll().First().Something);

            Assert.AreEqual(1, _context.TestEntities.AsNoTracking().Count());
            Assert.AreEqual("bbb", _context.TestEntities.AsNoTracking().First().Something);

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
