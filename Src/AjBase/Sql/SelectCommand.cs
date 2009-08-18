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

        public RowList Result { get { return this.result; } }

        public void AddColumn(string columnName)
        {
            this.columnNames.Add(columnName);
        }

        #region ICommand Members

        public void Execute(Engine engine)
        {
            throw new NotSupportedException();
        }

        public void Execute(Database database)
        {
            Table table = database.GetTable(this.tableName);

            this.result = table.GetRows();
        }

        #endregion
    }
}
