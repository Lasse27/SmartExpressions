using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokens.Registered
{
	public readonly struct NumericToken(int position, string lexeme, object value) : IToken
	{
		public TokenType Type => TokenType.Numeric;

		public string Lexeme => lexeme;

		public int Position => position;

		public object Value => value;

		public static Operation Add(Lexer lexer)
		{
			int entryPointer = lexer._pointer;

			while (!lexer.PointerIsAtEnd() && lexer.IsValidDigitCharacter())
			{
				lexer.AdvancePointer();
			}

			string number = lexer._input.Substring(entryPointer, lexer._pointer - entryPointer);
			lexer.AddToken(new NumericToken(entryPointer, number, number));
			return Operation.Success();
		}
	}
}
