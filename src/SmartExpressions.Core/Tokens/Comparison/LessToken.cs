namespace SmartExpressions.Core.Tokens.Comparison
{
	public readonly struct LessToken(int position) : IToken
	{
		public TokenType Type => TokenType.Less;

		public string Lexeme => "<";

		public int Position => position;

		public object Value => "<";
	}
}
