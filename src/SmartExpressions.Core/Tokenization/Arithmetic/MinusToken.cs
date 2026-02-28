namespace SmartExpressions.Core.Tokenization.Arithmetic
{
	public readonly struct MinusToken(int position) : IToken
	{
		public TokenType Type => TokenType.Minus;

		public string Lexeme => "-";

		public int Position => position;

		public object Value => "-";
	}
}
