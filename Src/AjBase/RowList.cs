namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RowList : List<Row>
    {
        public RowList()
        {
        }

        public RowList(IEnumerable<Row> rowcollection)
            : base(rowcollection)
        {
        }

        public RowList ApplyFilter(IRowFilter filter)
        {
            return new RowList(this.Where(filter.RowPredicate));
        }
    }
}
