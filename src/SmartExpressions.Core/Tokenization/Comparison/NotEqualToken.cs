using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Comparison
{
	public readonly struct NotEqualToken(int position) : IToken
	{
		public TokenType Type => TokenType.NotEqual;

		public string Lexeme => "!=";

		public int Position => position;

		public object Value => "!=";
	}
}
