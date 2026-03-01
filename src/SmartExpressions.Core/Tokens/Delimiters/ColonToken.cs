using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Delimiters
{
	public readonly struct ColonToken(int position) : IToken
	{
		public TokenType Type => TokenType.Colon;

		public string Lexeme => ":";

		public int Position => position;

		public object Value => ":";


		public static Operation Add(Lexer lexer)
		{
			lexer.AddToken(new ColonToken(lexer._pointer));
			lexer.AdvancePointer();
			return Operation.Success();
		}
	}
}