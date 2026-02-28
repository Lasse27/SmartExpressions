namespace SmartExpressions.Core.Tokens.Comparison
{
	public readonly struct EqualToken(int position) : IToken
	{
		public TokenType Type => TokenType.Equal;

		public string Lexeme => "==";

		public int Position => position;

		public object Value => "==";
	}
}
