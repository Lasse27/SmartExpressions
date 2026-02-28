namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct KeywordToken(TokenType type, int position, string lexeme, object value) : IToken
	{
		public TokenType Type => type;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;
	}
}
