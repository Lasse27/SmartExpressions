namespace SmartExpressions.Core.Tokens
{
	public interface IToken
	{
		TokenType Type { get; }

		string Lexeme { get; }

		int Position { get; }

		object Value { get; }
	}
}
