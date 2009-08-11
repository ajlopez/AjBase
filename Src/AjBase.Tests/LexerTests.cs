namespace AjBase.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using AjBase;
    using AjBase.Sql;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void ParseSimpleNames()
        {
            TestParse("select", "select", TokenType.Name);
            TestParse(" from ", "from", TokenType.Name);
            TestParse(" From  ", "From", TokenType.Name);
        }

        [TestMethod]
        public void ParseBracketNames()
        {
            TestParse("[dbo]", "dbo", TokenType.Name);
            TestParse("[complex name]", "complex name", TokenType.Name);
            TestParse("[my.customers]", "my.customers", TokenType.Name);
        }

        [TestMethod]
        public void ParseSimpleIntegers()
        {
            TestParse("123", "123", TokenType.Integer);
            TestParse(" 4567 ", "4567", TokenType.Integer);
            TestParse("-100", "-100", TokenType.Integer);
        }

        [TestMethod]
        public void ParseSeparators()
        {
            TestParse(".", ".", TokenType.Separator);
            TestParse(" , ", ",", TokenType.Separator);
            TestParse("(", "(", TokenType.Separator);
            TestParse(")", ")", TokenType.Separator);
        }

        [TestMethod]
        public void ParseStrings()
        {
            TestParse("''", "", TokenType.String);
            TestParse("'foo'", "foo", TokenType.String);
            TestParse("'  bar  '", "  bar  ", TokenType.String);
            TestParse(@"'\''", "'", TokenType.String);
        }

        [TestMethod]
        [ExpectedException(typeof(LexerException))]
        public void RaiseWhenUnclosedString()
        {
            TestParse("'foo", "foo", TokenType.String);
        }

        [TestMethod]
        public void ParseShortCommands()
        {
            TestParse("create table", "create", "table");
            TestParse("create table customers", "create", "table", "customers");
            TestParse("insert into customers(name,address)", "insert", "into", "customers", 
                "(", "name", ",", "address", ")");
        }

        [TestMethod]
        public void ParseSimpleCreateTable()
        {
            TestParse("create table customers(name varchar(50), address varchar(100))",
                "create", "table", "customers", "(", "name", "varchar", "(", "50", ")", ",",
                "address", "varchar", "(", "100", ")", ")");
        }

        [TestMethod]
        public void ParseSimpleInsert()
        {
            TestParse("insert into customers('Name','Address')",
                "insert", "into", "customers",
                "(", "Name", ",", "Address", ")");
        }

        private static void TestParse(string text, string value, TokenType type)
        {
            Lexer lexer = new Lexer(text);

            Token token = lexer.NextToken();

            Assert.IsNotNull(token);
            Assert.AreEqual(value, token.Value);
            Assert.AreEqual(type, token.TokenType);

            Assert.IsNull(lexer.NextToken());
        }

        private static void TestParse(string text, params string[] values)
        {
            Lexer lexer = new Lexer(text);

            Token token;

            for (int k = 0; k < values.Length; k++)
            {
                token = lexer.NextToken();
                Assert.IsNotNull(token);
                Assert.AreEqual(values[k], token.Value);
            }

            Assert.IsNull(lexer.NextToken());
        }
    }
}
