namespace SmartExpressions.Core.Tokenization.Arithmetic
{
	public readonly struct ModuloToken(int position) : IToken
	{
		public TokenType Type => TokenType.Modulo;

		public string Lexeme => "%";

		public int Position => position;

		public object Value => "%";
	}
}
