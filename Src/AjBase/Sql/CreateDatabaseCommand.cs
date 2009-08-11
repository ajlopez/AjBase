namespace AjBase.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CreateDatabaseCommand : ICommand
    {
        private string name;

        public CreateDatabaseCommand(string name)
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        #region ICommand Members

        public void Execute(Engine engine)
        {
            engine.CreateDatabase(this.name);
        }

        public void Execute(Database database)
        {
            throw new InvalidOperationException();
        }

        #endregion
    }
}
