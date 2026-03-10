namespace SmartExpressions.Core.Lexing
{
	public enum TokenType : int
	{
		Numeric,
		Identifier,
		Keyword,

		// Delimiters and brackets
		Colon,
		Dot,
		Comma,
		LParen,
		RParen,
		LBrace,
		RBrace,
	}
}
