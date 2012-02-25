namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RowDefinition : IRowDefinition
    {
        private IList<Column> columns = new List<Column>();

        public void AddColumn(Column column)
        {
            this.columns.Add(column);
        }

        public int ColumnCount
        {
            get { return this.columns.Count; }
        }

        public Column GetColumn(string name)
        {
            return this.columns.Single(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Column GetColumn(int ncol)
        {
            return this.columns[ncol];
        }

        public int GetColumnPosition(string name)
        {
            Column column = this.GetColumn(name);
            return this.columns.IndexOf(column);
        }
    }
}
