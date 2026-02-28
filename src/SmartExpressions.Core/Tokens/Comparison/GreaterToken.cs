namespace SmartExpressions.Core.Tokens.Comparison
{
	public readonly struct GreaterToken(int position) : IToken
	{
		public TokenType Type => TokenType.Greater;

		public string Lexeme => ">";

		public int Position => position;

		public object Value => ">";
	}
}
