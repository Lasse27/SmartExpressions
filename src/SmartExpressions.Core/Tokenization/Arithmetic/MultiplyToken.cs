using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Arithmetic
{
	public readonly struct MultiplyToken(int position) : IToken
	{
		public TokenType Type => TokenType.Multiply;

		public string Lexeme => "*";

		public int Position => position;

		public object Value => "*";
	}
}
