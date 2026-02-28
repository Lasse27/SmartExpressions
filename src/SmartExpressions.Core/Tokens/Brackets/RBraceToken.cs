namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct RBraceToken(int position) : IToken
	{
		public TokenType Type => TokenType.RBrace;

		public string Lexeme => "}";

		public int Position => position;

		public object Value => "}";
	}
}
