using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct LBraceToken(int position) : IToken
	{
		public TokenType Type => TokenType.LBrace;

		public string Lexeme => "{";

		public int Position => position;

		public object Value => "{";

		public static Operation Add(Lexer lexer)
		{
			lexer.AddToken(new LBraceToken(lexer._pointer));
			lexer.AdvancePointer();
			return Operation.Success();
		}
	}
}
