namespace AjBase.Tests.Sql
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;
    using AjBase.Sql;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SelectCommandsTests
    {
        private Engine engine;
        private Database database;
        private Table table;

        [TestInitialize]
        public void SetUpTable()
        {
            this.engine = new Engine();
            this.database = this.engine.CreateDatabase("Sales");
            this.table = database.CreateTable("Employees");
            this.table.AddColumn(new Column("FirstName"));
            this.table.AddColumn(new Column("LastName"));
            this.table.AddRow(new object[] { "John", "Smith" });
            this.table.AddRow(new object[] { "Adam", "Jones" });
            this.table.AddRow(new object[] { "Alice", "Stuart" });
        }

        [TestMethod]
        public void ParseAndExecuteSelectCommand()
        {
            SelectCommand cmd = (SelectCommand)GetCommand("select * from Employees");

            cmd.Execute(this.database);

            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RowList));

            RowList list = (RowList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("John", list[0]["FirstName"]);
            Assert.AreEqual("Smith", list[0]["LastName"]);

            Row row = list[0];

            Assert.AreEqual(2, row.ColumnCount);
            Assert.AreEqual("FirstName", row.GetColumn(0).Name);
            Assert.AreEqual("LastName", row.GetColumn(1).Name);
        }

        [TestMethod]
        public void ParseAndExecuteSelectCommandWithFields()
        {
            SelectCommand cmd = (SelectCommand)GetCommand("select LastName, FirstName from Employees");

            cmd.Execute(this.database);

            var result = cmd.Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RowList));

            RowList list = (RowList)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("John", list[0]["FirstName"]);
            Assert.AreEqual("Smith", list[0]["LastName"]);

            Row row = list[0];

            Assert.AreEqual(2, row.ColumnCount);
            Assert.AreEqual("LastName", row.GetColumn(0).Name);
            Assert.AreEqual("FirstName", row.GetColumn(1).Name);
        }

        private static ICommand GetCommand(string text)
        {
            Parser parser = new Parser(text);

            return parser.ParseCommand();
        }

        private void GetAndExecuteCommand(string text)
        {
            ICommand cmd = GetCommand(text);

            cmd.Execute(this.database);
        }
    }
}
