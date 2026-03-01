using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct RParenToken(int position) : IToken
	{
		public TokenType Type => TokenType.RParen;

		public string Lexeme => ")";

		public int Position => position;

		public object Value => ")";

		public static Operation Add(Lexer lexer)
		{
			lexer.AddToken(new RParenToken(lexer._pointer));
			lexer.AdvancePointer();
			return Operation.Success();
		}
	}
}
