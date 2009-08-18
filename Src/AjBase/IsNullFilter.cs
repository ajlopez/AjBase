namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IsNullFilter : IRowFilter
    {
        private Column column;
        private int position;

        public IsNullFilter(Column column)
        {
            this.column = column;
            this.position = column.Position;
        }

        public Func<Row, bool> RowPredicate { get { return r => r[this.position] == null; } }
    }
}
