namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;
    using AjBase.Sql;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TableCommandsTests
    {
        private Engine engine;
        private Database database;

        [TestInitialize]
        public void Setup()
        {
            this.engine = new Engine();
            this.database = this.engine.CreateDatabase("foo");
        }

        [TestMethod]
        public void ExecuteCreateTableCommand()
        {
            CreateTableCommand cmd = new CreateTableCommand("Customers");
            cmd.AddColumn(new Column("Name"));
            cmd.AddColumn(new Column("Address"));

            cmd.Execute(this.database);

            Table table = this.database.GetTable("Customers");

            Assert.IsNotNull(table);
            Assert.AreEqual(2, table.ColumnCount);
            Assert.IsNotNull(table.GetColumn("Name"));
            Assert.IsNotNull(table.GetColumn("Address"));
        }

        [TestMethod]
        public void ParseAndExecuteCreateTableCommand()
        {
            CreateTableCommand cmd = (CreateTableCommand) GetCommand("create table Customers(Name, Address)");

            cmd.Execute(this.database);

            Table table = this.database.GetTable("Customers");

            Assert.IsNotNull(table);
            Assert.AreEqual(2, table.ColumnCount);
            Assert.IsNotNull(table.GetColumn("Name"));
            Assert.IsNotNull(table.GetColumn("Address"));
        }

        [TestMethod]
        public void ParseAndExecuteInsertCommand()
        {
            GetAndExecuteCommand("create table Customers(Name, Address)");
            Table table = this.database.GetTable("Customers");
            Assert.IsNotNull(table);
            Assert.AreEqual(0, table.RowCount);

            InsertCommand cmd = (InsertCommand)GetCommand("insert into Customers('Name 1', 'Address 1')");

            cmd.Execute(this.database);

            Assert.AreEqual(1, table.RowCount);

            Row row = table.GetRow(0);

            Assert.AreEqual("Name 1", row["Name"]);
            Assert.AreEqual("Address 1", row["Address"]);
        }

        [TestMethod]
        public void ParseAndExecuteInsertCommandWithColumnNames()
        {
            GetAndExecuteCommand("create table Customers(Name, Address)");
            Table table = this.database.GetTable("Customers");
            Assert.IsNotNull(table);
            Assert.AreEqual(0, table.RowCount);

            InsertCommand cmd = (InsertCommand)GetCommand("insert into Customers(Name, Address) values('Name 1', 'Address 1')");

            cmd.Execute(this.database);

            Assert.AreEqual(1, table.RowCount);

            Row row = table.GetRow(0);

            Assert.AreEqual("Name 1", row["Name"]);
            Assert.AreEqual("Address 1", row["Address"]);
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
