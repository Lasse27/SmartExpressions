using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Comparison
{
	public readonly struct GreaterEqualToken(int position) : IToken
	{
		public TokenType Type => TokenType.GreaterEqual;

		public string Lexeme => ">=";

		public int Position => position;

		public object Value => ">=";
	}
}
