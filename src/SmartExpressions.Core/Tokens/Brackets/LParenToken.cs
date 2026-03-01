using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct LParenToken(int position) : IToken
	{
		public TokenType Type => TokenType.LParen;

		public string Lexeme => "(";

		public int Position => position;

		public object Value => "(";


		public static Operation Add(Lexer lexer)
		{
			lexer.AddToken(new LParenToken(lexer._pointer));
			lexer.AdvancePointer();
			return Operation.Success();
		}
	}
}
