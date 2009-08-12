namespace AjBase.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class InsertCommand : ICommand
    {
        private string tableName;
        private List<string> columnNames = new List<string>();
        private List<object> values = new List<object>();

        public InsertCommand(string tableName)
        {
            this.tableName = tableName;
        }

        public string TableName { get { return this.tableName; } }

        public void AddColumn(string columnName)
        {
            this.columnNames.Add(columnName);
        }

        public void AddValue(object value)
        {
            this.values.Add(value);
        }

        #region ICommand Members

        public void Execute(Engine engine)
        {
            throw new NotSupportedException();
        }

        public void Execute(Database database)
        {
            Table table = database.GetTable(this.tableName);

            if (this.columnNames.Count==0)
                table.AddRow(values.ToArray());
            else
            {
                object[] allValues = new object[table.ColumnCount];

                for (int k = 0; k < this.columnNames.Count; k++)
                {
                    Column column = table.GetColumn(this.columnNames[k]);
                    allValues[column.Position] = values[k];
                }

                table.AddRow(allValues);
            }
        }

        #endregion
    }
}
