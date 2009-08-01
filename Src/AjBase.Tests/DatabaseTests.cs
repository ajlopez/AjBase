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
        private Engine engine;
        private Database database;

        [TestInitialize]
        public void SetUpDatabase()
        {
            this.engine = new Engine();
            this.database = this.engine.CreateDatabase("foo");
        }

        [TestMethod]
        public void VerifyCreatedDatabase()
        {
            Assert.IsNotNull(this.database);
            Assert.AreEqual("foo", this.database.Name);
            Assert.IsTrue(this.engine == this.database.Engine);

            Schema schema = this.database.GetSchema(Database.DefaultSchemaName);
            Assert.IsNotNull(schema);
            Assert.AreEqual(Database.DefaultSchemaName, schema.Name);
            Assert.IsTrue(this.database == schema.Database);
        }

        [TestMethod]
        public void GetCreatedDatabase()
        {
            Database db = this.engine.GetDatabase("foo");

            Assert.IsNotNull(db);
            Assert.AreEqual("foo", db.Name);
            Assert.IsTrue(this.database == db);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDatabaseAlreadyExists()
        {
            this.engine.CreateDatabase("foo");
            this.engine.CreateDatabase("foo");
        }

        [TestMethod]
        public void CreateNewSchema()
        {
            Schema schema = this.database.CreateSchema("bar");

            Assert.IsNotNull(schema);
            Assert.AreEqual("bar", schema.Name);
            Assert.IsTrue(this.database == schema.Database);
        }

        [TestMethod]
        public void GetDefaultSchema()
        {
            Schema schema = this.database.GetDefaultSchema();

            Assert.IsNotNull(schema);
            Assert.AreEqual(Database.DefaultSchemaName, schema.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfSchemaAlreadyExists()
        {
            this.database.CreateSchema("bar");
            this.database.CreateSchema("bar");
        }

        [TestMethod]
        public void CreateTableInDefaultSchema()
        {
            Table table = this.database.CreateTable("customers");

            Assert.IsNotNull(table);
            Assert.AreEqual("customers", table.Name);
            Assert.IsTrue(this.database == table.Database);
            Assert.IsTrue(this.database.GetDefaultSchema() == table.Schema);
        }
    }
}
