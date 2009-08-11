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

        private static ICommand Compile(string text)
        {
            Parser parser = new Parser(text);

            return parser.ParseCommand();
        }
    }
}
