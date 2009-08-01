namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SchemaTests
    {
        private Schema schema;

        [TestInitialize]
        public void SetUp()
        {
            Engine engine = new Engine();
            Database database = engine.CreateDatabase("foo");
            this.schema = database.CreateSchema("bar");
        }

        [TestMethod]
        public void CreateTable()
        {
            Table table = this.schema.CreateTable("customers");

            Assert.IsNotNull(table);
            Assert.AreEqual("customers", table.Name);
            Assert.IsTrue(this.schema == table.Schema);
            Assert.IsTrue(this.schema.Database == table.Database);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfTableAlreadyExists()
        {
            this.schema.CreateTable("customers");
            this.schema.CreateTable("customers");
        }
    }
}
