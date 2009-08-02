namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Row
    {
        private object[] data;
        private Table table;

        internal Row(Table table, object[] data)
        {
            this.table = table;
            this.data = new object[this.table.ColumnCount];

            for (int k = 0; k < this.data.Length && k < data.Length; k++)
                this[k] = data[k];
        }

        public object this[int index]
        {
            get { return this.data[index]; }
            set { this.data[index] = value; }
        }

        public object this[string columnName]
        {
            get { return this[this.table.GetColumn(columnName).Position]; }
            set { this[this.table.GetColumn(columnName).Position] = value; }
        }
    }
}
