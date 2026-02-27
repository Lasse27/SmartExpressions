namespace SmartExpressions.Core.Tokens.Arithmetic
{
	public readonly struct PlusToken(int position) : IToken
	{
		public TokenType Type => TokenType.Plus;

		public string Lexeme => "+";

		public int Position => position;

		public object Value => "+";
	}
}
