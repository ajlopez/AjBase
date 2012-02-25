namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IsNullFilter : IRowFilter
    {
        private int position;

        public IsNullFilter(int position)
        {
            this.position = position;
        }

        public Func<Row, bool> RowPredicate { get { return r => r[this.position] == null; } }
    }
}
