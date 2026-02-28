using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens
{
	public enum TokenType : int
	{
		Numeric,
		Identifier,
		Constant,

		// Delimiters and brackets
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

		// Logical keywords
		AndKeyWord,
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
		RangeKeyWord
	}
}
