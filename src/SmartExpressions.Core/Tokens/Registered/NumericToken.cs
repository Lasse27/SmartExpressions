namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct NumericToken(int position, string lexeme, object value) : IToken
	{
		public TokenType Type => TokenType.Numeric;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;
	}
}
