namespace SmartExpressions.Core.Tokens.Arithmetic
{
	public readonly struct MultiplyToken(int position) : IToken
	{
		public TokenType Type => TokenType.Multiply;

		public string Lexeme => "*";

		public int Position => position;

		public object Value => "*";
	}
}
