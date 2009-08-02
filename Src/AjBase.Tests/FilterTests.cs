namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FilterTests
    {
        private Table table;

        [TestInitialize]
        public void SetUpTable()
        {
            Engine engine = new Engine();
            Database database = engine.CreateDatabase("Northwind");
            this.table = database.CreateTable("Employees");

            this.table.AddColumn(new Column("FirstName"));
            this.table.AddColumn(new Column("LastName"));
            this.table.AddColumn(new Column("DepartmentID"));
            this.table.AddColumn(new Column("Address"));

            for (int k = 1; k <= 100; k++)
                this.table.AddRow(new object[] { "John " + k, "Doe " + k, (k%10) + 1, (k%2)==0 ? null : "Address " + k});
        }

        [TestMethod]
        public void GetRowsWithNullAddress()
        {
            IRowFilter filter = new IsNullFilter(this.table.GetColumn("Address"));

            RowList rows = this.table.ApplyFilter(filter);

            Assert.IsNotNull(rows);
            Assert.AreEqual(this.table.RowCount / 2, rows.Count);

            foreach (Row row in rows)
                Assert.IsNull(row["Address"]);
        }

        [TestMethod]
        public void GetRowsWithNullFirstName()
        {
            IRowFilter filter = new IsNullFilter(this.table.GetColumn("FirstName"));

            RowList rows = this.table.ApplyFilter(filter);

            Assert.IsNotNull(rows);
            Assert.AreEqual(0, rows.Count);
        }

        [TestMethod]
        public void GetRowWithFirstName()
        {
            IRowFilter filter = new AreEqualsFilter(this.table.GetColumn("FirstName"), "John 10");

            RowList rows = this.table.ApplyFilter(filter);

            Assert.IsNotNull(rows);
            Assert.AreEqual(1, rows.Count);

            Row row = rows[0];

            Assert.AreEqual("John 10", row["FirstName"]);
            Assert.AreEqual("Doe 10", row["LastName"]);
            Assert.IsNull(row["Address"]);
            Assert.AreEqual(1, row["DepartmentID"]);
        }

        [TestMethod]
        public void GetRowWithAddress()
        {
            IRowFilter filter = new AreEqualsFilter(this.table.GetColumn("Address"), "Address 11");

            RowList rows = this.table.ApplyFilter(filter);

            Assert.IsNotNull(rows);
            Assert.AreEqual(1, rows.Count);

            Row row = rows[0];

            Assert.AreEqual("John 11", row["FirstName"]);
            Assert.AreEqual("Doe 11", row["LastName"]);
            Assert.AreEqual("Address 11", row["Address"]);
            Assert.AreEqual(2, row["DepartmentID"]);
        }
    }
}

