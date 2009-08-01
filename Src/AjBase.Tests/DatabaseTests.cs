namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DatabaseTests
    {
        [TestInitialize]
        public void SetUpDatabase()
        {
            Database.Initialize();
        }

        [TestMethod]
        public void CreateDatabase()
        {
            Database db = Database.Create("foo");

            Assert.IsNotNull(db);
            Assert.AreEqual("foo", db.Name);
        }

        [TestMethod]
        public void CreateAndGetDatabase()
        {
            Database.Create("foo");

            Database db = Database.Get("foo");

            Assert.IsNotNull(db);
            Assert.AreEqual("foo", db.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDatabaseAlreadyExists()
        {
            Database.Create("foo");
            Database.Create("foo");
        }
    }
}
