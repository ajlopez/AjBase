namespace AjBase.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CreateTableCommand : ICommand
    {
        private string name;
        private List<Column> columns = new List<Column>();

        public CreateTableCommand(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public ICollection<Column> Columns { get { return this.columns; } }

        public void AddColumn(Column column)
        {
            this.columns.Add(column);
        }

        #region ICommand Members

        public void Execute(Engine engine)
        {
            throw new InvalidOperationException();
        }

        public void Execute(Database database)
        {
            Table table = database.CreateTable(this.name);
            
            foreach (Column column in this.columns)
                table.AddColumn(column);
        }

        #endregion
    }
}
