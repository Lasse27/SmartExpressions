namespace SmartExpressions.Core.Tokens.Arithmetic
{
	public readonly struct DivideToken(int position) : IToken
	{
		public TokenType Type => TokenType.Divide;

		public string Lexeme => "/";

		public int Position => position;

		public object Value => "/";
	}
}
