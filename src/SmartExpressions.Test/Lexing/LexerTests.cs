using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Tokens.Brackets;
using SmartExpressions.Core.Tokens.Delimiters;
using SmartExpressions.Core.Tokens.Registered;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test.Lexing
{
	public class LexerTests
	{
		private readonly ITestOutputHelper outputHelper;

		public LexerTests(ITestOutputHelper outputHelper)
			=> this.outputHelper = outputHelper;


		private static List<IToken> Tokenize(string input)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);
			return result.Value;
		}

		private static string TokenizeFailure(string input)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Failure, result.Status);
			return result.Message;
		}

		// -------------------------------------------------------------------------
		// Empty / whitespace
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_EmptyString_ReturnsEmptyList()
		{
			Lexer tokenizer = new Lexer("");
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Value);
		}

		[Fact]
		public void Run_WhitespaceOnly_ReturnsEmptyList()
		{
			Lexer tokenizer = new Lexer("   \t\n");
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Value);
		}

		[Fact]
		public void Constructor_NullInput_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new Lexer(null!));

		// -------------------------------------------------------------------------
		// Delimiter tokens
		// -------------------------------------------------------------------------

		[Theory]
		[InlineData(",", typeof(CommaToken))]
		[InlineData(".", typeof(DotToken))]
		[InlineData(":", typeof(ColonToken))]
		[InlineData(";", typeof(SemiColonToken))]
		public void Run_SingleDelimiter_ReturnsCorrectToken(string input, Type expectedType)
		{
			List<IToken> tokens = Tokenize(input);
			_ = Assert.Single(tokens);
			Assert.IsType(expectedType, tokens[0]);
		}

		// -------------------------------------------------------------------------
		// Bracket tokens
		// -------------------------------------------------------------------------

		[Theory]
		[InlineData("(", typeof(LParenToken))]
		[InlineData(")", typeof(RParenToken))]
		[InlineData("{", typeof(LBraceToken))]
		[InlineData("}", typeof(RBraceToken))]
		public void Run_SingleBracket_ReturnsCorrectToken(string input, Type expectedType)
		{
			List<IToken> tokens = Tokenize(input);
			_ = Assert.Single(tokens);
			Assert.IsType(expectedType, tokens[0]);
		}

		// -------------------------------------------------------------------------
		// Identifier tokens
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_ValidIdentifier_ReturnsIdentifierToken()
		{
			List<IToken> tokens = Tokenize("@{TEMP_1}");
			_ = Assert.Single(tokens);
			IdentifierToken token = Assert.IsType<IdentifierToken>(tokens[0]);
			Assert.Equal("TEMP_1", token.Value);
		}

		[Fact]
		public void Run_IdentifierWithWhitespace_ReturnsIdentifierToken()
		{
			List<IToken> tokens = Tokenize("  @{MY_VAR}  ");
			_ = Assert.Single(tokens);
			_ = Assert.IsType<IdentifierToken>(tokens[0]);
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
			List<IToken> tokens = Tokenize("@{TEMP_1},@{TEMP_2}");
			Assert.Equal(3, tokens.Count);
			_ = Assert.IsType<IdentifierToken>(tokens[0]);
			_ = Assert.IsType<CommaToken>(tokens[1]);
			_ = Assert.IsType<IdentifierToken>(tokens[2]);
		}

		[Fact]
		public void Run_And_Keyword_With_Params_Returns_Six_Tokens()
		{
			List<IToken> tokens = Tokenize(" AND( @{TEMP_1} , @{TRUE} ) ");
			Assert.Equal(6, tokens.Count);
		}

		[Fact]
		public void Run_And_Keyword_With_Params_Returns_Correct_Sequence()
		{
			List<IToken> tokens = Tokenize(" OR( @{TEMP_1} , @{TRUE} ) ");
			Assert.Equal(6, tokens.Count);
			_ = Assert.IsType<KeywordToken>(tokens[0]);
			_ = Assert.IsType<LParenToken>(tokens[1]);
			_ = Assert.IsType<IdentifierToken>(tokens[2]);
			_ = Assert.IsType<CommaToken>(tokens[3]);
			_ = Assert.IsType<IdentifierToken>(tokens[4]);
			_ = Assert.IsType<RParenToken>(tokens[5]);
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
			List<IToken> tokens = Tokenize("  @{ABC}");
			IdentifierToken token = Assert.IsType<IdentifierToken>(tokens[0]);
			Assert.Equal(2, token.Position); // '@' is at index 2
		}

		// -------------------------------------------------------------------------
		// Custom Cases
		// -------------------------------------------------------------------------

		[Fact]
		public void Run_Finds_Keyword_With_Number_And_Number_Correctly()
		{
			List<IToken> tokens = Tokenize("ADD(125,123.0)");
			_ = Assert.IsType<KeywordToken>(tokens[0]);
			_ = Assert.IsType<LParenToken>(tokens[1]);
			_ = Assert.IsType<NumericToken>(tokens[2]);
			_ = Assert.IsType<CommaToken>(tokens[3]);
			_ = Assert.IsType<NumericToken>(tokens[4]);
			_ = Assert.IsType<RParenToken>(tokens[5]);
			foreach (IToken item in tokens)
			{
				this.outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Identifier_And_Number_Correctly()
		{
			List<IToken> tokens = Tokenize("ADD(@{Identifier_BE1_123},123.0)");
			_ = Assert.IsType<KeywordToken>(tokens[0]);
			_ = Assert.IsType<LParenToken>(tokens[1]);
			_ = Assert.IsType<IdentifierToken>(tokens[2]);
			_ = Assert.IsType<CommaToken>(tokens[3]);
			_ = Assert.IsType<NumericToken>(tokens[4]);
			_ = Assert.IsType<RParenToken>(tokens[5]);
			foreach (IToken item in tokens)
			{
				this.outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Identifier_And_Identifier_Correctly()
		{
			List<IToken> tokens = Tokenize("ADD(@{Identifier_BE1_123},@{Identifier_BE1_123})");
			_ = Assert.IsType<KeywordToken>(tokens[0]);
			_ = Assert.IsType<LParenToken>(tokens[1]);
			_ = Assert.IsType<IdentifierToken>(tokens[2]);
			_ = Assert.IsType<CommaToken>(tokens[3]);
			_ = Assert.IsType<IdentifierToken>(tokens[4]);
			_ = Assert.IsType<RParenToken>(tokens[5]);
			foreach (IToken item in tokens)
			{
				this.outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Keyword_And_Keyword_Correctly()
		{
			List<IToken> tokens = Tokenize("ADD(ADD(1,1),MULT(1,1))");
			_ = Assert.IsType<KeywordToken>(tokens[0]);
			_ = Assert.IsType<LParenToken>(tokens[1]);

			_ = Assert.IsType<KeywordToken>(tokens[2]);
			_ = Assert.IsType<LParenToken>(tokens[3]);
			_ = Assert.IsType<NumericToken>(tokens[4]);
			_ = Assert.IsType<CommaToken>(tokens[5]);
			_ = Assert.IsType<NumericToken>(tokens[6]);
			_ = Assert.IsType<RParenToken>(tokens[7]);

			_ = Assert.IsType<CommaToken>(tokens[8]);

			_ = Assert.IsType<KeywordToken>(tokens[9]);
			_ = Assert.IsType<LParenToken>(tokens[10]);
			_ = Assert.IsType<NumericToken>(tokens[11]);
			_ = Assert.IsType<CommaToken>(tokens[12]);
			_ = Assert.IsType<NumericToken>(tokens[13]);
			_ = Assert.IsType<RParenToken>(tokens[14]);

			_ = Assert.IsType<RParenToken>(tokens[15]);
			foreach (IToken item in tokens)
			{
				this.outputHelper.WriteLine(item.ToString());
			}
		}

		[Fact]
		public void Run_Finds_Keyword_With_Keyword_And_Identifier_Correctly()
		{
			List<IToken> tokens = Tokenize("ADD(ADD(1,1),@{BAU_123_BE1})");
			_ = Assert.IsType<KeywordToken>(tokens[0]);
			_ = Assert.IsType<LParenToken>(tokens[1]);

			_ = Assert.IsType<KeywordToken>(tokens[2]);
			_ = Assert.IsType<LParenToken>(tokens[3]);
			_ = Assert.IsType<NumericToken>(tokens[4]);
			_ = Assert.IsType<CommaToken>(tokens[5]);
			_ = Assert.IsType<NumericToken>(tokens[6]);
			_ = Assert.IsType<RParenToken>(tokens[7]);

			_ = Assert.IsType<CommaToken>(tokens[8]);

			_ = Assert.IsType<IdentifierToken>(tokens[9]);
			_ = Assert.IsType<RParenToken>(tokens[10]);
			foreach (IToken item in tokens)
			{
				this.outputHelper.WriteLine(item.ToString());
			}
		}
	}
}