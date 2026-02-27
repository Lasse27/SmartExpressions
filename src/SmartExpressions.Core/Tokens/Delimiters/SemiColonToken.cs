namespace SmartExpressions.Core.Tokens.Delimiters
{
	public readonly struct SemiColonToken(int position) : IToken
	{
		public TokenType Type => TokenType.Colon;

		public string Lexeme => ";";

		public int Position => position;

		public object Value => ";";
	}
}