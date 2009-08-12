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
    public class DatabaseCommandsTests
    {
        private Engine engine;

        [TestInitialize]
        public void SetupEngine()
        {
            this.engine = new Engine();
        }

        [TestMethod]
        public void ExecuteCreateDatabaseCommand()
        {
            CreateDatabaseCommand cmd = new CreateDatabaseCommand("foo");

            cmd.Execute(this.engine);

            Database database = this.engine.GetDatabase("foo");
            Assert.IsNotNull(database);
            Assert.AreEqual("foo", database.Name);
        }

        [TestMethod]
        public void ParseAndExecuteCreateDatabaseCommand()
        {
            CreateDatabaseCommand cmd = (CreateDatabaseCommand) GetCommand("create database foo");

            cmd.Execute(this.engine);

            Database database = this.engine.GetDatabase("foo");
            Assert.IsNotNull(database);
            Assert.AreEqual("foo", database.Name);
        }

        private static ICommand GetCommand(string text)
        {
            Parser parser = new Parser(text);

            return parser.ParseCommand();
        }
    }
}
