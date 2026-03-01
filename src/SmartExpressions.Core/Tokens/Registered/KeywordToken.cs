using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct KeywordToken(TokenType type, int position, string lexeme, object value) : IToken
	{
		public TokenType Type => type;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;

		public static Operation Add(Lexer lexer)
		{
			int entryPointer = lexer._pointer;

			while (!lexer.PointerIsAtEnd() && lexer.IsValidKeywordCharacter())
			{
				lexer.AdvancePointer();
			}

			// No allocate for lookup
			string word = lexer._input.Substring(entryPointer, lexer._pointer - entryPointer);
			if (Lexer.KeywordsLookup.TryGetValue(word, out TokenType tokentype))
			{
				// Substr only on found tokentype
				lexer.AddToken(new KeywordToken(tokentype, entryPointer, word, word));
				return Operation.Success();
			}

			return Operation.Failure($"Unknown keyword starting at index {entryPointer}.");
		}
	}
}
