using System.Collections.Generic;

using BenchmarkDotNet.Attributes;

using Microsoft.VSDiagnostics;

using SmartExpressions.Core.Tokenization;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Benchmark.Tokens
{
	[CPUUsageDiagnoser]
	[MemoryDiagnoser]
	public class TokenizerBenchmark
	{
		// Trivial
		private const string EMPTY = "";
		private const string WHITESPACE = "   \t  \n  ";

		// Single token per category
		private const string SINGLE_NUMBER = "42";
		private const string SINGLE_KEYWORD = "true";
		private const string SINGLE_OP = "+";

		// Short (~5-10 tokens)
		private const string SHORT = "add(3, 5)";

		// Keyword-heavy (~14 tokens, all keyword branches)
		private const string KEYWORD_HEAVY = "if add sub mult div mod true false null pi e if add sub";

		// Numeric-heavy (~10 tokens, digit + decimal branches)
		private const string NUMERIC_HEAVY = "1 2 3 42 3.14 99 1000 0.5 7 8";

		// Mixed / realistic – used as baseline
		private const string MIXED = "if add(3.14, pi) >= true != false";

		// Long (~40+ tokens, all token-type branches hit)
		private const string LONG =
			"if add(1, 2) == true != false && sub(10, 5) <= 6 " +
			"mult(3, pi) >= mod(7, 2) div(9, 3) null e 0.5 1000 " +
			"@{myVar} @{otherVar} add sub mult div mod if true false";

		private Tokenizer _empty = null!;
		private Tokenizer _whitespace = null!;
		private Tokenizer _singleNum = null!;
		private Tokenizer _singleKw = null!;
		private Tokenizer _singleOp = null!;
		private Tokenizer _short = null!;
		private Tokenizer _kwHeavy = null!;
		private Tokenizer _numHeavy = null!;
		private Tokenizer _mixed = null!;
		private Tokenizer _long = null!;

		[GlobalSetup]
		public void Setup()
		{
			_empty = new Tokenizer(EMPTY);
			_whitespace = new Tokenizer(WHITESPACE);
			_singleNum = new Tokenizer(SINGLE_NUMBER);
			_singleKw = new Tokenizer(SINGLE_KEYWORD);
			_singleOp = new Tokenizer(SINGLE_OP);
			_short = new Tokenizer(SHORT);
			_kwHeavy = new Tokenizer(KEYWORD_HEAVY);
			_numHeavy = new Tokenizer(NUMERIC_HEAVY);
			_mixed = new Tokenizer(MIXED);
			_long = new Tokenizer(LONG);
		}

		// ------------------------------------------------------------------
		//  Benchmarks
		// ------------------------------------------------------------------ 

		[Benchmark]
		public Operation<List<IToken>> Run_Empty_Input() => _empty.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_Whitespace_Input() => _whitespace.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_Single_Number() => _singleNum.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_Single_Keyword() => _singleKw.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_Single_Operator() => _singleOp.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_Short_Input() => _short.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_KeywordHeavy_Input() => _kwHeavy.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_NumericHeavy_Input() => _numHeavy.Run();

		[Benchmark(Baseline = true)]
		public Operation<List<IToken>> Run_Mixed_Input() => _mixed.Run();

		[Benchmark]
		public Operation<List<IToken>> Run_Long_Input() => _long.Run();
	}
}