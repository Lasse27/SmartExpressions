namespace SmartExpressions.Core.Tokens
{
	public enum TokenType : int
	{
		Number,

		Identifier,

		Plus,

		Minus,

		Multiply,

		Divide,

		Comma,

		LParen,

		RParen,

		KeywordIf,

		KeywordElse,

		EOF,
		LBrace,
		RBrace,
		Modulo,
		Colon,
		Dot,
		Less,
		LessEqual,
		Greater,
		GreaterEqual,
		NotEqual,
		Equal
	}
}
