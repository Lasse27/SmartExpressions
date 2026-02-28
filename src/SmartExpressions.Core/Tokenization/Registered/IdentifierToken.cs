using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Registered
{
	public readonly struct IdentifierToken(int position, string lexeme, object value) : IToken
	{
		public TokenType Type => TokenType.Identifier;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;
	}
}
