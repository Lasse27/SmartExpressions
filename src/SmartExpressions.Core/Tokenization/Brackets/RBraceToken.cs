using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Brackets
{
	public readonly struct RBraceToken(int position) : IToken
	{
		public TokenType Type => TokenType.RBrace;

		public string Lexeme => "}";

		public int Position => position;

		public object Value => "}";
	}
}
