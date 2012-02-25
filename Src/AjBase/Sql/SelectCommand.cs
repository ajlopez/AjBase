namespace AjBase.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SelectCommand : ICommand
    {
        private string tableName;
        private List<string> columnNames = new List<string>();
        private RowList result;

        public SelectCommand(string tableName)
        {
            this.tableName = tableName;
        }

        public string TableName { get { return this.tableName; } }

        public IEnumerable<string> ColumnNames { get { return this.columnNames; } }

        public RowList Result { get { return this.result; } }

        public void AddColumn(string columnName)
        {
            this.columnNames.Add(columnName);
        }

        public void Execute(Engine engine)
        {
            throw new NotSupportedException();
        }

        public void Execute(Database database)
        {
            Table table = database.GetTable(this.tableName);

            if (this.columnNames.Count == 0)
            {
                this.result = table.GetRows();
                return;
            }

            RowDefinition rowdef = new RowDefinition();
            IList<Row> rows = new List<Row>();

            foreach (var name in this.columnNames)
                rowdef.AddColumn(table.GetColumn(name));

            int nc = this.columnNames.Count;

            foreach (var row in table.GetRows())
            {
                object[] values = new object[nc];

                for (int k = 0; k < nc; k++)
                    values[k] = row[this.columnNames[k]];

                rows.Add(new Row(rowdef, values));
            }

            this.result = new RowList(rows);
        }
    }
}
