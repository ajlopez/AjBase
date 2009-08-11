namespace AjBase.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ICommand
    {
        void Execute(Engine engine);

        void Execute(Database database);
    }
}
