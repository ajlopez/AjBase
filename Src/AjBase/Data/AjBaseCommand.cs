namespace AjBase.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Common;
    using System.Data;
    using AjBase.Sql;

    public class AjBaseCommand : DbCommand, IDbCommand, IDisposable
    {
        private AjBaseConnection connection;
        private string commandText;

        public AjBaseCommand()
        {
        }

        public AjBaseCommand(string commandText)
        {
            this.commandText = commandText;
        }

        public AjBaseCommand(string commandText, AjBaseConnection connection)
        {
            this.commandText = commandText;
            this.connection = connection;
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            Parser parser = new Parser(this.commandText);
            ICommand command = parser.ParseCommand();
            command.Execute(this.connection.GetDatabase());
            // TODO implement result count
            return 1;
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool DesignTimeVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override DbTransaction DbTransaction
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { throw new NotImplementedException(); }
        }

        protected override DbConnection DbConnection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = (AjBaseConnection)value;
            }
        }

        public override CommandType CommandType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string CommandText
        {
            get
            {
                return this.commandText;
            }
            set
            {
                this.commandText = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
