namespace SmartExpressions.Core.Lexing
{
	public readonly struct Token(TokenType type, int pointer, string lexeme)
	{
		public TokenType Type { get; } = type;
		public int Position { get; } = pointer;
		public string Lexeme { get; } = lexeme;
	}
}
