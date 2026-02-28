namespace SmartExpressions.Core.Tokens.Delimiters
{
	public readonly struct CommaToken(int position) : IToken
	{
		public TokenType Type => TokenType.Comma;

		public string Lexeme => ",";

		public int Position => position;

		public object Value => ",";
	}
}
