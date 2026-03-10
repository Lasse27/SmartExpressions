using BenchmarkDotNet.Attributes;

using Microsoft.VSDiagnostics;

using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Benchmark.Parsing
{
	[CPUUsageDiagnoser]
	[MemoryDiagnoser]
	public class ParserBenchmark
	{
		private const string SINGLE_NUMERIC = "42";
		private const string SINGLE_DECIMAL = "3.14";
		private const string SINGLE_BOOL = "true";
		private const string SINGLE_NULL = "null";
		private const string SINGLE_CONSTANT = "pi";
		private const string SINGLE_IDENTIFIER = "@{myVar}";
		private const string ARITHMETIC_FLAT = "add(1, 2)";
		private const string ARITHMETIC_NESTED = "add(mult(2, 3), div(9, 3))";
		private const string ARITHMETIC_DEEP = "add(mult(pow(2, 8), sub(10, 3)), div(abs(neg(100)), mod(7, 2)))";
		private const string COMPARISON_FLAT = "eq(1, 1)";
		private const string LOGICAL_FLAT = "and(true, false)";
		private const string LOGICAL_NESTED = "and(or(true, false), not(xor(true, false)))";
		private const string IF_SIMPLE = "if(true) { 1 } else { 2 }";
		private const string IF_NESTED = "if(eq(1, 1)) { true } else { false }";
		private const string MIXED = "if(eq(add(1, 2), 3)) { true } else { false }";
		private const string LONG =
			"if(and(gte(add(1, 2), 3), eq(mod(7, 2), 1))) { mult(pow(2, 8), pi) } else { div(abs(neg(100)), sub(10, 3)) }";

		private List<Token> _tokensSingleNumeric = null!;
		private List<Token> _tokensSingleDecimal = null!;
		private List<Token> _tokensSingleBool = null!;
		private List<Token> _tokensSingleNull = null!;
		private List<Token> _tokensSingleConstant = null!;
		private List<Token> _tokensSingleIdentifier = null!;
		private List<Token> _tokensArithmeticFlat = null!;
		private List<Token> _tokensArithmeticNested = null!;
		private List<Token> _tokensArithmeticDeep = null!;
		private List<Token> _tokensComparisonFlat = null!;
		private List<Token> _tokensLogicalFlat = null!;
		private List<Token> _tokensLogicalNested = null!;
		private List<Token> _tokensIfSimple = null!;
		private List<Token> _tokensIfNested = null!;
		private List<Token> _tokensMixed = null!;
		private List<Token> _tokensLong = null!;

		[GlobalSetup]
		public void Setup()
		{
			this._tokensSingleNumeric = Lex(SINGLE_NUMERIC);
			this._tokensSingleDecimal = Lex(SINGLE_DECIMAL);
			this._tokensSingleBool = Lex(SINGLE_BOOL);
			this._tokensSingleNull = Lex(SINGLE_NULL);
			this._tokensSingleConstant = Lex(SINGLE_CONSTANT);
			this._tokensSingleIdentifier = Lex(SINGLE_IDENTIFIER);
			this._tokensArithmeticFlat = Lex(ARITHMETIC_FLAT);
			this._tokensArithmeticNested = Lex(ARITHMETIC_NESTED);
			this._tokensArithmeticDeep = Lex(ARITHMETIC_DEEP);
			this._tokensComparisonFlat = Lex(COMPARISON_FLAT);
			this._tokensLogicalFlat = Lex(LOGICAL_FLAT);
			this._tokensLogicalNested = Lex(LOGICAL_NESTED);
			this._tokensIfSimple = Lex(IF_SIMPLE);
			this._tokensIfNested = Lex(IF_NESTED);
			this._tokensMixed = Lex(MIXED);
			this._tokensLong = Lex(LONG);
		}

		private static List<Token> Lex(string input)
		{
			Result<List<	Token>> result = new Lexer(input).Run();
			return result.Status != Status.Success
				? throw new InvalidOperationException($"Lexer failed for '{input}': {result.Message}")
				: result.Value;
		}

		// ------------------------------------------------------------------ //
		//  Benchmarks
		// ------------------------------------------------------------------ //

		[Benchmark]
		public Result<ExpressionNode> Parse_Single_Numeric()
			=> new Parser(this._tokensSingleNumeric).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Single_Decimal()
			=> new Parser(this._tokensSingleDecimal).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Single_Bool()
			=> new Parser(this._tokensSingleBool).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Single_Null()
			=> new Parser(this._tokensSingleNull).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Single_Constant()
			=> new Parser(this._tokensSingleConstant).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Single_Identifier()
			=> new Parser(this._tokensSingleIdentifier).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Arithmetic_Flat()
			=> new Parser(this._tokensArithmeticFlat).Run();


		[Benchmark]
		public Result<ExpressionNode> Parse_Arithmetic_Nested()
			=> new Parser(this._tokensArithmeticNested).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Arithmetic_Deep()
			=> new Parser(this._tokensArithmeticDeep).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Comparison_Flat()
			=> new Parser(this._tokensComparisonFlat).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Logical_Flat()
			=> new Parser(this._tokensLogicalFlat).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Logical_Nested()
			=> new Parser(this._tokensLogicalNested).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_If_Simple()
			=> new Parser(this._tokensIfSimple).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_If_Nested()
			=> new Parser(this._tokensIfNested).Run();

		[Benchmark(Baseline = true)]
		public Result<ExpressionNode> Parse_Mixed()
			=> new Parser(this._tokensMixed).Run();

		[Benchmark]
		public Result<ExpressionNode> Parse_Long()
			=> new Parser(this._tokensLong).Run();
	}
}