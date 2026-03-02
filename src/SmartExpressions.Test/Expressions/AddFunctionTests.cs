using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test.Expressions
{
	public partial class AddFunctionTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
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
		public void Add_Function_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression("ADD(1,2)");
			_ = expression.Assemble();

			Operation<object> r1 = expression.Evaluate();
			Operation<object> r2 = expression.Evaluate();

			Assert.Equal(r1.Value, r2.Value);
		}

		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void Add_Function_Ignores_Whitespace()
		{
			object value = this.EvaluateSuccess("  ADD ( 1 , 2 ) ");
			Assert.Equal(3m, value);
		}

		[Fact]
		public void Add_Function_Ignores_Linebreaks()
		{
			object value = this.EvaluateSuccess("  ADD\n\r( 1\t,\n2\n\r) ");
			Assert.Equal(3m, value);
		}

		[Fact]
		public void Add_Function_Adds_Negative_Numbers()
		{
			object value = this.EvaluateSuccess("ADD(-5,2)");
			Assert.Equal(-3m, value);
		}

		[Fact]
		public void Add_Function_Adds_With_Zero()
		{
			object value = this.EvaluateSuccess("ADD(0,5)");
			Assert.Equal(5m, value);
		}



		// -----------------------------------------------
		// Same node type
		// -----------------------------------------------

		[Fact]
		public void Add_Function_Adds_Two_Integer()
		{
			object value = this.EvaluateSuccess("ADD(100,50)");
			Assert.Equal(150m, value);
		}

		[Fact]
		public void Add_Function_Adds_Two_Floats()
		{
			object value = this.EvaluateSuccess("ADD(1.5,1.5)");
			Assert.Equal(3.0m, value);
		}

		[Fact]
		public void Add_Function_Adds_Two_Constants()
		{
			object value = this.EvaluateSuccess("ADD(E,PI)");
			Assert.Equal((decimal)(Math.E + Math.PI), (decimal)value, 10);
		}

		// -----------------------------------------------
		// Different node type
		// -----------------------------------------------

		[Fact]
		public void Add_Function_Adds_Integer_And_Float()
		{
			object value = this.EvaluateSuccess("ADD(1,1.5)");
			Assert.Equal(2.5m, value);
		}

		[Fact]
		public void Add_Function_Adds_Integer_And_Constant()
		{
			object value = this.EvaluateSuccess("ADD(1,PI)");
			Assert.Equal((decimal)(1 + Math.PI), (decimal)value, 10);
		}

		[Fact]
		public void Add_Function_Adds_Float_And_Constant()
		{
			object value = this.EvaluateSuccess("ADD(1.5,PI)");
			Assert.Equal((decimal)(1.5 + Math.PI), (decimal)value, 10);
		}

		// -----------------------------------------------
		// Identifier node type
		// -----------------------------------------------

		[Fact]
		public void Add_Function_Adds_Integer_And_Identifier()
		{
			object value = this.EvaluateSuccess("ADD(@{Key_1},2)", new Binding("Key_1", 1));
			Assert.Equal(3m, value);
		}

		[Fact]
		public void Add_Function_Adds_Float_And_Identifier()
		{
			object value = this.EvaluateSuccess("ADD(@{Key_1},1.5)", new Binding("Key_1", 1));
			Assert.Equal(2.5m, value);
		}

		[Fact]
		public void Add_Function_Adds_Constant_And_Identifier()
		{
			object value = this.EvaluateSuccess("ADD(@{Key_1},PI)", new Binding("Key_1", 1));
			Assert.Equal((decimal)(1 + Math.PI), (decimal)value, 10);
		}

		[Fact]
		public void Add_Function_Adds_Int_And_Double_From_Binding()
		{
			object value = this.EvaluateSuccess("ADD(@{Key_1},1)",
				new Binding("Key_1", 1.5d));
			Assert.Equal(2.5m, value);
		}

		[Fact]
		public void Add_Function_Adds_Two_Identifier()
		{
			object value = this.EvaluateSuccess("ADD(@{Key_1},@{Key_2})", new Binding("Key_1", 1), new Binding("Key_2", 2));
			Assert.Equal(3, (decimal)value, 10);
		}

		// -----------------------------------------------
		// Recursive node type
		// -----------------------------------------------

		[Fact]
		public void Add_Function_Is_Associative()
		{
			object left = this.EvaluateSuccess("ADD(ADD(1,2),3)");
			object right = this.EvaluateSuccess("ADD(1,ADD(2,3))");
			Assert.Equal(left, right);
		}

		public static IEnumerable<object[]> Get_Integer_And_Function()
		{
			return
			[
				["ADD(1,Abs(-99.9))",100.9m],
				["ADD(1,ADD(1,99))",101m],
				["ADD(1,DIV(4,2))",3m],
				["ADD(1,MOD(4,2))",1m],
				["ADD(1,MULT(4,2))",9m],
				["ADD(1,NEG(2))",-1m],
				["ADD(1,POW(2,3))",9m],
				["ADD(1,ROOT(4,2))",3m],
				["ADD(1,SUB(4,2))",3m],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Integer_And_Function))]
		public void Add_Function_Adds_Integer_And_Function(string formula, decimal result)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(result, value);
		}

		public static IEnumerable<object[]> Get_Float_And_Function()
		{
			return
			[
				["ADD(2.5,Abs(-99.9))",102.4m],
				["ADD(2.5,ADD(1,99))", 102.5m],
				["ADD(2.5,DIV(4,2))", 4.5m],
				["ADD(2.5,MOD(4,2))", 2.5m],
				["ADD(2.5,MULT(4,2))", 10.5m],
				["ADD(2.5,NEG(2))", 0.5m],
				["ADD(2.5,POW(2,3))", 10.5m],
				["ADD(2.5,ROOT(4,2))", 4.5m],
				["ADD(2.5,SUB(4,2))", 4.5m],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Float_And_Function))]
		public void Add_Function_Adds_Float_And_Function(string formula, decimal result)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(result, value);
		}

		public static IEnumerable<object[]> Get_Constant_And_Function()
		{
			return
			[
				["ADD(PI,Abs(-99.9))",99.9m + (decimal)Math.PI],
				["ADD(PI,ADD(1,99))", 100m + (decimal)Math.PI],
				["ADD(PI,DIV(4,2))", 2m + (decimal)Math.PI],
				["ADD(PI,MOD(4,2))", 0m + (decimal)Math.PI],
				["ADD(PI,MULT(4,2))", 8m + (decimal)Math.PI],
				["ADD(PI,NEG(2))", -2m + (decimal)Math.PI],
				["ADD(PI,POW(2,3))", 8m + (decimal)Math.PI],
				["ADD(PI,ROOT(4,2))", 2m + (decimal)Math.PI],
				["ADD(PI,SUB(4,2))", 2m + (decimal)Math.PI],
			];
		}

		[Theory]
		[MemberData(nameof(Get_Constant_And_Function))]
		public void Add_Function_Adds_Constant_And_Function(string formula, decimal result)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(result, value);
		}

		[Fact]
		public void Add_Function_Adds_Identifier_And_Function()
		{
			object value = this.EvaluateSuccess("ADD(@{Key_1},ADD(1,99))", new Binding("Key_1", 1));
			Assert.Equal(101, (decimal)value, 10);
		}

		// -----------------------------------------------
		// Robustness
		// -----------------------------------------------

		public static IEnumerable<object[]> Add_Number_Combinations()
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
					yield return new object[] { a, b };
				}
			}
		}

		[Theory]
		[MemberData(nameof(Add_Number_Combinations))]
		public void Add_Function_Many_Number_Combinations(object left, object right)
		{
			string formula = "ADD(@{A},@{B})";

			Expression expression = new Expression(formula);
			_ = expression.Bind("A", left);
			_ = expression.Bind("B", right);

			_ = expression.Assemble();
			Operation<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);

			// Erwartungswert strikt über decimal berechnen
			decimal expected = Convert.ToDecimal(left) + Convert.ToDecimal(right);

			_ = Assert.IsType<decimal>(result.Value);
			Assert.Equal(expected, (decimal)result.Value);
		}

		[Fact]
		public void Add_Function_Randomized_Combinations()
		{
			Random random = new Random(42);

			for (int i = 0; i < 100_000; i++)
			{
				decimal a = (decimal)((random.NextDouble() * 1000) - 500);
				decimal b = (decimal)((random.NextDouble() * 1000) - 500);

				object value = this.EvaluateSuccess(
					$"ADD(" +
					$"{a.ToString(CultureInfo.InvariantCulture)}," +
					$"{b.ToString(CultureInfo.InvariantCulture)}" +
					$")");

				Assert.Equal(a + b, (decimal)value);
			}
		}
	}
}
