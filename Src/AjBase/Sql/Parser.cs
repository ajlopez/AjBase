namespace AjBase.Sql
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Parser
    {
        private Lexer lexer;

        public Parser(string text)
            : this(new Lexer(text))
        {
        }

        public Parser(TextReader reader)
            : this(new Lexer(reader))
        {
        }

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public ICommand ParseCommand()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return null;

            if (IsToken(token, "create", TokenType.Name))
                return this.ParseCreate();

            if (IsName(token, "insert"))
                return this.ParseInsert();

            if (IsName(token, "select"))
                return this.ParseSelect();

            throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }

        private static bool IsName(Token token, string value)
        {
            return IsToken(token, value, TokenType.Name);
        }

        private static bool IsToken(Token token, string value, TokenType type)
        {
            if (token == null)
                return false;

            if (token.TokenType != type)
                return false;

            if (type == TokenType.Name)
                return token.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase);

            return token.Value.Equals(value);
        }

        private ICommand ParseCreate()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                throw new ParserException(string.Format("Unexpected end of input"));

            if (IsName(token, "database"))
                return this.ParseCreateDatabase();

            if (IsName(token, "table"))
                return this.ParseCreateTable();

            throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }

        private ICommand ParseCreateDatabase()
        {
            return new CreateDatabaseCommand(this.ParseName());
        }

        private ICommand ParseCreateTable()
        {
            string name = this.ParseName();

            CreateTableCommand cmd = new CreateTableCommand(name);

            this.Parse("(", TokenType.Separator);

            while (!this.TryParse(")", TokenType.Separator))
            {
                cmd.AddColumn(this.ParseColumnDefinition());

                if (this.TryParse(")", TokenType.Separator))
                    break;

                this.Parse(",", TokenType.Separator);
            }

            return cmd;
        }

        private ICommand ParseInsert()
        {
            this.Parse("into", TokenType.Name);
            string tableName = this.ParseName();

            InsertCommand cmd = new InsertCommand(tableName);

            if (this.TryParse("(", TokenType.Separator))
            {
                string columnName = this.ParseName();

                cmd.AddColumn(columnName);

                while (!this.TryParse(")", TokenType.Separator))
                {
                    this.Parse(",", TokenType.Separator);
                    columnName = this.ParseName();
                    cmd.AddColumn(columnName);
                }
            }

            this.Parse("values", TokenType.Name);
            this.Parse("(", TokenType.Separator);

            object value = this.ParseValue();

            cmd.AddValue(value);

            while (!this.TryParse(")", TokenType.Separator))
            {
                this.Parse(",", TokenType.Separator);
                value = this.ParseValue();
                cmd.AddValue(value);
            }

            return cmd;
        }

        private ICommand ParseSelect()
        {
            this.Parse("*", TokenType.Operator);
            this.Parse("from", TokenType.Name);

            string tableName = this.ParseName();

            return new SelectCommand(tableName);
        }

        private bool TryPeekName()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return false;

            this.lexer.PushToken(token);

            return token.TokenType == TokenType.Name;
        }

        private object ParseValue()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                throw new ParserException("Unexpected end of input");

            if (token.TokenType == TokenType.String)
                return token.Value;

            if (token.TokenType == TokenType.Integer)
                return int.Parse(token.Value);

            throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }

        private Column ParseColumnDefinition()
        {
            string name = this.ParseName();

            return new Column(name);
        }

        private bool TryParse(string value, TokenType type)
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                return false;

            if (type == TokenType.Name)
            {
                if (IsName(token, value))
                    return true;
                else
                {
                    this.lexer.PushToken(token);
                    return false;
                }
            }

            if (token.TokenType == type && token.Value == value)
                return true;

            this.lexer.PushToken(token);
            return false;
        }

        private void Parse(string value, TokenType type)
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                throw new ParserException(string.Format("Unexpected end of input"));

            if (type == TokenType.Name)
                if (IsName(token, value))
                    return;
                else
                    throw new ParserException(string.Format("Unexpected '{0}'", token.Value));

            if (token.TokenType != type || token.Value != value)
                throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }

        private string ParseName()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                throw new ParserException(string.Format("Unexpected end of input"));

            if (token.TokenType == TokenType.Name)
                return token.Value;

            throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }
    }
}
