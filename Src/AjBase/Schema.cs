namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Schema
    {
        private string name;
        private Database database;
        private Dictionary<string, Table> tables = new Dictionary<string, Table>();

        internal Schema(Database database, string name)
        {
            this.name = name;
            this.database = database;
        }

        public string Name { get { return this.name; } }

        public Database Database { get { return this.database; } }

        public Table CreateTable(string name)
        {
            if (this.tables.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Table '{0}' already exists", name));

            Table table = new Table(this, name);

            this.tables[name] = table;

            return table;
        }
    }
}
