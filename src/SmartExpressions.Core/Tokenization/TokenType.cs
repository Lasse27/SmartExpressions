using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokenization
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

		Equal,

		Keyword,
		IfKeyWord,
		ModKeyWord,
		MultKeyWord,
		DivKeyWord,
		SubKeyWord,
		AddKeyWord,
		NullKeyword,
		FalseKeyword,
		TrueKeyword,
		PiKeyword,
		EulerKeyword,
		Numeric,
		Constant,
		ElseKeyword,
		AbsKeyword,
		RootKeyWord,
		NegKeyWord,
		PowerKeyWord,
		SumKeyWord,
		StDKeyWord,
		AvgKeyWord
	}

	public static class TokenTypeExtensions
	{
		public static ArithmeticOperator GetArithmeticOperator(this TokenType tokenType)
		{
			return tokenType switch
			{
				TokenType.MultKeyWord => ArithmeticOperator.Multiply,
				TokenType.DivKeyWord => ArithmeticOperator.Divide,
				TokenType.SubKeyWord => ArithmeticOperator.Subtract,
				TokenType.AddKeyWord => ArithmeticOperator.Add,
				_ => throw new NotSupportedException(),
			};
		}
	}
}
