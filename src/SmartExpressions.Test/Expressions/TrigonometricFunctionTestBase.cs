using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Expressions
{
	/// <summary> Abstrakte Basisklasse für alle unären trigonometrischen Funktionen. </summary>
	public abstract class TrigonometricFunctionTestBase(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{
		protected abstract string FunctionName { get; }

		protected abstract double Compute(double operand);

		protected virtual bool IsValidInput(double operand) => true;

		private string Formula(string operand) => $"{this.FunctionName}({operand})";


		// -----------------------------------------------
		// Determinism
		// -----------------------------------------------

		[Fact]
		public void UnaryFunction_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression(this.Formula("1"));
			_ = expression.Assemble();

			Result<object> r1 = expression.Evaluate();
			Result<object> r2 = expression.Evaluate();

			Assert.Equal(r1.Value, r2.Value);
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void UnaryFunction_Ignores_Whitespace()
		{
			string formula = $"  {this.FunctionName} ( 1 )  ";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(this.Compute(1), value, 10);
		}

		[Fact]
		public void UnaryFunction_Ignores_Linebreaks()
		{
			string formula = $"{this.FunctionName}\n\r( 1\n\r)";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(this.Compute(1), value, 10);
		}

		[Fact]
		public void UnaryFunction_Is_Case_Insensitive()
		{
			string formula = $"{this.FunctionName.ToLowerInvariant()}(1)";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(this.Compute(1), value, 10);
		}


		// -----------------------------------------------
		// Same node type
		// -----------------------------------------------

		[Fact]
		public void UnaryFunction_With_Integer()
		{
			if (!this.IsValidInput(1))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("1"));
			Assert.Equal(this.Compute(1), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Float()
		{
			if (!this.IsValidInput(0.5))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("0.5"));
			Assert.Equal(this.Compute(0.5), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Zero()
		{
			if (!this.IsValidInput(0))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("0"));
			Assert.Equal(this.Compute(0), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Negative()
		{
			if (!this.IsValidInput(-1))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("-1"));
			Assert.Equal(this.Compute(-1), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_PI_Constant()
		{
			if (!this.IsValidInput(Math.PI))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("PI"));
			Assert.Equal(this.Compute(Math.PI), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_E_Constant()
		{
			if (!this.IsValidInput(Math.E))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("E"));
			Assert.Equal(this.Compute(Math.E), value, 10);
		}


		// -----------------------------------------------
		// Identifier node type
		// -----------------------------------------------

		[Fact]
		public void UnaryFunction_With_Integer_Identifier()
		{
			if (!this.IsValidInput(1))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}"),
				new Binding("Key_1", 1));
			Assert.Equal(this.Compute(1), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Float_Identifier()
		{
			if (!this.IsValidInput(0.5))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}"),
				new Binding("Key_1", 0.5d));
			Assert.Equal(this.Compute(0.5), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Negative_Identifier()
		{
			if (!this.IsValidInput(-0.5))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}"),
				new Binding("Key_1", -0.5d));
			Assert.Equal(this.Compute(-0.5), value, 10);
		}


		// -----------------------------------------------
		// Recursive / nested
		// -----------------------------------------------

		[Fact]
		public void UnaryFunction_Nested_Same_Function()
		{
			if (!this.IsValidInput(0.5))
			{
				return;
			}

			double inner = this.Compute(0.5);
			if (!this.IsValidInput(inner))
			{
				return;
			}

			double expected = this.Compute(inner);
			double value = (double)this.EvaluateSuccess(this.Formula(this.Formula("0.5")));
			Assert.Equal(expected, value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Add_As_Argument()
		{
			double input = 0.1 + 0.2;
			if (!this.IsValidInput(input))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("ADD(0.1,0.2)"));
			Assert.Equal(this.Compute(input), value, 10);
		}

		[Fact]
		public void UnaryFunction_With_Sub_As_Argument()
		{
			double input = 0.5 - 0.2;
			if (!this.IsValidInput(input))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("SUB(0.5,0.2)"));
			Assert.Equal(this.Compute(input), value, 10);
		}


		// -----------------------------------------------
		// Robustness
		// -----------------------------------------------

		public static IEnumerable<object[]> Single_Number_Values()
		{
			object[] values =
			[
				0, 1, -1,
				0.5, -0.5, 0.1, -0.1,
				0.9999, -0.9999,
				Math.PI, Math.E, -Math.PI
			];

			foreach (object v in values)
			{
				yield return [v];
			}
		}

		[Theory]
		[MemberData(nameof(Single_Number_Values))]
		public void UnaryFunction_Many_Number_Values(object input)
		{
			double d = Convert.ToDouble(input);
			if (!this.IsValidInput(d))
			{
				return;
			}

			Expression expression = new Expression(this.Formula("@{A}"));
			_ = expression.RegisterBinding("A", input);
			_ = expression.Assemble();

			Result<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);
			_ = Assert.IsType<double>(result.Value);
			Assert.Equal(this.Compute(d), (double)result.Value, 10);
		}

		[Fact]
		public void UnaryFunction_Randomized_Values()
		{
			Random random = new Random(42);

			for (int i = 0; i < 10_000; i++)
			{
				double a = Math.Round((random.NextDouble() * 2D) - 1D, 10);

				if (!this.IsValidInput(a))
				{
					continue;
				}

				double value = (double)this.EvaluateSuccess(
					this.Formula(a.ToString(CultureInfo.InvariantCulture)));

				Assert.Equal(this.Compute(a), value, 10);
			}
		}
	}
}