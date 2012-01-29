namespace AjBase.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Common;
    using System.Data;

    public class AjBaseConnection : DbConnection, IDbConnection, IDisposable
    {
        private ConnectionState state = ConnectionState.Closed;
        private string connectionString;
        private IDictionary<string, string> connectionValues = null;
        private Database database;

        public AjBaseConnection()
        {
        }

        public AjBaseConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override string ConnectionString { get { return this.connectionString; } set { this.connectionString = value; this.connectionValues = null; } }

        public override ConnectionState State
        {
            get { return this.state; }
        }

        public override string ServerVersion
        {
            get { throw new NotImplementedException(); }
        }

        public override string DataSource
        {
            get { return this.GetConnectionValue("Data Source"); }
        }

        public override string Database
        {
            get { return this.GetConnectionValue("Database"); }
        }

        public override void Open()
        {
            string dbname = this.GetConnectionValue("Database");

            Engine engine = Engine.Current;

            if (engine == null)
                Engine.Current = engine = new Engine();

            this.database = Engine.Current.GetDatabase(dbname);

            if (this.database == null)
            {
                if (this.GetConnectionValue("CreateIfNotExists") == "true")
                    this.database = Engine.Current.CreateDatabase(dbname);
                else
                    throw new InvalidOperationException(string.Format("Database '{0}' does not exist", dbname));
            }

            this.state = ConnectionState.Open;
        }

        public override void Close()
        {
            this.state = ConnectionState.Closed;
            this.database = null;
        }

        protected override DbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        private string GetConnectionValue(string name)
        {
            if (string.IsNullOrEmpty(this.connectionString))
                throw new InvalidOperationException("Invalid Connection String");

            if (this.connectionValues == null)
            {
                string[] namevalues = this.connectionString.Split(';');

                if (namevalues.Length == 0)
                    throw new InvalidOperationException("Invalid Connection String");

                this.connectionValues = new Dictionary<string, string>();

                foreach (string namevalue in namevalues)
                {
                    string[] split = namevalue.Split('=');

                    if (split.Length != 2)
                        throw new InvalidOperationException("Invalid Connection String");

                    if (string.IsNullOrEmpty(split[0]) || string.IsNullOrEmpty(split[1]))
                        throw new InvalidOperationException("Invalid Connection String");

                    if (this.connectionValues.ContainsKey(split[0]))
                        throw new InvalidOperationException("Invalid Connection String");

                    this.connectionValues[split[0]] = split[1];
                }                
            }

            if (this.connectionValues.ContainsKey(name))
                return this.connectionValues[name];
            else
                return null;
        }
    }
}
