using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Brackets
{
	public readonly struct LBraceToken(int position) : IToken
	{
		public TokenType Type => TokenType.LBrace;

		public string Lexeme => "{";

		public int Position => position;

		public object Value => "{";
	}
}
