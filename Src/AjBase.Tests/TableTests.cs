namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TableTests
    {
        private Table table;

        [TestInitialize]
        public void SetUpTable()
        {
            Engine engine = new Engine();
            Database database = engine.CreateDatabase("Sales");
            this.table = database.CreateTable("Customers");
            this.table.AddColumn(new Column("FirstName"));
            this.table.AddColumn(new Column("LastName"));
        }

        [TestMethod]
        public void AddRow()
        {
            Row added = this.table.AddRow(new object[] { "John", "Doe" });

            Assert.AreEqual(1, this.table.RowCount);

            Assert.IsNotNull(added);

            Row row = this.table.GetRow(0);

            Assert.IsTrue(added == row);

            Assert.IsNotNull(row);
            Assert.AreEqual("John", row["FirstName"]);
            Assert.AreEqual("Doe", row["LastName"]);
        }

        [TestMethod]
        public void AddRowWithOnlyOneField()
        {
            Row added = this.table.AddRow(new object[] { "John" });

            Assert.AreEqual(1, this.table.RowCount);

            Assert.IsNotNull(added);

            Row row = this.table.GetRow(0);

            Assert.IsTrue(added == row);

            Assert.IsNotNull(row);
            Assert.AreEqual("John", row["FirstName"]);
            Assert.IsNull(row["LastName"]);
        }

        [TestMethod]
        public void AddTwoRows()
        {
            this.table.AddRow(new object[] { "John", "Doe" });
            this.table.AddRow(new object[] { "John2", "Doe2" });

            Assert.AreEqual(2, this.table.RowCount);

            Row row = this.table.GetRow(0);

            Assert.IsNotNull(row);
            Assert.AreEqual("John", row["FirstName"]);
            Assert.AreEqual("Doe", row["LastName"]);

            row = this.table.GetRow(1);

            Assert.IsNotNull(row);
            Assert.AreEqual("John2", row["FirstName"]);
            Assert.AreEqual("Doe2", row["LastName"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfNegativeNumberOfRow()
        {
            this.table.GetRow(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfRowDoesNotExist()
        {
            this.table.GetRow(0);
        }
    }
}

