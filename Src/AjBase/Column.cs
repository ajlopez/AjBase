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

        public string Name { get { return this.name; } }

        public Table Table { get { return this.table; } }
    }
}
