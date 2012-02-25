namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Row
    {
        private object[] data;
        private IRowDefinition rowDefinition;

        internal Row(IRowDefinition rowDefinition, object[] data)
        {
            this.rowDefinition = rowDefinition;
            this.data = new object[this.rowDefinition.ColumnCount];

            for (int k = 0; k < this.data.Length && k < data.Length; k++)
                this[k] = data[k];
        }

        public int ColumnCount { get { return this.data.Length; } }

        public Column GetColumn(int ncolumn)
        {
            return this.rowDefinition.GetColumn(ncolumn);
        }

        public Column GetColumn(string name)
        {
            return this.rowDefinition.GetColumn(name);
        }

        public object this[int index]
        {
            get { return this.data[index]; }
            set { this.data[index] = value; }
        }

        public object this[string columnName]
        {
            get { return this[this.rowDefinition.GetColumnPosition(columnName)]; }
            set { this[this.rowDefinition.GetColumnPosition(columnName)] = value; }
        }
    }
}
