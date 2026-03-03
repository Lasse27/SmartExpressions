using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Expressions
{
	public partial class SubFunctionTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
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
		public void Sub_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression("SUB(5,3)");
			_ = expression.Assemble();

			Operation<object> r1 = expression.Evaluate();
			Operation<object> r2 = expression.Evaluate();

			Assert.Equal(r1.Value, r2.Value);
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void Sub_Ignores_Whitespace()
		{
			object value = this.EvaluateSuccess("  SUB ( 5 , 3 )  ");
			Assert.Equal(2m, value);
		}

		[Fact]
		public void Sub_Ignores_Linebreaks()
		{
			object value = this.EvaluateSuccess("  SUB\n\r( 5\t,\n3\n\r)  ");
			Assert.Equal(2m, value);
		}

		[Fact]
		public void Sub_Subtracts_Negative_Numbers()
		{
			object value = this.EvaluateSuccess("SUB(-5,2)");
			Assert.Equal(-7m, value);
		}

		[Fact]
		public void Sub_Subtracts_With_Zero_Minuend()
		{
			object value = this.EvaluateSuccess("SUB(0,5)");
			Assert.Equal(-5m, value);
		}

		[Fact]
		public void Sub_Subtracts_With_Zero_Subtrahend()
		{
			object value = this.EvaluateSuccess("SUB(5,0)");
			Assert.Equal(5m, value);
		}

		[Fact]
		public void Sub_Subtracts_Equal_Values_Returns_Zero()
		{
			object value = this.EvaluateSuccess("SUB(7,7)");
			Assert.Equal(0m, value);
		}

		[Fact]
		public void Sub_Is_Not_Commutative()
		{
			object left = this.EvaluateSuccess("SUB(10,3)");
			object right = this.EvaluateSuccess("SUB(3,10)");
			Assert.NotEqual(left, right);
		}


		// -----------------------------------------------
		// Same node type
		// -----------------------------------------------

		[Fact]
		public void Sub_Subtracts_Two_Integers()
		{
			object value = this.EvaluateSuccess("SUB(100,50)");
			Assert.Equal(50m, value);
		}

		[Fact]
		public void Sub_Subtracts_Two_Floats()
		{
			object value = this.EvaluateSuccess("SUB(3.5,1.5)");
			Assert.Equal(2.0m, value);
		}

		[Fact]
		public void Sub_Subtracts_Two_Constants()
		{
			object value = this.EvaluateSuccess("SUB(PI,E)");
			Assert.Equal((decimal)(Math.PI - Math.E), (decimal)value, 10);
		}


		// -----------------------------------------------
		// Different node type
		// -----------------------------------------------

		[Fact]
		public void Sub_Subtracts_Integer_And_Float()
		{
			object value = this.EvaluateSuccess("SUB(5,1.5)");
			Assert.Equal(3.5m, value);
		}

		[Fact]
		public void Sub_Subtracts_Float_And_Integer()
		{
			object value = this.EvaluateSuccess("SUB(1.5,1)");
			Assert.Equal(0.5m, value);
		}

		[Fact]
		public void Sub_Subtracts_Integer_And_Constant()
		{
			object value = this.EvaluateSuccess("SUB(5,PI)");
			Assert.Equal((decimal)(5 - Math.PI), (decimal)value, 10);
		}

		[Fact]
		public void Sub_Subtracts_Constant_And_Integer()
		{
			object value = this.EvaluateSuccess("SUB(PI,1)");
			Assert.Equal((decimal)(Math.PI - 1), (decimal)value, 10);
		}

		[Fact]
		public void Sub_Subtracts_Float_And_Constant()
		{
			object value = this.EvaluateSuccess("SUB(5.5,PI)");
			Assert.Equal((decimal)(5.5 - Math.PI), (decimal)value, 10);
		}

		[Fact]
		public void Sub_Subtracts_Constant_And_Float()
		{
			object value = this.EvaluateSuccess("SUB(PI,1.5)");
			Assert.Equal((decimal)(Math.PI - 1.5), (decimal)value, 10);
		}


		// -----------------------------------------------
		// Identifier node type
		// -----------------------------------------------

		[Fact]
		public void Sub_Subtracts_Identifier_And_Integer()
		{
			object value = this.EvaluateSuccess("SUB(@{Key_1},2)",
				new Binding("Key_1", 5));
			Assert.Equal(3m, value);
		}

		[Fact]
		public void Sub_Subtracts_Integer_And_Identifier()
		{
			object value = this.EvaluateSuccess("SUB(10,@{Key_1})",
				new Binding("Key_1", 3));
			Assert.Equal(7m, value);
		}

		[Fact]
		public void Sub_Subtracts_Identifier_And_Float()
		{
			object value = this.EvaluateSuccess("SUB(@{Key_1},1.5)",
				new Binding("Key_1", 5));
			Assert.Equal(3.5m, value);
		}

		[Fact]
		public void Sub_Subtracts_Identifier_And_Constant()
		{
			object value = this.EvaluateSuccess("SUB(@{Key_1},PI)",
				new Binding("Key_1", 5));
			Assert.Equal((decimal)(5 - Math.PI), (decimal)value, 10);
		}

		[Fact]
		public void Sub_Subtracts_Double_From_Binding_And_Integer()
		{
			object value = this.EvaluateSuccess("SUB(@{Key_1},1)",
				new Binding("Key_1", 3.5d));
			Assert.Equal(2.5m, value);
		}

		[Fact]
		public void Sub_Subtracts_Two_Identifiers()
		{
			object value = this.EvaluateSuccess("SUB(@{Key_1},@{Key_2})",
				new Binding("Key_1", 10),
				new Binding("Key_2", 4));
			Assert.Equal(6m, (decimal)value, 10);
		}


		// -----------------------------------------------
		// Recursive node type
		// -----------------------------------------------

		[Fact]
		public void Sub_Is_Left_Associative()
		{
			// (10 - 3) - 2 = 5  !=  10 - (3 - 2) = 9
			object left = this.EvaluateSuccess("SUB(SUB(10,3),2)");
			object right = this.EvaluateSuccess("SUB(10,SUB(3,2))");
			Assert.NotEqual(left, right);
			Assert.Equal(5m, left);
			Assert.Equal(9m, right);
		}

		public static IEnumerable<object[]> Get_Integer_And_Function()
		{
			return
			[
				["SUB(100,ABS(-9.9))",  90.1m ],
				["SUB(100,ADD(1,9))",   90m   ],
				["SUB(100,DIV(10,2))",  95m   ],
				["SUB(100,MOD(7,3))",   99m   ],
				["SUB(100,MULT(4,2))",  92m   ],
				["SUB(100,NEG(5))",    105m   ],
				["SUB(100,POW(2,3))",   92m   ],
				["SUB(100,ROOT(9,2))",  97m   ],
				["SUB(100,SUB(6,1))",   95m   ],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Integer_And_Function))]
		public void Sub_Subtracts_Integer_And_Function(string formula, decimal expected)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(expected, value);
		}

		public static IEnumerable<object[]> Get_Float_And_Function()
		{
			return
			[
				["SUB(10.5,ABS(-0.5))",  10.0m ],
				["SUB(10.5,ADD(1,2))",    7.5m ],
				["SUB(10.5,DIV(3,2))",    9.0m ],
				["SUB(10.5,MOD(7,3))",    9.5m ],
				["SUB(10.5,MULT(2,2))",   6.5m ],
				["SUB(10.5,NEG(2))",      12.5m],
				["SUB(10.5,POW(2,2))",    6.5m ],
				["SUB(10.5,ROOT(9,2))",   7.5m ],
				["SUB(10.5,SUB(3,1))",    8.5m ],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Float_And_Function))]
		public void Sub_Subtracts_Float_And_Function(string formula, decimal expected)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(expected, value);
		}

		public static IEnumerable<object[]> Get_Constant_And_Function()
		{
			return
			[
				["SUB(PI,ABS(-1))",   (decimal)Math.PI - 1m ],
				["SUB(PI,ADD(1,0))",  (decimal)Math.PI - 1m ],
				["SUB(PI,DIV(2,2))",  (decimal)Math.PI - 1m ],
				["SUB(PI,MOD(5,4))",  (decimal)Math.PI - 1m ],
				["SUB(PI,MULT(1,1))", (decimal)Math.PI - 1m ],
				["SUB(PI,NEG(-1))",   (decimal)Math.PI - 1m ],
				["SUB(PI,POW(1,5))",  (decimal)Math.PI - 1m ],
				["SUB(PI,ROOT(1,2))", (decimal)Math.PI - 1m ],
				["SUB(PI,SUB(3,2))",  (decimal)Math.PI - 1m ],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Constant_And_Function))]
		public void Sub_Subtracts_Constant_And_Function(string formula, decimal expected)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(expected, (decimal)value, 10);
		}

		[Fact]
		public void Sub_Subtracts_Identifier_And_Function()
		{
			object value = this.EvaluateSuccess("SUB(@{Key_1},ADD(1,9))",
				new Binding("Key_1", 100));
			Assert.Equal(90m, (decimal)value, 10);
		}


		// -----------------------------------------------
		// Robustness
		// -----------------------------------------------

		public static IEnumerable<object[]> Sub_Number_Combinations()
		{
			object[] values =
			[
				0,
				1,
				-1,
				42,
				-100,
				0.1m,
				-0.1m,
				1.5,
				-2.75,
				1000000,
				999999999L,
				0.0000001m
			];

			foreach (object a in values)
			{
				foreach (object b in values)
				{
					yield return [a, b];
				}
			}
		}

		[Theory]
		[MemberData(nameof(Sub_Number_Combinations))]
		public void Sub_Many_Number_Combinations(object left, object right)
		{
			Expression expression = new Expression("SUB(@{A},@{B})");
			_ = expression.Bind("A", left);
			_ = expression.Bind("B", right);
			_ = expression.Assemble();

			Operation<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);

			decimal expected = Convert.ToDecimal(left) - Convert.ToDecimal(right);

			_ = Assert.IsType<decimal>(result.Value);
			Assert.Equal(expected, (decimal)result.Value);
		}

		[Fact]
		public void Sub_Randomized_Combinations()
		{
			Random random = new Random(42);

			for (int i = 0; i < 100_000; i++)
			{
				decimal a = (decimal)((random.NextDouble() * 1000) - 500);
				decimal b = (decimal)((random.NextDouble() * 1000) - 500);

				object value = this.EvaluateSuccess(
					$"SUB(" +
					$"{a.ToString(CultureInfo.InvariantCulture)}," +
					$"{b.ToString(CultureInfo.InvariantCulture)}" +
					$")");

				Assert.Equal(a - b, (decimal)value);
			}
		}
	}
}