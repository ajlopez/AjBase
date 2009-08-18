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
    public class ParserTests
    {
        [TestMethod]
        public void CompileToNull()
        {
            Assert.IsNull(Compile(""));
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfUnknownFirstName()
        {
            Compile("foo create database Northwind");
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfInvalidFirstToken()
        {
            Compile("123 create database Northwind");
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfUnknownCreateToken()
        {
            Compile("create foo");
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfNoDatabaseName()
        {
            Compile("create database");
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void RaiseIfInvalidDatabaseName()
        {
            Compile("create database 123");
        }

        [TestMethod]
        public void CompileCreateDatabase()
        {
            ICommand cmd = Compile("create database Northwind");

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(CreateDatabaseCommand));

            CreateDatabaseCommand create = (CreateDatabaseCommand)cmd;

            Assert.AreEqual("Northwind", create.Name);
        }

        [TestMethod]
        public void CompileCreateTable()
        {
            ICommand cmd = Compile("create table Customers(name,address)");

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(CreateTableCommand));

            CreateTableCommand create = (CreateTableCommand)cmd;

            Assert.AreEqual("Customers", create.Name);

            ICollection<Column> columns = create.Columns;

            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Count);
            Assert.AreEqual("name", columns.ElementAt(0).Name);
            Assert.AreEqual("address", columns.ElementAt(1).Name);
        }

        [TestMethod]
        public void CompileSimpleInsertWithValues()
        {
            ICommand cmd = Compile("insert into Customers values('Name 1', 'Address 1')");

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(InsertCommand));

            InsertCommand insert = (InsertCommand)cmd;

            Assert.AreEqual("Customers", insert.TableName);
        }

        [TestMethod]
        public void CompileSimpleInsertWithColumnNamesAndValues()
        {
            ICommand cmd = Compile("insert into Customers(Name, Address) values('Name 1', 'Address 1')");

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(InsertCommand));

            InsertCommand insert = (InsertCommand)cmd;

            Assert.AreEqual("Customers", insert.TableName);
        }

        [TestMethod]
        public void CompileSimpleSelectWithAsterisk()
        {
            ICommand cmd = Compile("select * from Customers");

            Assert.IsNotNull(cmd);
            Assert.IsInstanceOfType(cmd, typeof(SelectCommand));

            SelectCommand select = (SelectCommand)cmd;

            Assert.AreEqual("Customers", select.TableName);
        }

        private static ICommand Compile(string text)
        {
            Parser parser = new Parser(text);

            return parser.ParseCommand();
        }
    }
}
