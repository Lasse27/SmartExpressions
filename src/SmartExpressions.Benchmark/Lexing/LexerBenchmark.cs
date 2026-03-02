using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

using Microsoft.VSDiagnostics;

using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Benchmark.Lexing
{
	[CPUUsageDiagnoser]
	[MemoryDiagnoser]
	public class LexerBenchmark
	{
		// Trivial
		private const string EMPTY = "";
		private const string WHITESPACE = "   \t  \n  ";

		// Single token per category
		private const string SINGLE_NUMBER = "42";
		private const string SINGLE_KEYWORD = "true";

		// Short (~5-10 tokens)
		private const string SHORT = "add(3, 5)";

		// Keyword-heavy (~14 tokens, all keyword branches)
		private const string KEYWORD_HEAVY = "if add sub mult div mod true false null pi e if add sub";

		// Numeric-heavy (~10 tokens, digit + decimal branches)
		private const string NUMERIC_HEAVY = "1 2 3 42 3.14 99 1000 0.5 7 8";

		// Mixed / realistic – used as baseline
		private const string MIXED = "if(and(gte(add(3.14, pi), 5), true))";

		// Long (~40+ tokens, all token-type branches hit)
		private const string LONG =
			"if add(1, 2) true false sub(10, 5) 6 and(@{myVar0},@{otherVar0})" +
			"mult(3, pi) mod(7, 2) div(9, 3) null e 0.5 1000 " +
			"@{myVar} @{otherVar} add sub mult div mod if true false";

		private Lexer _empty = null!;
		private Lexer _whitespace = null!;
		private Lexer _singleNum = null!;
		private Lexer _singleKw = null!;
		private Lexer _short = null!;
		private Lexer _kwHeavy = null!;
		private Lexer _numHeavy = null!;
		private Lexer _mixed = null!;
		private Lexer _long = null!;

		[GlobalSetup]
		public void Setup()
		{
			_empty = new Lexer(EMPTY);
			_whitespace = new Lexer(WHITESPACE);
			_singleNum = new Lexer(SINGLE_NUMBER);
			_singleKw = new Lexer(SINGLE_KEYWORD);
			_short = new Lexer(SHORT);
			_kwHeavy = new Lexer(KEYWORD_HEAVY);
			_numHeavy = new Lexer(NUMERIC_HEAVY);
			_mixed = new Lexer(MIXED);
			_long = new Lexer(LONG);
		}

		// ------------------------------------------------------------------
		//  Benchmarks
		// ------------------------------------------------------------------ 

		[Benchmark]
		public Operation<List<Token>> Run_Empty_Input() => _empty.Run();

		[Benchmark]
		public Operation<List<Token>> Run_Whitespace_Input() => _whitespace.Run();

		[Benchmark]
		public Operation<List<Token>> Run_Single_Number() => _singleNum.Run();

		[Benchmark]
		public Operation<List<Token>> Run_Single_Keyword() => _singleKw.Run();

		[Benchmark]
		public Operation<List<Token>> Run_Short_Input() => _short.Run();

		[Benchmark]
		public Operation<List<Token>> Run_KeywordHeavy_Input() => _kwHeavy.Run();

		[Benchmark]
		public Operation<List<Token>> Run_NumericHeavy_Input() => _numHeavy.Run();

		[Benchmark(Baseline = true)]
		public Operation<List<Token>> Run_Mixed_Input() => _mixed.Run();

		[Benchmark]
		public Operation<List<Token>> Run_Long_Input() => _long.Run();
	}
}