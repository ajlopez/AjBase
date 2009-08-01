namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Database
    {
        private static Dictionary<string, Database> databases = new Dictionary<string, Database>();

        private string name;
        private Dictionary<string, Schema> schemas = new Dictionary<string,Schema>();

        public const string DefaultSchemaName = "dbo";

        public static void Initialize()
        {
            databases.Clear();
        }

        public static Database Create(string name)
        {
            if (databases.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Database {0} already exists", name));

            Database db = new Database(name);

            databases[name] = db;

            return db;
        }

        public static Database Get(string name)
        {
            return databases[name];
        }

        private Database(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }
    }
}
