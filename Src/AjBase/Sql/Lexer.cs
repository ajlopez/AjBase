namespace AjBase.Sql
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private const string NameCharacters = "_";
        private const string SeparatorCharacters = "(),.";

        private TextReader reader;
        private char lastChar;
        private bool hasChar = false;
        private Stack<Token> tokenStack = new Stack<Token>();

        public Lexer(TextReader reader)
        {
            this.reader = reader;
        }

        public Lexer(string text)
            : this(new StringReader(text))
        {
        }

        public void PushToken(Token token)
        {
            if (token != null)
                this.tokenStack.Push(token);
        }

        public Token NextToken()
        {
            if (this.tokenStack.Count > 0)
                return this.tokenStack.Pop();

            char ch;

            try
            {
                this.SkipBlanks();
                ch = this.NextChar();

                if (ch == '[')
                    return this.NextBracketName();

                if (SeparatorCharacters.IndexOf(ch) >= 0)
                    return new Token() { TokenType = TokenType.Separator, Value = ch.ToString() };

                if (ch == '\'')
                    return this.NextString();

                if (char.IsDigit(ch) || ch == '-')
                    return this.NextInteger(ch);

                if (char.IsLetter(ch))
                    return this.NextName(ch);

                throw new LexerException(string.Format("Invalid character '{0}'", ch));
            }
            catch (EndOfInputException)
            {
                return null;
            }
        }

        internal char NextChar()
        {
            if (this.hasChar)
            {
                this.hasChar = false;
                return this.lastChar;
            }

            int ch;
            ch = this.reader.Read();

            if (ch < 0)
            {
                throw new EndOfInputException();
            }

            return (char)ch;
        }

        internal void SkipUpToEndOfLine()
        {
            try
            {
                char ch = this.NextChar();

                while (ch != '\n' && ch != '\r')
                    ch = this.NextChar();

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }
        }

        private Token NextName(char firstChar)
        {
            string name;

            name = firstChar.ToString();

            char ch;

            try
            {
                ch = this.NextChar();

                while (NameCharacters.IndexOf(ch)>=0 || char.IsLetterOrDigit(ch))
                {
                    name += ch;
                    ch = this.NextChar();
                }
                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Name, Value = name };

            return token;
        }

        private Token NextBracketName()
        {
            string name = string.Empty;

            char ch;

            try
            {
                ch = this.NextChar();

                while (ch != ']')
                {
                    name += ch;
                    ch = this.NextChar();
                }
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Name, Value = name };

            return token;
        }

        private Token NextInteger(char firstChar)
        {
            string integer;

            integer = new string(firstChar, 1);

            char ch;

            try
            {
                ch = this.NextChar();

                while (char.IsDigit(ch))
                {
                    integer += ch;
                    ch = this.NextChar();
                }

                this.PushChar(ch);
            }
            catch (EndOfInputException)
            {
            }

            Token token = new Token() { TokenType = TokenType.Integer, Value = integer };

            return token;
        }

        private Token NextString()
        {
            string text = string.Empty;

            char ch;

            try
            {
                ch = this.NextChar();

                while (ch != '\'')
                {
                    if (ch == '\\')
                        ch = this.NextChar();

                    text += ch;
                    ch = this.NextChar();
                }
            }
            catch (EndOfInputException)
            {
                throw new LexerException("Unclosed string");
            }

            Token token = new Token() { TokenType = TokenType.String, Value = text };

            return token;
        }

        private void PushChar(char ch)
        {
            this.lastChar = ch;
            this.hasChar = true;
        }

        private void SkipBlanks()
        {
            char ch;

            ch = this.NextChar();

            while (char.IsWhiteSpace(ch) || ch == ';')
            {
                if (ch == ';')
                    this.SkipUpToEndOfLine();

                ch = this.NextChar();
            }

            this.PushChar(ch);
        }
    }
}
