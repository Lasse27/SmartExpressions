using BenchmarkDotNet.Attributes;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Benchmark.Expressions
{
	[MemoryDiagnoser]
	public class AddFunctionBenchmark
	{
		private Expression _simpleExpression = null!;
		private Expression _nestedExpression = null!;
		private Expression _identifierExpression = null!;

		private const string SimpleFormula = "ADD(1,2)";
		private const string NestedFormula = "ADD(1,ADD(1,99))";
		private const string IdentifierFormula = "ADD(@{A},@{B})";

		[GlobalSetup]
		public void Setup()
		{
			// Pre-assembled expressions for Evaluate-only benchmarks
			this._simpleExpression = new Expression(SimpleFormula);
			_ = this._simpleExpression.Assemble();

			this._nestedExpression = new Expression(NestedFormula);
			_ = this._nestedExpression.Assemble();

			this._identifierExpression = new Expression(IdentifierFormula);
			_ = this._identifierExpression.RegisterBinding("A", 100);
			_ = this._identifierExpression.RegisterBinding("B", 200);
			_ = this._identifierExpression.Assemble();
		}

		// ------------------------------------------------
		// Assemble Benchmarks
		// ------------------------------------------------

		[Benchmark]
		public void Assemble_Simple()
		{
			Expression expr = new Expression(SimpleFormula);
			_ = expr.Assemble();
		}

		[Benchmark]
		public void Assemble_Nested()
		{
			Expression expr = new Expression(NestedFormula);
			_ = expr.Assemble();
		}

		[Benchmark]
		public void Assemble_With_Identifiers()
		{
			Expression expr = new Expression(IdentifierFormula);
			_ = expr.RegisterBinding("A", 100);
			_ = expr.RegisterBinding("B", 200);
			_ = expr.Assemble();
		}

		// ------------------------------------------------
		// Evaluate Benchmarks
		// ------------------------------------------------

		[Benchmark]
		public EvaluationResult Evaluate_Simple() 
			=> this._simpleExpression.Evaluate();

		[Benchmark]
		public EvaluationResult Evaluate_Nested() 
			=> this._nestedExpression.Evaluate();

		[Benchmark]
		public EvaluationResult Evaluate_With_Identifiers() 
			=> this._identifierExpression.Evaluate();

		// ------------------------------------------------
		// End-to-End
		// ------------------------------------------------

		[Benchmark]
		public EvaluationResult Assemble_And_Evaluate_Simple()
		{
			Expression expr = new Expression(SimpleFormula);
			_ = expr.Assemble();
			return expr.Evaluate();
		}

		[Benchmark]
		public EvaluationResult Assemble_And_Evaluate_Nested()
		{
			Expression expr = new Expression(NestedFormula);
			_ = expr.Assemble();
			return expr.Evaluate();
		}
	}
}