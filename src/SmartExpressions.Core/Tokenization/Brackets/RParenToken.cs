using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Brackets
{
	public readonly struct RParenToken(int position) : IToken
	{
		public TokenType Type => TokenType.RParen;

		public string Lexeme => ")";

		public int Position => position;

		public object Value => ")";
	}
}
