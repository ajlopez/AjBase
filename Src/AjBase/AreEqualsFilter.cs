namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AreEqualsFilter : IRowFilter
    {
        private Column column;
        private object value;
        private int position;

        public AreEqualsFilter(Column column, object value)
        {
            this.column = column;
            this.value = value;
            this.position = column.Position;
        }

        public Func<Row, bool> RowPredicate { get { return this.AreEquals; } }

        private bool AreEquals(Row row)
        {
            object rowvalue = row[this.position];

            if (rowvalue == null)
                return this.value == null;

            return rowvalue.Equals(value);
        }
    }
}
