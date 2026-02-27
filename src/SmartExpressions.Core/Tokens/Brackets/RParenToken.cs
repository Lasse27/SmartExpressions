namespace SmartExpressions.Core.Tokens.Brackets
{
	public readonly struct RParenToken(int position) : IToken
	{
		public TokenType Type => TokenType.RParen;

		public string Lexeme => ")";

		public int Position => position;

		public object Value => ")";
	}
}
