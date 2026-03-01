using System.Collections.Frozen;
using System.Runtime.CompilerServices;

using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Tokens.Brackets;
using SmartExpressions.Core.Tokens.Delimiters;
using SmartExpressions.Core.Tokens.Registered;
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
		internal readonly List<IToken> _tokens;
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
			this._tokens = new List<IToken>(input.Length / 3); // assume every third char for less copy cicles
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
		public void AddToken(IToken token) => this._tokens.Add(token);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsValidDigitCharacter()
		{
			char c = this.PeakAtPointer();
			return char.IsDigit(c) || c == Characters.DOT;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsValidKeywordCharacter()
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
			}

			return Operation<List<IToken>>.Success(this._tokens);
		}

		#endregion


		private Operation AddTokenByPointer()
		{
			char c = this.PeakAtPointer();
			return c switch
			{
				/* 
				* Bracket tokens
				*/
				Characters.LPAREN => LParenToken.Add(this),
				Characters.RPAREN => RParenToken.Add(this),
				Characters.LBRACE => LBraceToken.Add(this),
				Characters.RBRACE => RBraceToken.Get(this),

				/* 
				* Delimiter tokens
				*/
				Characters.COMMA => CommaToken.Add(this),
				Characters.DOT => DotToken.Add(this),
				Characters.COLON => ColonToken.Add(this),
				Characters.SEMICOLON => SemiColonToken.Add(this),

				/* 
				* Keyword and identifier tokens
				*/
				Characters.AT => IdentifierToken.Add(this),
				_ => this.HandleNonDelimitedToken(c),
			};
		}

		private Operation HandleNonDelimitedToken(char c)
		{
			if (char.IsDigit(c))
			{
				// Parse for numbers first
				return NumericToken.Add(this);
			}

			if (char.IsLetter(c))
			{
				// Keywords last
				return KeywordToken.Add(this);
			}

			// If none found, return failure
			return Operation.Failure($"Unexpected character at index {this._pointer}. Actual: '{c}'.");
		}
	}
}
