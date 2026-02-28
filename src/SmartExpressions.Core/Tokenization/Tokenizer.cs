using System.Runtime.CompilerServices;

using SmartExpressions.Core.Tokenization.Arithmetic;
using SmartExpressions.Core.Tokenization.Brackets;
using SmartExpressions.Core.Tokenization.Comparison;
using SmartExpressions.Core.Tokenization.Delimiters;
using SmartExpressions.Core.Tokenization.Registered;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Tokenization
{
	public class Tokenizer
	{
		private static readonly Dictionary<string, TokenType> Keywords;
		private static readonly Dictionary<string, TokenType>.AlternateLookup<ReadOnlySpan<char>> KeywordsLookup;
		private readonly string _input;
		private readonly List<IToken> _tokens;
		private int _pointer;
		private int _length;

		static Tokenizer()
		{
			Keywords = new Dictionary<string, TokenType>(StringComparer.OrdinalIgnoreCase);

			// Constants
			Keywords.Add("e", TokenType.EulerKeyword);
			Keywords.Add("pi", TokenType.PiKeyword);
			Keywords.Add("true", TokenType.TrueKeyword);
			Keywords.Add("false", TokenType.FalseKeyword);
			Keywords.Add("null", TokenType.NullKeyword);

			Keywords.Add("if", TokenType.IfKeyWord);
			Keywords.Add("add", TokenType.AddKeyWord);
			Keywords.Add("sub", TokenType.SubKeyWord);
			Keywords.Add("div", TokenType.DivKeyWord);
			Keywords.Add("mult", TokenType.MultKeyWord);
			Keywords.Add("mod", TokenType.ModKeyWord);

			KeywordsLookup = Keywords.GetAlternateLookup<ReadOnlySpan<char>>();
		}


		public Tokenizer(string input)
		{
			ArgumentNullException.ThrowIfNull(input, nameof(input));

			this._input = input;
			this._tokens = new List<IToken>(input.Length / 3); // assume half for less copy cicles
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
		private void RevertPointer() => this._pointer--;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool PointerIsAtEnd() => this._pointer >= this._length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool PointerCanAdvance() => this._pointer + 1 < this._length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private char PeakAtPointer() => this._input[this._pointer];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private char PeakAtNext() => this._input[this._pointer + 1];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddToken(IToken token) => this._tokens.Add(token);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsValidDigitCharacter()
		{
			char c = this.PeakAtPointer();
			return char.IsDigit(c) || c == Characters.DOT;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsValidKeywordCharacter()
		{
			char c = this.PeakAtPointer();
			return char.IsAsciiLetter(c) || c == Characters.UNDERSCORE;
		}

		#endregion


		#region Interface methods

		public Operation<List<IToken>> Run()
		{
			// Guard against stupidity
			if (string.IsNullOrWhiteSpace(this._input))
			{
				return Operation<List<IToken>>.Success(new List<IToken>());
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
				Operation triState = this.AddTokenByPointer();
				if (triState.Status != Status.Success)
				{
					return Operation<List<IToken>>.Failure(triState.Message);
				}

				this.AdvancePointer();
			}

			return Operation<List<IToken>>.Success(this._tokens);
		}

		#endregion


		private Operation AddTokenByPointer()
		{
			char c = this.PeakAtPointer();
			switch (c)
			{
				/* 
				 * Delimiter tokens
				 */

				case Characters.COMMA:
					this.AddToken(new CommaToken(this._pointer));
					return Operation.Success();

				case Characters.DOT:
					this.AddToken(new DotToken(this._pointer));
					return Operation.Success();

				case Characters.COLON:
					this.AddToken(new ColonToken(this._pointer));
					return Operation.Success();

				case Characters.SEMICOLON:
					this.AddToken(new SemiColonToken(this._pointer));
					return Operation.Success();

				/* 
				* Bracket tokens
				*/

				case Characters.LPAREN:
					this.AddToken(new LParenToken(this._pointer));
					return Operation.Success();

				case Characters.RPAREN:
					this.AddToken(new RParenToken(this._pointer));
					return Operation.Success();

				case Characters.LBRACE:
					this.AddToken(new LBraceToken(this._pointer));
					return Operation.Success();

				case Characters.RBRACE:
					this.AddToken(new RBraceToken(this._pointer));
					return Operation.Success();

				/* 
				* Arithmetic tokens
				*/

				case Characters.PLUS:
					this.AddToken(new PlusToken(this._pointer));
					return Operation.Success();

				case Characters.MINUS:
					this.AddToken(new MinusToken(this._pointer));
					return Operation.Success();

				case Characters.MULTIPLY:
					this.AddToken(new MultiplyToken(this._pointer));
					return Operation.Success();

				case Characters.DIVIDE:
					this.AddToken(new DivideToken(this._pointer));
					return Operation.Success();

				case Characters.MODULO:
					this.AddToken(new ModuloToken(this._pointer));
					return Operation.Success();

				/* 
				* Comparison tokens
				*/

				case Characters.LESS:
					if (this.PointerCanAdvance() && this.PeakAtNext() == Characters.EQUAL)
					{
						this.AddToken(new LessEqualToken(this._pointer));
						this.AdvancePointer();
						return Operation.Success();
					}
					this.AddToken(new LessToken(this._pointer));
					return Operation.Success();


				case Characters.GREATER:
					if (this.PointerCanAdvance() && this.PeakAtNext() == Characters.EQUAL)
					{
						this.AddToken(new GreaterEqualToken(this._pointer));
						this.AdvancePointer();
						return Operation.Success();
					}
					this.AddToken(new GreaterToken(this._pointer));
					return Operation.Success();


				case Characters.EXCLAMATION:
					if (this.PointerCanAdvance())
					{
						this.AdvancePointer();
						char advancedTo = this.PeakAtPointer();
						if (advancedTo == Characters.EQUAL)
						{
							this.AddToken(new NotEqualToken(this._pointer));
							return Operation.Success();
						}
						return Operation.Failure($"Unexpected character at index {this._pointer}. Expected: '{Characters.EQUAL}'. Actual: '{advancedTo}'.");
					}
					return Operation.Failure($"Unexpected end of input at index {this._pointer}. Expected: '{Characters.EQUAL}'.");


				case Characters.EQUAL:
					if (this.PointerCanAdvance())
					{
						this.AdvancePointer();
						char advancedTo = this.PeakAtPointer();
						if (advancedTo == Characters.EQUAL)
						{
							this.AddToken(new EqualToken(this._pointer));
							return Operation.Success();
						}
						return Operation.Failure($"Unexpected character at index {this._pointer}. Expected: '{Characters.EQUAL}'. Actual: '{advancedTo}'.");
					}
					return Operation.Failure($"Unexpected end of input at index {this._pointer}. Expected: '{Characters.EQUAL}'.");


				/* 
				* Keyword and identifier tokens
				*/

				case Characters.AT:
					return this.AddIdentifierToken();

				default:
					if (char.IsDigit(c))
					{
						return this.AddNumericToken();
					}

					if (char.IsLetter(c))
					{
						return this.AddKeywordToken();
					}

					return Operation.Failure($"Unexpected character at index {this._pointer}. Actual: '{c}'.");
			}
		}

		private Operation AddKeywordToken()
		{
			int entryPointer = this._pointer;

			while (!this.PointerIsAtEnd() && this.IsValidKeywordCharacter())
			{
				this.AdvancePointer();
			}

			// No allocate for lookup
			ReadOnlySpan<char> span = this._input.AsSpan(entryPointer, this._pointer - entryPointer);
			if (KeywordsLookup.TryGetValue(span, out TokenType tokentype))
			{
				// Substr only on found tokentype
				string word = this._input.Substring(entryPointer, this._pointer - entryPointer);
				this.RevertPointer();
				this.AddToken(new KeywordToken(tokentype, entryPointer, word, word));
				return Operation.Success();
			}

			return Operation.Failure($"Unknown keyword starting at index {entryPointer}.");
		}


		private Operation AddNumericToken()
		{
			int entryPointer = this._pointer;

			while (!this.PointerIsAtEnd() && this.IsValidDigitCharacter())
			{
				this.AdvancePointer();
			}

			string number = this._input.Substring(entryPointer, this._pointer - entryPointer);
			this.RevertPointer();
			this.AddToken(new NumericToken(entryPointer, number, number));
			return Operation.Success();
		}


		private Operation AddIdentifierToken()
		{
			int entryP = this._pointer;

			this.AdvancePointer(); // Skip @
			if (this.PointerIsAtEnd() || this.PeakAtPointer() != Characters.LBRACE)
			{
				return Operation.Failure($"Unexpected character at index {this._pointer}. Expected: '{Characters.LBRACE}'. Actual: '{this.PeakAtPointer()}'.");
			}

			this.AdvancePointer(); // Skip {
			int identifierStart = this._pointer;

			while (!this.PointerIsAtEnd() && this.PeakAtPointer() != Characters.RBRACE)
			{
				this.AdvancePointer();
			}

			if (this.PointerIsAtEnd())
			{
				return Operation.Failure($"Unclosed identifier starting at index {entryP}.");
			}

			string identifier = this._input.Substring(identifierStart, this._pointer - identifierStart);
			if (string.IsNullOrWhiteSpace(identifier))
			{
				return Operation.Failure($"Empty identifier at index {entryP}.");
			}
			this.AddToken(new IdentifierToken(entryP, identifier, identifier));
			return Operation.Success();
		}
	}
}
