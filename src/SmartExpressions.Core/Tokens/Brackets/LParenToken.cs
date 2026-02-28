namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct LParenToken(int position) : IToken
	{
		public TokenType Type => TokenType.LParen;

		public string Lexeme => "(";

		public int Position => position;

		public object Value => "(";
	}
}
