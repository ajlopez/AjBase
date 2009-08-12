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
        private List<Column> columns = new List<Column>();
        private RowList rows = new RowList();

        internal Table(Schema schema, string name)
        {
            this.schema = schema;
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public Schema Schema { get { return this.schema; } }

        public Database Database { get { return this.schema.Database; } }

        public int RowCount { get { return this.rows.Count; } }

        public int ColumnCount { get { return this.columns.Count; } }

        public void AddColumn(Column column)
        {
            this.columns.Add(column);
            column.SetTable(this, this.columns.Count - 1);
        }

        public Column GetColumn(string name)
        {
            return this.columns.Single(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Row GetRow(int nrow)
        {
            if (nrow < 0 || nrow >= this.rows.Count)
                throw new InvalidOperationException("Invalid row number");

            return this.rows[nrow];
        }

        public Row AddRow(object[] data)
        {
            Row row = new Row(this, data);
            this.rows.Add(row);

            return row;
        }

        public RowList GetRows()
        {
            return new RowList(this.rows);
        }

        public RowList ApplyFilter(IRowFilter filter)
        {
            return this.rows.ApplyFilter(filter);
        }
    }
}
