using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct IdentifierToken(int position, string lexeme, object value) : IToken
	{
		public TokenType Type => TokenType.Identifier;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;


		public static Operation Add(Lexer lexer)
		{
			int entryP = lexer._pointer;

			lexer.AdvancePointer(); // Skip @
			if (lexer.PointerIsAtEnd() || lexer.PeakAtPointer() != Characters.LBRACE)
			{
				return Operation.Failure($"Unexpected character at index {lexer._pointer}. Expected: '{Characters.LBRACE}'. Actual: '{lexer.PeakAtPointer()}'.");
			}

			lexer.AdvancePointer(); // Skip {
			int identifierStart = lexer._pointer;

			while (!lexer.PointerIsAtEnd() && lexer.PeakAtPointer() != Characters.RBRACE)
			{
				lexer.AdvancePointer();
			}

			if (lexer.PointerIsAtEnd())
			{
				return Operation.Failure($"Unclosed identifier starting at index {entryP}.");
			}

			string identifier = lexer._input.Substring(identifierStart, lexer._pointer - identifierStart);
			if (string.IsNullOrWhiteSpace(identifier))
			{
				return Operation.Failure($"Empty identifier at index {entryP}.");
			}

			lexer.AdvancePointer(); // Skip }
			lexer.AddToken(new IdentifierToken(entryP, identifier, identifier));

			// Added
			return Operation.Success();
		}
	}
}
