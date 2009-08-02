namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IRowFilter
    {
        Func<Row, bool> RowPredicate { get; }
    }
}
