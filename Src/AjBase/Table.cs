namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Table
    {
        private string name;
        private Schema schema;
        private List<Column> columns;

        public string Name { get { return this.name; } }

        public Schema Schema { get { return this.schema; } }
    }
}
