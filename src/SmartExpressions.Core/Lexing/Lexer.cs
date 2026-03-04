using System.Collections.Frozen;
using System.Runtime.CompilerServices;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Lexing
{
	public class Lexer
	{
		/// <summary> Dictionary containing all constants and keywords. </summary>
		internal static readonly Dictionary<string, TokenType> Keywords;

		/// <summary> Frozen Dictionary containing all constants and keywords for runtime lookup </summary>
		internal static readonly FrozenDictionary<string, TokenType> KeywordsLookup;
		internal readonly string _input;
		internal readonly List<Token> _tokens;
		internal int _pointer;
		internal int _length;

		static Lexer()
		{
			Keywords = new Dictionary<string, TokenType>(StringComparer.OrdinalIgnoreCase);

			// Conditional
			Keywords.Add("if", TokenType.IfKeyWord);
			Keywords.Add("else", TokenType.ElseKeyword);

			// Arithmetic
			Keywords.Add("abs", TokenType.AbsKeyword);
			Keywords.Add("add", TokenType.AddKeyWord);
			Keywords.Add("div", TokenType.DivKeyWord);
			Keywords.Add("mod", TokenType.ModKeyWord);
			Keywords.Add("mult", TokenType.MultKeyWord);
			Keywords.Add("neg", TokenType.NegKeyWord);
			Keywords.Add("pow", TokenType.PowerKeyWord);
			Keywords.Add("root", TokenType.RootKeyWord);
			Keywords.Add("sub", TokenType.SubKeyWord);
			Keywords.Add("rand", TokenType.RandKeyWord);

			// Comparison
			Keywords.Add("eq", TokenType.EqualKeyWord);
			Keywords.Add("neq", TokenType.NotEqualKeyWord);
			Keywords.Add("gt", TokenType.GreaterThanKeyWord);
			Keywords.Add("gte", TokenType.GreaterThanEqualKeyWord);
			Keywords.Add("lt", TokenType.LessThanKeyWord);
			Keywords.Add("lte", TokenType.LessThanEqualKeyWord);

			// Logical
			Keywords.Add("and", TokenType.AndKeyWord);
			Keywords.Add("nand", TokenType.NandKeyWord);
			Keywords.Add("nor", TokenType.NorKeyWord);
			Keywords.Add("not", TokenType.NotKeyWord);
			Keywords.Add("or", TokenType.OrKeyWord);
			Keywords.Add("xnor", TokenType.XnorKeyWord);
			Keywords.Add("xor", TokenType.XorKeyWord);

			// Statistical
			Keywords.Add("min", TokenType.MinKeyWord);
			Keywords.Add("max", TokenType.MaxKeyWord);
			Keywords.Add("avg", TokenType.AvgKeyWord);
			Keywords.Add("std", TokenType.StDKeyWord);
			Keywords.Add("sum", TokenType.SumKeyWord);
			Keywords.Add("range", TokenType.RangeKeyWord);
			Keywords.Add("count", TokenType.CountKeyWord);

			// Constants
			Keywords.Add("e", TokenType.EulerKeyword);
			Keywords.Add("pi", TokenType.PiKeyword);
			Keywords.Add("true", TokenType.TrueKeyword);
			Keywords.Add("false", TokenType.FalseKeyword);
			Keywords.Add("null", TokenType.NullKeyword);

			KeywordsLookup = Keywords.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
		}


		public Lexer(string input)
		{
			ArgumentNullException.ThrowIfNull(input, nameof(input));

			this._input = input;
			this._tokens = new List<Token>(input.Length / 3); // assume every third char for less copy cicles
			this._length = input.Length;
			this._pointer = 0;
		}


		#region Helpers

		public void ResetTokenizer()
		{
			this._tokens.Clear();
			this._length = this._input.Length;
			this._pointer = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AdvancePointer() => this._pointer++;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool PointerIsAtEnd() => this._pointer >= this._length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool PointerCanAdvance() => this._pointer + 1 < this._length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public char PeakAtPointer() => this._input[this._pointer];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public char PeakAtNext() => this._input[this._pointer + 1];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddToken(Token token) => this._tokens.Add(token);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValidDigitCharacter(char c)
			=> char.IsDigit(c) || c == Characters.DOT || c == Characters.MINUS;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsValidKeywordCharacter()
		{
			char c = this.PeakAtPointer();
			return char.IsAsciiLetter(c) || c == Characters.UNDERSCORE;
		}

		#endregion


		#region Interface methods

		public Operation<List<Token>> Run()
		{
			// Guard against stupidity
			if (string.IsNullOrWhiteSpace(this._input))
			{
				return Operation<List<Token>>.Success(new List<Token>());
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
					return Operation<List<Token>>.Failure(triState.Message);
				}
			}

			return Operation<List<Token>>.Success(this._tokens);
		}

		#endregion


		private Operation AddTokenByPointer()
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
					return Operation.Success();

				case Characters.RPAREN:
					this.AddToken(new Token(TokenType.RParen, this._pointer, ")"));
					this.AdvancePointer();
					return Operation.Success();

				case Characters.LBRACE:
					this.AddToken(new Token(TokenType.LBrace, this._pointer, "{"));
					this.AdvancePointer();
					return Operation.Success();

				case Characters.RBRACE:
					this.AddToken(new Token(TokenType.RBrace, this._pointer, "}"));
					this.AdvancePointer();
					return Operation.Success();

				/* 
				* Delimiter tokens
				*/
				case Characters.COMMA:
					this.AddToken(new Token(TokenType.Comma, this._pointer, ","));
					this.AdvancePointer();
					return Operation.Success();


				/* 
				* Keyword and identifier tokens
				*/
				case Characters.AT:
					return this.AddIdentifierToken();

				default:
					return this.HandleNonDelimitedToken(c);
			}
		}

		public Operation AddIdentifierToken()
		{
			int entryP = this._pointer;

			this.AdvancePointer(); // Skip @
			if (this.PointerIsAtEnd())
			{
				return Operation.Failure($"Unexpected end of input at index {this._pointer}. Expected: '{Characters.LBRACE}'.");
			}
			else if (this.PeakAtPointer() != Characters.LBRACE)
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

			this.AdvancePointer(); // Skip }
			this.AddToken(new Token(TokenType.Identifier, entryP, identifier));

			// Added
			return Operation.Success();
		}

		private Operation HandleNonDelimitedToken(char c)
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
			return Operation.Failure($"Unexpected character at index {this._pointer}. Actual: '{c}'.");
		}

		public Operation AddKeyWordToken()
		{
			int entryPointer = this._pointer;

			while (!this.PointerIsAtEnd() && this.IsValidKeywordCharacter())
			{
				this.AdvancePointer();
			}

			// No allocate for lookup
			string word = this._input.Substring(entryPointer, this._pointer - entryPointer);
			if (Lexer.KeywordsLookup.TryGetValue(word, out TokenType tokentype))
			{
				// Substr only on found tokentype
				this.AddToken(new Token(tokentype, entryPointer, word));
				return Operation.Success();
			}

			return Operation.Failure($"Unknown keyword starting at index {entryPointer}.");
		}

		public Operation AddNumericToken()
		{
			int entryPointer = this._pointer;
			while (!this.PointerIsAtEnd() && Lexer.IsValidDigitCharacter(this.PeakAtPointer()))
			{
				this.AdvancePointer();
			}

			string number = this._input.Substring(entryPointer, this._pointer - entryPointer);
			this.AddToken(new Token(TokenType.Numeric, entryPointer, number));
			return Operation.Success();
		}
	}
}
