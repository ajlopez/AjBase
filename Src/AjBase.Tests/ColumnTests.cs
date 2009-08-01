namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ColumnTests
    {
        private Engine engine;
        private Database database;
        private Table table;

        [TestInitialize]
        public void SetUpDatabase()
        {
            this.engine = new Engine();
            this.database = this.engine.CreateDatabase("foo");
            this.table = this.database.CreateTable("customers");
        }

        [TestMethod]
        public void CreateWithName() 
        {
            Column column = new Column("FirstName");

            Assert.AreEqual("FirstName", column.Name);
            Assert.IsNull(column.Table);
            Assert.IsNull(column.Type);
        }

        [TestMethod]
        public void CreateWithNameAndType()
        {
            Column column = new Column("FirstName", typeof(string));

            Assert.AreEqual("FirstName", column.Name);
            Assert.IsNull(column.Table);
            Assert.AreEqual(typeof(string), column.Type);
        }

        [TestMethod]
        public void AddToTable()
        {
            Column column = new Column("FirstName", typeof(string));

            this.table.AddColumn(column);

            Assert.IsTrue(this.table == column.Table);
        }

        [TestMethod]
        public void AddToAndRetrieveFromTable()
        {
            Column column = new Column("FirstName", typeof(string));

            this.table.AddColumn(column);

            Column col = this.table.GetColumn("FirstName");

            Assert.IsNotNull(col);
            Assert.AreEqual("FirstName", col.Name);
            Assert.IsTrue(this.table == col.Table);
        }

        [TestMethod]
        public void AddToAndRetrieveFromTableUsingLowerCaseName()
        {
            Column column = new Column("FirstName", typeof(string));

            this.table.AddColumn(column);

            Column col = this.table.GetColumn("firstname");

            Assert.IsNotNull(col);
            Assert.AreEqual("FirstName", col.Name);
            Assert.IsTrue(this.table == col.Table);
        }
    }
}
