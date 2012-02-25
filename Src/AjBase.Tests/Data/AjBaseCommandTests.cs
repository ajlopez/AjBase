using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AjBase.Data;

namespace AjBase.Tests.Data
{
    [TestClass]
    public class AjBaseCommandTests
    {
        private Engine engine;
        private Database database;
        private Table table;

        // TODO review DRY in tests
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
            Engine.Current = this.engine;
        }

        [TestMethod]
        public void ExecuteInsertCommand()
        {
            int initial = this.table.GetRows().Count;
            AjBaseConnection connection = new AjBaseConnection("Database=Sales");
            AjBaseCommand command = new AjBaseCommand("insert into Employees(FirstName, LastName) values('New First Name', 'New Last Name')", connection);

            connection.Open();
            int n = command.ExecuteNonQuery();
            connection.Close();

            Assert.AreEqual(1, n);

            Assert.AreEqual(initial + 1, this.table.GetRows().Count());
            Row row = this.table.GetRows().Last();

            Assert.AreEqual("New First Name", row["FirstName"]);
            Assert.AreEqual("New Last Name", row["LastName"]);
        }

        [TestMethod]
        public void ExecuteCreateCommand()
        {
            int initial = this.table.GetRows().Count;
            AjBaseConnection connection = new AjBaseConnection("Database=Sales");
            AjBaseCommand command = new AjBaseCommand("create table Customers(Name, Address)", connection);

            connection.Open();
            // TODO test return
            command.ExecuteNonQuery();
            connection.Close();

            Table table = this.database.GetTable("Customers");
            Assert.IsNotNull(table);
        }
    }
}
