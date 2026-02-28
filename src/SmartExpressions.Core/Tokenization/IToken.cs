namespace SmartExpressions.Core.Tokenization
{
	public interface IToken
	{
		TokenType Type { get; }

		string Lexeme { get; }

		int Position { get; }

		object Value { get; }
	}
}
