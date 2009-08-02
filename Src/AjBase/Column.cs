namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Column
    {
        private Table table;
        private string name;
        private Type type;
        private int position;

        public Column(string name)
            : this(name, null)
        {
        }

        public Column(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        public string Name { get { return this.name; } }

        public Table Table { get { return this.table; } }

        public Type Type { get { return this.type; } }

        public int Position { get { return this.position; } }

        internal void SetTable(Table table, int position)
        {
            if (this.table != null)
                throw new InvalidOperationException(string.Format("Column '{0}' is associated to Table '{1}'", this.name, this.table.Name));

            this.table = table;
            this.position = position;
        }
    }
}
