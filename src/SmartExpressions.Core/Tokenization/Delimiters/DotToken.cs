namespace SmartExpressions.Core.Tokenization.Delimiters
{
	public readonly struct DotToken(int position) : IToken
	{
		public TokenType Type => TokenType.Dot;

		public string Lexeme => ".";

		public int Position => position;

		public object Value => ".";
	}
}