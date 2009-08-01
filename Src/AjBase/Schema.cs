namespace AjBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Schema
    {
        private string name;
        private Database database;
        private Dictionary<string, Table> tables;

        public string Name { get { return this.name; } }

        public Database Database { get { return this.database; } }
    }
}
