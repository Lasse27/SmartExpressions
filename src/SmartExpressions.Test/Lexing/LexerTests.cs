using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test.Lexing
{
	public class LexerTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{
		private static List<Token> Tokenize(string input)
		{
			Lexer tokenizer = new Lexer(input);
			Result<List<Token>> result = tokenizer.Run();
			Assert.Equal(Status.Ok, result.Status);
			return result.Value;
		}

		private static string TokenizeFailure(string input)
		{
			Lexer tokenizer = new Lexer(input);
			Result<List<Token>> result = tokenizer.Run();
			Assert.Equal(Status.Fail, result.Status);
			return result.Message;
		}

		// -------------------------------------------------------------------------
		// Empty / whitespace
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_EmptyString_ReturnsEmptyList()
		{
			Lexer tokenizer = new Lexer("");
			Result<List<Token>> result = tokenizer.Run();
			Assert.Equal(Status.Ok, result.Status);
			Assert.Empty(result.Value);
		}

		[Fact]
		public void Run_WhitespaceOnly_ReturnsEmptyList()
		{
			Lexer tokenizer = new Lexer("   \t\n");
			Result<List<Token>> result = tokenizer.Run();
			Assert.Equal(Status.Ok, result.Status);
			Assert.Empty(result.Value);
		}

		[Fact]
		public void Constructor_NullInput_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new Lexer(null!));

		// -------------------------------------------------------------------------
		// Delimiter tokens
		// -------------------------------------------------------------------------

		[Theory]
		[InlineData(",", TokenType.Comma)]
		public void Run_SingleDelimiter_ReturnsCorrectToken(string input, TokenType expectedType)
		{
			List<Token> tokens = Tokenize(input);
			_ = Assert.Single(tokens);
			Assert.Equal(expectedType, tokens[0].Type);
		}



		// -------------------------------------------------------------------------
		// Identifier tokens
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_ValidIdentifier_ReturnsIdentifierToken()
		{
			List<Token> tokens = Tokenize("@{TEMP_1}");
			_ = Assert.Single(tokens);
			Assert.Equal(TokenType.Identifier, tokens[0].Type);
			Assert.Equal("TEMP_1", tokens[0].Lexeme);
		}

		[Fact]
		public void Run_IdentifierWithWhitespace_ReturnsIdentifierToken()
		{
			List<Token> tokens = Tokenize("  @{MY_VAR}  ");
			_ = Assert.Single(tokens);
			Assert.Equal(TokenType.Identifier, tokens[0].Type);
			Assert.Equal("MY_VAR", tokens[0].Lexeme);
		}

		[Fact]
		public void Run_AtWithoutBrace_ReturnsFailure()
		{
			string message = TokenizeFailure("@X");
			Assert.Contains("Expected", message);
		}

		[Fact]
		public void Run_UnclosedIdentifier_ReturnsFailure()
		{
			string message = TokenizeFailure("@{TEMP_1");
			Assert.Contains("Unclosed identifier", message);
		}

		[Fact]
		public void Run_EmptyIdentifier_ReturnsFailure()
		{
			string message = TokenizeFailure("@{}");
			Assert.Contains("Empty identifier", message);
		}

		[Fact]
		public void Run_WhitespaceOnlyIdentifier_ReturnsFailure()
		{
			string message = TokenizeFailure("@{   }");
			Assert.Contains("Empty identifier", message);
		}

		// -------------------------------------------------------------------------
		// Compound expressions
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_IdentifierCommaIdentifier_ReturnsThreeTokens()
		{
			// @{TEMP_1}==@{TEMP_2}  — no spaces
			List<Token> tokens = Tokenize("@{TEMP_1},@{TEMP_2}");
			Assert.Equal(3, tokens.Count);
			Assert.Equal(TokenType.Identifier, tokens[0].Type);
			Assert.Equal(TokenType.Comma, tokens[1].Type);
			Assert.Equal(TokenType.Identifier, tokens[2].Type);
		}

		[Fact]
		public void Run_And_Keyword_With_Params_Returns_Six_Tokens()
		{
			List<Token> tokens = Tokenize(" AND( @{TEMP_1} , @{TRUE} ) ");
			Assert.Equal(6, tokens.Count);
		}

		[Fact]
		public void Run_And_Keyword_With_Params_Returns_Correct_Sequence()
		{
			List<Token> tokens = Tokenize(" OR( @{TEMP_1} , @{TRUE} ) ");
			Assert.Equal(6, tokens.Count);
			Assert.Equal(TokenType.Keyword, tokens[0].Type);
			Assert.Equal(TokenType.LParen, tokens[1].Type);
			Assert.Equal(TokenType.Identifier, tokens[2].Type);
			Assert.Equal(TokenType.Comma, tokens[3].Type);
			Assert.Equal(TokenType.Identifier, tokens[4].Type);
			Assert.Equal(TokenType.RParen, tokens[5].Type);
		}

		[Fact]
		public void Run_UnknownCharacter_ReturnsFailure()
		{
			string message = TokenizeFailure("@{A} $ @{B}");
			Assert.Contains("Unexpected character", message);
		}

		// -------------------------------------------------------------------------
		// Position tracking
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_IdentifierPosition_PointsToAtSign()
		{
			List<Token> tokens = Tokenize("  @{ABC}");
			Assert.Equal(2, tokens[0].Position); // '@' is at index 2
		}

		// -------------------------------------------------------------------------
		// Custom Cases
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_Finds_Keyword_With_Number_And_Number_Correctly()
		{
			List<Token> tokens = Tokenize("ADD(125,123.0)");
			Assert.Equal(TokenType.Keyword, tokens[0].Type);
			Assert.Equal(TokenType.LParen, tokens[1].Type);
			Assert.Equal(TokenType.Numeric, tokens[2].Type);
			Assert.Equal(TokenType.Comma, tokens[3].Type);
			Assert.Equal(TokenType.Numeric, tokens[4].Type);
			Assert.Equal(TokenType.RParen, tokens[5].Type);
			foreach (Token item in tokens)
			{
				this._outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Identifier_And_Number_Correctly()
		{
			List<Token> tokens = Tokenize("ADD(@{Identifier_BE1_123},123.0)");
			Assert.Equal(TokenType.Keyword, tokens[0].Type);
			Assert.Equal(TokenType.LParen, tokens[1].Type);
			Assert.Equal(TokenType.Identifier, tokens[2].Type);
			Assert.Equal(TokenType.Comma, tokens[3].Type);
			Assert.Equal(TokenType.Numeric, tokens[4].Type);
			Assert.Equal(TokenType.RParen, tokens[5].Type);
			foreach (Token item in tokens)
			{
				this._outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Identifier_And_Identifier_Correctly()
		{
			List<Token> tokens = Tokenize("ADD(@{Identifier_BE1_123},@{Identifier_BE1_123})");
			Assert.Equal(TokenType.Keyword, tokens[0].Type);
			Assert.Equal(TokenType.LParen, tokens[1].Type);
			Assert.Equal(TokenType.Identifier, tokens[2].Type);
			Assert.Equal(TokenType.Comma, tokens[3].Type);
			Assert.Equal(TokenType.Identifier, tokens[4].Type);
			Assert.Equal(TokenType.RParen, tokens[5].Type);
			foreach (Token item in tokens)
			{
				this._outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Keyword_And_Keyword_Correctly()
		{
			List<Token> tokens = Tokenize("ADD(ADD(1,1),MULT(1,1))");
			Assert.Equal(TokenType.Keyword, tokens[0].Type);
			Assert.Equal(TokenType.LParen, tokens[1].Type);

			Assert.Equal(TokenType.Keyword, tokens[2].Type);
			Assert.Equal(TokenType.LParen, tokens[3].Type);
			Assert.Equal(TokenType.Numeric, tokens[4].Type);
			Assert.Equal(TokenType.Comma, tokens[5].Type);
			Assert.Equal(TokenType.Numeric, tokens[6].Type);
			Assert.Equal(TokenType.RParen, tokens[7].Type);

			Assert.Equal(TokenType.Comma, tokens[8].Type);

			Assert.Equal(TokenType.Keyword, tokens[9].Type);
			Assert.Equal(TokenType.LParen, tokens[10].Type);
			Assert.Equal(TokenType.Numeric, tokens[11].Type);
			Assert.Equal(TokenType.Comma, tokens[12].Type);
			Assert.Equal(TokenType.Numeric, tokens[13].Type);
			Assert.Equal(TokenType.RParen, tokens[14].Type);

			Assert.Equal(TokenType.RParen, tokens[15].Type);
			foreach (Token item in tokens)
			{
				this._outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Keyword_And_Identifier_Correctly()
		{
			List<Token> tokens = Tokenize("ADD(ADD(1,1),@{KEY})");
			Assert.Equal(TokenType.Keyword, tokens[0].Type);
			Assert.Equal(TokenType.LParen, tokens[1].Type);

			Assert.Equal(TokenType.Keyword, tokens[2].Type);
			Assert.Equal(TokenType.LParen, tokens[3].Type);
			Assert.Equal(TokenType.Numeric, tokens[4].Type);
			Assert.Equal(TokenType.Comma, tokens[5].Type);
			Assert.Equal(TokenType.Numeric, tokens[6].Type);
			Assert.Equal(TokenType.RParen, tokens[7].Type);

			Assert.Equal(TokenType.Comma, tokens[8].Type);

			Assert.Equal(TokenType.Identifier, tokens[9].Type);
			Assert.Equal(TokenType.RParen, tokens[10].Type);

			foreach (Token item in tokens)
			{
				this._outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Returns_Failure_On_Missing_Identifier_Bracket()
		{
			string str = TokenizeFailure("@{ Para1}\n@");
			this._outputHelper.WriteLine(str);
		}
	}
}