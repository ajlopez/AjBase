namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Database
    {
        public const string DefaultSchemaName = "dbo";

        private string name;
        private Engine engine;

        private Dictionary<string, Schema> schemas = new Dictionary<string, Schema>();

        internal Database(Engine engine, string name)
        {
            this.name = name;
            this.engine = engine;
            this.CreateSchema(Database.DefaultSchemaName);
        }

        public string Name { get { return this.name; } }

        public Engine Engine { get { return this.engine; } }

        public Schema GetSchema(string name)
        {
            return this.schemas[name];
        }

        public Schema GetDefaultSchema()
        {
            return this.GetSchema(Database.DefaultSchemaName);
        }

        public Schema CreateSchema(string name)
        {
            if (this.schemas.ContainsKey(name))
                throw new InvalidOperationException(string.Format("Schema '{0}' already exists in Database '{0}'", name, this.name));

            Schema schema = new Schema(this, name);

            this.schemas[name] = schema;

            return schema;
        }

        public Table CreateTable(string name)
        {
            return this.GetDefaultSchema().CreateTable(name);
        }

        public Table GetTable(string name)
        {
            return this.GetDefaultSchema().GetTable(name);
        }
    }
}
