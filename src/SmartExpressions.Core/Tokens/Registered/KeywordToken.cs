namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct KeywordToken(int position, string lexeme, object value) : IToken
	{
		public TokenType Type => TokenType.Keyword;
		
		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;
	}
}
