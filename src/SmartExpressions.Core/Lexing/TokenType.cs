namespace SmartExpressions.Core.Lexing
{
	public enum TokenType : int
	{
		Numeric,
		Identifier,

		// Delimiters and brackets
		Colon,
		Dot,
		Comma,
		LParen,
		RParen,
		LBrace,
		RBrace,

		// Conditional
		IfKeyWord,
		ElseKeyword,

		// Arithmetical
		ModKeyWord,
		MultKeyWord,
		DivKeyWord,
		SubKeyWord,
		AddKeyWord,
		AbsKeyword,
		RootKeyWord,
		NegKeyWord,
		PowerKeyWord,
		RandKeyWord,

		// Logical keywords
		AndKeyWord,
		OrKeyWord,
		NandKeyWord,
		NorKeyWord,
		NotKeyWord,
		XnorKeyWord,
		XorKeyWord,

		// Comparsion
		EqualKeyWord,
		NotEqualKeyWord,
		LessThanKeyWord,
		LessThanEqualKeyWord,
		GreaterThanKeyWord,
		GreaterThanEqualKeyWord,

		// Statistics
		SumKeyWord,
		StDKeyWord,
		AvgKeyWord,
		CountKeyWord,
		MedianKeyWord,
		MinKeyWord,
		MaxKeyWord,
		RangeKeyWord,

		// Constants
		EulerKeyword,
		PiKeyword,
		TrueKeyword,
		FalseKeyword,
		NullKeyword,
	}
}
