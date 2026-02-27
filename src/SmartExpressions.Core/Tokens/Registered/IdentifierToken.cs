namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct IdentifierToken(int position, string lexeme, object value) : IToken
	{
		public TokenType Type => TokenType.Identifier;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;
	}
}
