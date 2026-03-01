using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct RBraceToken(int position) : IToken
	{
		public TokenType Type => TokenType.RBrace;

		public string Lexeme => "}";

		public int Position => position;

		public object Value => "}";

		public static Operation Get(Lexer lexer)
		{
			lexer.AddToken(new RBraceToken(lexer._pointer));
			lexer.AdvancePointer();
			return Operation.Success();
		}
	}
}
