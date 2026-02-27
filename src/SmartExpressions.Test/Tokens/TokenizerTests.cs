using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Tokens.Arithmetic;
using SmartExpressions.Core.Tokens.Brackets;
using SmartExpressions.Core.Tokens.Comparison;
using SmartExpressions.Core.Tokens.Delimiters;
using SmartExpressions.Core.Tokens.Registered;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Tokens
{
	public class TokenizerTests
	{
		private static List<IToken> Tokenize(string input)
		{
			Tokenizer tokenizer = new Tokenizer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);
			return result.Value;
		}

		private static string TokenizeFailure(string input)
		{
			Tokenizer tokenizer = new Tokenizer(input);
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
			Tokenizer tokenizer = new Tokenizer("");
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Value);
		}

		[Fact]
		public void Run_WhitespaceOnly_ReturnsEmptyList()
		{
			Tokenizer tokenizer = new Tokenizer("   \t\n");
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Value);
		}

		[Fact]
		public void Constructor_NullInput_ThrowsArgumentNullException() => Assert.Throws<ArgumentNullException>(() => new Tokenizer(null!));

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
		// Arithmetic tokens
		// -------------------------------------------------------------------------

		[Theory]
		[InlineData("+", typeof(PlusToken))]
		[InlineData("-", typeof(MinusToken))]
		[InlineData("*", typeof(MultiplyToken))]
		[InlineData("/", typeof(DivideToken))]
		[InlineData("%", typeof(ModuloToken))]
		public void Run_SingleArithmeticOperator_ReturnsCorrectToken(string input, Type expectedType)
		{
			List<IToken> tokens = Tokenize(input);
			_ = Assert.Single(tokens);
			Assert.IsType(expectedType, tokens[0]);
		}

		// -------------------------------------------------------------------------
		// Comparison tokens
		// -------------------------------------------------------------------------

		[Theory]
		[InlineData("<", typeof(LessToken))]
		[InlineData("<=", typeof(LessEqualToken))]
		[InlineData(">", typeof(GreaterToken))]
		[InlineData(">=", typeof(GreaterEqualToken))]
		[InlineData("==", typeof(EqualToken))]
		[InlineData("!=", typeof(NotEqualToken))]
		public void Run_ComparisonOperator_ReturnsCorrectToken(string input, Type expectedType)
		{
			List<IToken> tokens = Tokenize(input);
			_ = Assert.Single(tokens);
			Assert.IsType(expectedType, tokens[0]);
		}

		[Fact]
		public void Run_SingleEqual_ReturnsFailure()
		{
			string message = TokenizeFailure("=");
			Assert.Contains("Unexpected end of input", message);
		}

		[Fact]
		public void Run_EqualFollowedByNonEqual_ReturnsFailure()
		{
			string message = TokenizeFailure("=+");
			Assert.Contains("Unexpected character", message);
		}

		[Fact]
		public void Run_ExclamationOnly_ReturnsFailure()
		{
			string message = TokenizeFailure("!");
			Assert.Contains("Unexpected end of input", message);
		}

		[Fact]
		public void Run_ExclamationFollowedByNonEqual_ReturnsFailure()
		{
			string message = TokenizeFailure("!+");
			Assert.Contains("Unexpected character", message);
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
		public void Run_IdentifierEqualIdentifier_ReturnsThreeTokens()
		{
			// @{TEMP_1}==@{TEMP_2}  — no spaces
			List<IToken> tokens = Tokenize("@{TEMP_1}==@{TEMP_2}");
			Assert.Equal(3, tokens.Count);
			_ = Assert.IsType<IdentifierToken>(tokens[0]);
			_ = Assert.IsType<EqualToken>(tokens[1]);
			_ = Assert.IsType<IdentifierToken>(tokens[2]);
		}

		[Fact]
		public void Run_IdentifierEqualLiteral_ReturnsThreeTokens()
		{
			List<IToken> tokens = Tokenize("@{TEMP_1}==@{TRUE}");
			Assert.Equal(3, tokens.Count);
			IdentifierToken left = Assert.IsType<IdentifierToken>(tokens[0]);
			IdentifierToken right = Assert.IsType<IdentifierToken>(tokens[2]);
			Assert.Equal("TEMP_1", left.Value);
			Assert.Equal("TRUE", right.Value);
		}

		[Fact]
		public void Run_ComplexExpression_ReturnsCorrectTokenSequence()
		{
			// (@{A} + @{B}) * @{C}
			List<IToken> tokens = Tokenize("(@{A} + @{B}) * @{C}");
			Assert.Equal(7, tokens.Count);
			_ = Assert.IsType<LParenToken>(tokens[0]);
			_ = Assert.IsType<IdentifierToken>(tokens[1]);
			_ = Assert.IsType<PlusToken>(tokens[2]);
			_ = Assert.IsType<IdentifierToken>(tokens[3]);
			_ = Assert.IsType<RParenToken>(tokens[4]);
			_ = Assert.IsType<MultiplyToken>(tokens[5]);
			_ = Assert.IsType<IdentifierToken>(tokens[6]);
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
		public void Run_TokenPosition_IsCorrect()
		{
			List<IToken> tokens = Tokenize("+ -");
			Assert.Equal(0, tokens[0].Position);
			Assert.Equal(2, tokens[1].Position);
		}

		[Fact]
		public void Run_IdentifierPosition_PointsToAtSign()
		{
			List<IToken> tokens = Tokenize("  @{ABC}");
			IdentifierToken token = Assert.IsType<IdentifierToken>(tokens[0]);
			Assert.Equal(2, token.Position); // '@' is at index 2
		}
	}
}