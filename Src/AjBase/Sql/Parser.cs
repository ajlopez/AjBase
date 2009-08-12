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
            Token token = lexer.NextToken();

            if (token == null)
                return null;

            if (IsToken(token, "create", TokenType.Name))
                return ParseCreate();

            throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }

        private ICommand ParseCreate()
        {
            Token token = this.lexer.NextToken();

            if (token == null)
                throw new ParserException(string.Format("Unexpected end of input"));

            if (IsName(token, "database"))
                return ParseCreateDatabase();

            if (IsName(token, "table"))
                return ParseCreateTable();

            throw new ParserException(string.Format("Unexpected '{0}'", token.Value));
        }

        private ICommand ParseCreateDatabase()
        {
            return new CreateDatabaseCommand(this.ParseName());
        }

        private ICommand ParseCreateTable()
        {
            string name = ParseName();

            CreateTableCommand cmd = new CreateTableCommand(name);

            Parse("(", TokenType.Separator);

            while (!TryParse(")", TokenType.Separator))
            {
                cmd.AddColumn(ParseColumnDefinition());

                if (TryParse(")", TokenType.Separator))
                    break;

                Parse(",", TokenType.Separator);
            }

            return cmd;
        }

        private Column ParseColumnDefinition()
        {
            string name = ParseName();

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
    }
}
