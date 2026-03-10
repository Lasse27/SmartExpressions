using System.Collections.Frozen;
using System.Runtime.CompilerServices;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Lexing
{
	public class Lexer
	{
		internal readonly string _input;
		internal readonly List<Token> _tokens;
		internal int _pointer;
		internal int _length;


		public Lexer(string input)
		{
			ArgumentNullException.ThrowIfNull(input, nameof(input));

			this._input = input;
			this._tokens = new List<Token>(input.Length / 3); // assume every third char for less copy cicles
			this._length = input.Length;
			this._pointer = 0;
		}


		#region Helpers

		private void ResetTokenizer()
		{
			this._tokens.Clear();
			this._length = this._input.Length;
			this._pointer = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AdvancePointer() => this._pointer++;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool PointerIsAtEnd() => this._pointer >= this._length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool PointerCanAdvance() => this._pointer + 1 < this._length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private char PeakAtPointer() => this._input[this._pointer];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private char PeakAtNext() => this._input[this._pointer + 1];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddToken(Token token) => this._tokens.Add(token);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsValidDigitCharacter(char c)
			=> char.IsDigit(c) || c == Characters.DOT || c == Characters.MINUS;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsValidKeywordCharacter()
		{
			char c = this.PeakAtPointer();
			return char.IsAsciiLetter(c) || c == Characters.UNDERSCORE;
		}

		#endregion


		#region Interface methods

		public Result<List<Token>> Run()
		{
			// Guard against stupidity
			if (string.IsNullOrWhiteSpace(this._input))
			{
				return Result<List<Token>>.Success(new List<Token>());
			}

			this.ResetTokenizer();
			while (!this.PointerIsAtEnd())
			{
				// Skip whitespace
				if (char.IsWhiteSpace(this.PeakAtPointer()))
				{
					this.AdvancePointer();
					continue;
				}

				// find and add token -> if not found return failure
				Result triState = this.AddTokenByPointer();
				if (triState.Status != Status.Success)
				{
					return Result<List<Token>>.Failure(triState.Message);
				}
			}

			return Result<List<Token>>.Success(this._tokens);
		}

		#endregion


		private Result AddTokenByPointer()
		{
			char c = this.PeakAtPointer();
			switch (c)
			{
				/* 
				* Bracket tokens
				*/
				case Characters.LPAREN:
					this.AddToken(new Token(TokenType.LParen, this._pointer, "("));
					this.AdvancePointer();
					return Result.Success();

				case Characters.RPAREN:
					this.AddToken(new Token(TokenType.RParen, this._pointer, ")"));
					this.AdvancePointer();
					return Result.Success();

				case Characters.LBRACE:
					this.AddToken(new Token(TokenType.LBrace, this._pointer, "{"));
					this.AdvancePointer();
					return Result.Success();

				case Characters.RBRACE:
					this.AddToken(new Token(TokenType.RBrace, this._pointer, "}"));
					this.AdvancePointer();
					return Result.Success();

				/* 
				* Delimiter tokens
				*/
				case Characters.COMMA:
					this.AddToken(new Token(TokenType.Comma, this._pointer, ","));
					this.AdvancePointer();
					return Result.Success();


				/* 
				* Keyword and identifier tokens
				*/
				case Characters.AT:
					return this.AddIdentifierToken();

				default:
					return this.HandleNonDelimitedToken(c);
			}
		}

		private Result AddIdentifierToken()
		{
			int entryP = this._pointer;

			this.AdvancePointer(); // Skip @
			if (this.PointerIsAtEnd())
			{
				return Result.Failure($"Unexpected end of input at index {this._pointer}. Expected: '{Characters.LBRACE}'.");
			}
			else if (this.PeakAtPointer() != Characters.LBRACE)
			{
				return Result.Failure($"Unexpected character at index {this._pointer}. Expected: '{Characters.LBRACE}'. Actual: '{this.PeakAtPointer()}'.");
			}

			this.AdvancePointer(); // Skip {
			int identifierStart = this._pointer;

			while (!this.PointerIsAtEnd() && this.PeakAtPointer() != Characters.RBRACE)
			{
				this.AdvancePointer();
			}

			if (this.PointerIsAtEnd())
			{
				return Result.Failure($"Unclosed identifier starting at index {entryP}.");
			}

			string identifier = this._input.Substring(identifierStart, this._pointer - identifierStart);
			if (string.IsNullOrWhiteSpace(identifier))
			{
				return Result.Failure($"Empty identifier at index {entryP}.");
			}

			this.AdvancePointer(); // Skip }
			this.AddToken(new Token(TokenType.Identifier, entryP, identifier));

			// Added
			return Result.Success();
		}

		private Result HandleNonDelimitedToken(char c)
		{
			if (IsValidDigitCharacter(c))
			{
				// Parse for numbers first
				return this.AddNumericToken();
			}

			if (char.IsLetter(c))
			{
				// Keywords last
				return this.AddKeyWordToken();
			}

			// If none found, return failure
			return Result.Failure($"Unexpected character at index {this._pointer}. Actual: '{c}'.");
		}

		private Result AddKeyWordToken()
		{
			int entryPointer = this._pointer;

			while (!this.PointerIsAtEnd() && this.IsValidKeywordCharacter())
			{
				this.AdvancePointer();
			}

			string word = this._input.Substring(entryPointer, this._pointer - entryPointer);
			this.AddToken(new Token(TokenType.Keyword, entryPointer, word));
			return Result.Success();
		}

		private Result AddNumericToken()
		{
			int entryPointer = this._pointer;
			while (!this.PointerIsAtEnd() && IsValidDigitCharacter(this.PeakAtPointer()))
			{
				this.AdvancePointer();
			}

			string number = this._input.Substring(entryPointer, this._pointer - entryPointer);
			this.AddToken(new Token(TokenType.Numeric, entryPointer, number));
			return Result.Success();
		}
	}
}
