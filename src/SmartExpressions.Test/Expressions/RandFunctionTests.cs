using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Expressions
{
	public partial class RandFunctionTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{
		private object EvaluateSuccess(string formula, params Binding[] bindings)
		{
			Expression expression = new Expression(formula);
			foreach (Binding binding in bindings)
			{
				_ = expression.Bind(binding.Key, binding.Value);
			}
			_ = expression.Assemble();

			Progress<string> progress = new Progress<string>();
			progress.ProgressChanged += (s, e) => this._outputHelper.WriteLine(e);
			Operation<object> result = expression.Evaluate(progress);
			if (result.Status == Status.Failure)
			{
				this._outputHelper.WriteLine("Input: " + formula);
				this._outputHelper.WriteLine("Fail: " + result.Message);
			}

			Assert.Equal(Status.Success, result.Status);
			_ = Assert.IsType<decimal>(result.Value);
			return result.Value;
		}


		[Fact]
		public void Rand_Evaluate_Returns_Decimal()
		{
			Expression expression = new Expression("RAND(0,1)");
			_ = expression.Assemble();

			Operation<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);
			_ = Assert.IsType<decimal>(result.Value);
		}

		[Fact]
		public void Rand_Evaluate_Is_Non_Deterministic()
		{
			// Mit hoher Wahrscheinlichkeit liefern zwei Aufrufe unterschiedliche Werte
			Expression expression = new Expression("RAND(0,1000000)");
			_ = expression.Assemble();

			Operation<object> r1 = expression.Evaluate();
			Operation<object> r2 = expression.Evaluate();

			// Beide müssen erfolgreich sein
			Assert.Equal(Status.Success, r1.Status);
			Assert.Equal(Status.Success, r2.Status);

			// Exakt gleiche Werte bei diesem Bereich wären ein Zeichen für fehlende Zufälligkeit
			Assert.NotEqual(r1.Value, r2.Value);
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void Rand_Ignores_Whitespace()
		{
			decimal value = (decimal)this.EvaluateSuccess("  RAND ( 0 , 10 )  ");
			Assert.InRange(value, 0m, 10m);
		}

		[Fact]
		public void Rand_Ignores_Linebreaks()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(\n0\n,\n10\n)");
			Assert.InRange(value, 0m, 10m);
		}

		[Fact]
		public void Rand_Is_Case_Insensitive()
		{
			decimal value = (decimal)this.EvaluateSuccess("rand(0,10)");
			Assert.InRange(value, 0m, 10m);
		}


		// -----------------------------------------------
		// Range bounds
		// -----------------------------------------------

		[Fact]
		public void Rand_Result_Is_Within_Integer_Range()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,10)");
			Assert.InRange(value, 0m, 10m);
		}

		[Fact]
		public void Rand_Result_Is_Within_Float_Range()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(1.5,9.5)");
			Assert.InRange(value, 1.5m, 9.5m);
		}

		[Fact]
		public void Rand_Result_Is_Within_Negative_Range()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(-10,-1)");
			Assert.InRange(value, -10m, -1m);
		}

		[Fact]
		public void Rand_Result_Is_Within_Range_Crossing_Zero()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(-5,5)");
			Assert.InRange(value, -5m, 5m);
		}

		[Fact]
		public void Rand_Result_Is_Within_Large_Range()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,1000000)");
			Assert.InRange(value, 0m, 1000000m);
		}

		[Fact]
		public void Rand_Result_Is_Within_Small_Range()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,0.0001)");
			Assert.InRange(value, 0m, 0.0001m);
		}

		[Fact]
		public void Rand_With_Equal_Bounds_Returns_That_Value()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(5,5)");
			Assert.Equal(5m, value);
		}

		[Fact]
		public void Rand_With_Zero_Bounds_Returns_Zero()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,0)");
			Assert.Equal(0m, value);
		}


		// -----------------------------------------------
		// Same node type
		// -----------------------------------------------

		[Fact]
		public void Rand_Accepts_Two_Integers()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(1,100)");
			Assert.InRange(value, 1m, 100m);
		}

		[Fact]
		public void Rand_Accepts_Two_Floats()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(1.5,2.5)");
			Assert.InRange(value, 1.5m, 2.5m);
		}

		[Fact]
		public void Rand_Accepts_Two_Constants()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(E,PI)");
			Assert.InRange(value, (decimal)Math.E, (decimal)Math.PI);
		}


		// -----------------------------------------------
		// Different node type
		// -----------------------------------------------

		[Fact]
		public void Rand_Accepts_Integer_And_Float()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(1,9.5)");
			Assert.InRange(value, 1m, 9.5m);
		}

		[Fact]
		public void Rand_Accepts_Integer_And_Constant()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(1,PI)");
			Assert.InRange(value, 1m, (decimal)Math.PI);
		}

		[Fact]
		public void Rand_Accepts_Float_And_Constant()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(1.5,PI)");
			Assert.InRange(value, 1.5m, (decimal)Math.PI);
		}


		// -----------------------------------------------
		// Identifier node type
		// -----------------------------------------------

		[Fact]
		public void Rand_Accepts_Identifier_As_Min()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(@{Min},10)",
				new Binding("Min", 2));
			Assert.InRange(value, 2m, 10m);
		}

		[Fact]
		public void Rand_Accepts_Identifier_As_Max()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,@{Max})",
				new Binding("Max", 8));
			Assert.InRange(value, 0m, 8m);
		}

		[Fact]
		public void Rand_Accepts_Two_Identifiers()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(@{Min},@{Max})",
				new Binding("Min", 3),
				new Binding("Max", 7));
			Assert.InRange(value, 3m, 7m);
		}

		[Fact]
		public void Rand_Accepts_Float_Identifier_As_Min()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(@{Min},5)",
				new Binding("Min", 1.5d));
			Assert.InRange(value, 1.5m, 5m);
		}

		[Fact]
		public void Rand_Accepts_Float_Identifier_As_Max()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,@{Max})",
				new Binding("Max", 9.5d));
			Assert.InRange(value, 0m, 9.5m);
		}


		// -----------------------------------------------
		// Recursive node type
		// -----------------------------------------------

		[Fact]
		public void Rand_Accepts_Add_As_Min()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(ADD(1,1),10)");
			Assert.InRange(value, 2m, 10m);
		}

		[Fact]
		public void Rand_Accepts_Add_As_Max()
		{
			decimal value = (decimal)this.EvaluateSuccess("RAND(0,ADD(8,2))");
			Assert.InRange(value, 0m, 10m);
		}

		public static IEnumerable<object[]> Get_Function_As_Min_And_Max()
		{
			return
			[
				["RAND(ABS(-5),ABS(-10))",  5m,  10m],
				["RAND(ADD(1,1),ADD(8,2))", 2m,  10m],
				["RAND(SUB(5,3),SUB(9,1))", 2m,   8m],
				["RAND(DIV(2,2),DIV(8,2))", 1m,   4m],
				["RAND(MULT(1,2),MULT(2,5))",2m, 10m],
				["RAND(POW(1,2),POW(2,3))", 1m,   8m],
				["RAND(ROOT(4,2),ROOT(9,2))",2m,  3m],
				["RAND(NEG(-5),NEG(-1))",   1m,   5m],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Function_As_Min_And_Max))]
		public void Rand_Accepts_Arithmetic_Functions_As_Bounds(string formula, decimal min, decimal max)
		{
			decimal value = (decimal)this.EvaluateSuccess(formula);
			Assert.InRange(value, min, max);
		}

		[Fact]
		public void Rand_Result_Can_Be_Used_As_Argument_To_Add()
		{
			// ADD(RAND(0,10), 100) must be in [100, 110]
			decimal value = (decimal)this.EvaluateSuccess("ADD(RAND(0,10),100)");
			Assert.InRange(value, 100m, 110m);
		}

		[Fact]
		public void Rand_Result_Can_Be_Used_As_Condition_Bound_In_If()
		{
			// RAND(5,5) == 5 => IF(EQ(RAND(5,5),5)) => always true
			decimal value = (decimal)this.EvaluateSuccess(
				"IF(EQ(RAND(5,5),5)) { 1 } ELSE { 0 }");
			Assert.Equal(1m, value);
		}


		// -----------------------------------------------
		// Robustness
		// -----------------------------------------------

		public static IEnumerable<object[]> Rand_Range_Combinations()
		{
			decimal[] bounds =
			[
				0m, 1m, -1m, 0.5m, -0.5m, 10m, -10m, 100m, 0.0001m
			];

			foreach (decimal a in bounds)
			{
				foreach (decimal b in bounds)
				{
					if (a <= b)
					{
						yield return [a, b];
					}
				}
			}
		}

		[Theory]
		[MemberData(nameof(Rand_Range_Combinations))]
		public void Rand_Many_Range_Combinations(decimal min, decimal max)
		{
			Expression expression = new Expression("RAND(@{Min},@{Max})");
			_ = expression.Bind("Min", min);
			_ = expression.Bind("Max", max);
			_ = expression.Assemble();

			Operation<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);
			_ = Assert.IsType<decimal>(result.Value);
			Assert.InRange((decimal)result.Value, min, max);
		}

		[Fact]
		public void Rand_Randomized_Ranges_Always_Within_Bounds()
		{
			Random random = new Random(42);

			for (int i = 0; i < 100_000; i++)
			{
				decimal a = (decimal)((random.NextDouble() * 1000) - 500);
				decimal b = (decimal)((random.NextDouble() * 1000) - 500);

				decimal min = Math.Min(a, b);
				decimal max = Math.Max(a, b);

				Expression expression = new Expression("RAND(@{Min},@{Max})");
				_ = expression.Bind("Min", min);
				_ = expression.Bind("Max", max);
				_ = expression.Assemble();

				Operation<object> result = expression.Evaluate();

				Assert.Equal(Status.Success, result.Status);
				_ = Assert.IsType<decimal>(result.Value);
				Assert.InRange((decimal)result.Value, min, max);
			}
		}
	}
}