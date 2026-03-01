using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Tokens.Brackets;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Lexing
{
	public class BracketsTest
	{
		[Theory]
		[InlineData("(", typeof(LParenToken))]
		[InlineData(")", typeof(RParenToken))]
		[InlineData("{", typeof(LBraceToken))]
		[InlineData("}", typeof(RBraceToken))]
		public void Run_SingleBracket_Returns_Correct_Object_Type(string input, Type expectedType)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);

			List<IToken> tokens = result.Value;
			_ = Assert.Single(tokens);
			Assert.IsType(expectedType, tokens[0]);
		}


		[Theory]
		[InlineData("((((((((((((((", typeof(LParenToken))]
		[InlineData("))))))))))))))", typeof(RParenToken))]
		[InlineData("{{{{{{{{{{{{{{", typeof(LBraceToken))]
		[InlineData("}}}}}}}}}}}}}}", typeof(RBraceToken))]
		public void Run_MultiBracket_Returns_Correct_Object_Type(string input, Type expectedType)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);

			List<IToken> tokens = result.Value;
			foreach (IToken token in tokens)
			{
				Assert.IsType(expectedType, token);
			}
		}

		[Theory]
		[InlineData("(", TokenType.LParen)]
		[InlineData(")", TokenType.RParen)]
		[InlineData("{", TokenType.LBrace)]
		[InlineData("}", TokenType.RBrace)]
		public void Run_SingleBracket_Returns_Correct_Token_Type(string input, TokenType expectedType)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);

			List<IToken> tokens = result.Value;
			_ = Assert.Single(tokens);
			Assert.Equal(expectedType, tokens[0].Type);
		}

		[Theory]
		[InlineData("((((((((((((((", TokenType.LParen)]
		[InlineData("))))))))))))))", TokenType.RParen)]
		[InlineData("{{{{{{{{{{{{{{", TokenType.LBrace)]
		[InlineData("}}}}}}}}}}}}}}", TokenType.RBrace)]
		public void Run_MultiBracket_Returns_Correct_Token_Type(string input, TokenType expectedType)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);

			List<IToken> tokens = result.Value;
			foreach (IToken token in tokens)
			{
				Assert.Equal(expectedType, token.Type);
			}
		}

		[Theory]
		[InlineData("(", 0)]
		[InlineData(" (", 1)]
		[InlineData("  (   ", 2)]
		[InlineData(")", 0)]
		[InlineData(" )", 1)]
		[InlineData("  )   ", 2)]
		[InlineData("{", 0)]
		[InlineData(" {", 1)]
		[InlineData("  {   ", 2)]
		[InlineData("}", 0)]
		[InlineData(" }", 1)]
		[InlineData("  }   ", 2)]
		public void Run_SingleBracket_Returns_Correct_Position(string input, int position)
		{
			Lexer tokenizer = new Lexer(input);
			Operation<List<IToken>> result = tokenizer.Run();
			Assert.Equal(Status.Success, result.Status);

			List<IToken> tokens = result.Value;
			_ = Assert.Single(tokens);
			Assert.Equal(position, tokens[0].Position);
		}


	}
}
