using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AjBase.Data;
using System.Data;

namespace AjBase.Tests.Data
{
    [TestClass]
    public class AjBaseConnectionTests
    {
        [TestMethod]
        public void SetConnectionString()
        {
            AjBaseConnection connection = new AjBaseConnection();
            connection.ConnectionString = "Database=MyEnterprise";

            Assert.AreEqual("Database=MyEnterprise", connection.ConnectionString);
        }

        [TestMethod]
        public void CreateWithConnectionString()
        {
            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise");

            Assert.AreEqual("Database=MyEnterprise", connection.ConnectionString);
        }

        [TestMethod]
        public void GetDatabase()
        {
            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise");

            Assert.AreEqual("MyEnterprise", connection.Database);
        }

        [TestMethod]
        public void GetDatasource()
        {
            AjBaseConnection connection = new AjBaseConnection("Data Source=MyDataSource");

            Assert.AreEqual("MyDataSource", connection.DataSource);
        }

        [TestMethod]
        public void GetNullDatasourceIfNotDefined()
        {
            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise");

            Assert.IsNull(connection.DataSource);
        }

        [TestMethod]
        public void GetDatabaseAndDatasource()
        {
            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise;Data Source=MyDataSource");

            Assert.AreEqual("MyEnterprise", connection.Database);
            Assert.AreEqual("MyDataSource", connection.DataSource);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfConnectionStringIsEmpty()
        {
            AjBaseConnection connection = new AjBaseConnection("");

            Assert.AreEqual("MyEnterprise", connection.Database);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfConnectionStringIsWhiteSpace()
        {
            AjBaseConnection connection = new AjBaseConnection("  ");

            Assert.AreEqual("MyEnterprise", connection.Database);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfConnectionStringIsNameOrValueAreEmpty()
        {
            AjBaseConnection connection = new AjBaseConnection("=");

            Assert.AreEqual("MyEnterprise", connection.Database);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDatabaseDefinedTwice()
        {
            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise;Database=OtherEnterprise");

            Assert.AreEqual("MyEnterprise", connection.Database);
        }

        [TestMethod]
        public void OpenAndCloseExistingDatabase()
        {
            Engine engine = new Engine();
            engine.CreateDatabase("MyEnterprise");
            Engine.Current = engine;

            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise");

            connection.Open();
            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();
            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }

        [TestMethod]
        public void CreateDatabaseDatabase()
        {
            Engine engine = new Engine();
            Engine.Current = engine;

            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise;CreateIfNotExists=true");

            connection.Open();
            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();
            Assert.AreEqual(ConnectionState.Closed, connection.State);

            Assert.IsNotNull(engine.GetDatabase("MyEnterprise"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RaiseIfDatabaseDoesNotExistWhenOpen()
        {
            AjBaseConnection connection = new AjBaseConnection("Database=MyEnterprise");

            connection.Open();
        }
    }
}
