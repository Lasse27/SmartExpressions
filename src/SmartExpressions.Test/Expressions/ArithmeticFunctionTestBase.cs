using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Expressions
{
	/// <summary> Abstrakte Basisklasse für alle Zwei-Operand-Funktionen. </summary>
	public abstract class ArithmeticFunctionTestBase(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{
		/// <summary>Name der Funktion, z.B. "ADD", "SUB", "MULT".</summary>
		protected abstract string FunctionName { get; }

		/// <summary>Erwartetes Ergebnis für zwei gegebene Operanden.</summary>
		protected abstract double Compute(object left, object right);

		/// <summary>
		/// Optionale Einschränkung der Eingaben für bestimmte Funktionen
		/// (z.B. DIV/MOD: right != 0 / ROOT: right > 0).
		/// Gibt false zurück wenn die Kombination übersprungen werden soll.
		/// </summary>
		protected virtual bool IsValidInput(object left, object right) => true;

		private string Formula(string left, string right) =>
			$"{this.FunctionName}({left},{right})";



		// -----------------------------------------------
		// Determinism
		// -----------------------------------------------

		[Fact]
		public void BinaryFunction_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression(this.Formula("2", "3"));
			_ = expression.Assemble();

			Result<object> r1 = expression.Evaluate();
			Result<object> r2 = expression.Evaluate();

			Assert.Equal(r1.Value, r2.Value);
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void BinaryFunction_Ignores_Whitespace()
		{
			string formula = $"  {this.FunctionName} ( 2 , 3 )  ";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(this.Compute(2, 3), value);
		}

		[Fact]
		public void BinaryFunction_Ignores_Linebreaks()
		{
			string formula = $"{this.FunctionName}\n\r( 2\t,\n3\n\r)";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(this.Compute(2, 3), value);
		}

		[Fact]
		public void BinaryFunction_Is_Case_Insensitive()
		{
			string formula = $"{this.FunctionName.ToLowerInvariant()}(2,3)";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(this.Compute(2, 3), value);
		}


		// -----------------------------------------------
		// Same node type
		// -----------------------------------------------

		[Fact]
		public void BinaryFunction_Two_Integers()
		{
			double value = (double)this.EvaluateSuccess(this.Formula("6", "3"));
			Assert.Equal(this.Compute(6, 3), value);
		}

		[Fact]
		public void BinaryFunction_Two_Floats()
		{
			double value = (double)this.EvaluateSuccess(this.Formula("1.5", "0.5"));
			Assert.Equal(this.Compute(1.5, 0.5), value);
		}

		[Fact]
		public void BinaryFunction_Two_Constants()
		{
			if (!this.IsValidInput(Math.PI, Math.E))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("PI", "E"));
			double expected = this.Compute(Math.PI, Math.E);
			Assert.Equal(expected, value, 10);
		}

		[Fact]
		public void BinaryFunction_With_Zero_Left()
		{
			if (!this.IsValidInput(0, 3))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("0", "3"));
			Assert.Equal(this.Compute(0, 3), value);
		}

		[Fact]
		public void BinaryFunction_With_Zero_Right()
		{
			if (!this.IsValidInput(3, 0))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("3", "0"));
			Assert.Equal(this.Compute(3, 0), value);
		}

		[Fact]
		public void BinaryFunction_With_Negative_Left()
		{
			if (!this.IsValidInput(-4, 2))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("-4", "2"));
			Assert.Equal(this.Compute(-4, 2), value);
		}

		[Fact]
		public void BinaryFunction_With_Negative_Right()
		{
			if (!this.IsValidInput(4, -2))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("4", "-2"));
			Assert.Equal(this.Compute(4, -2), value);
		}

		[Fact]
		public void BinaryFunction_With_Both_Negative()
		{
			if (!this.IsValidInput(-4, -2))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("-4", "-2"));
			Assert.Equal(this.Compute(-4, -2), value);
		}


		// -----------------------------------------------
		// Different node type
		// -----------------------------------------------

		[Fact]
		public void BinaryFunction_Integer_And_Float()
		{
			double value = (double)this.EvaluateSuccess(this.Formula("4", "1.5"));
			Assert.Equal(this.Compute(4, 1.5), value);
		}

		[Fact]
		public void BinaryFunction_Float_And_Integer()
		{
			double value = (double)this.EvaluateSuccess(this.Formula("4.5", "2"));
			Assert.Equal(this.Compute(4.5, 2), value);
		}

		[Fact]
		public void BinaryFunction_Integer_And_Constant()
		{
			if (!this.IsValidInput(5, Math.PI))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("5", "PI"));
			Assert.Equal(this.Compute(5m, Math.PI), value, 10);
		}

		[Fact]
		public void BinaryFunction_Constant_And_Integer()
		{
			if (!this.IsValidInput(Math.PI, 1))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(this.Formula("PI", "1"));
			Assert.Equal(this.Compute(Math.PI, 1m), value, 10);
		}


		// -----------------------------------------------
		// Identifier node type
		// -----------------------------------------------

		[Fact]
		public void BinaryFunction_Identifier_Left_And_Integer_Right()
		{
			if (!this.IsValidInput(6, 3))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}", "3"),
				new Binding("Key_1", 6));
			Assert.Equal(this.Compute(6, 3), value);
		}

		[Fact]
		public void BinaryFunction_Integer_Left_And_Identifier_Right()
		{
			if (!this.IsValidInput(6, 3))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("6", "@{Key_1}"),
				new Binding("Key_1", 3));
			Assert.Equal(this.Compute(6, 3), value);
		}

		[Fact]
		public void BinaryFunction_Float_And_Identifier()
		{
			if (!this.IsValidInput(5, 1.5))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}", "1.5"),
				new Binding("Key_1", 5));
			Assert.Equal(this.Compute(5, 1.5), value);
		}

		[Fact]
		public void BinaryFunction_Constant_And_Identifier()
		{
			if (!this.IsValidInput(Math.PI, 1))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("PI", "@{Key_1}"),
				new Binding("Key_1", 1));
			Assert.Equal(this.Compute(Math.PI, 1), value, 10);
		}

		[Fact]
		public void BinaryFunction_Double_From_Binding()
		{
			if (!this.IsValidInput(5, 1.5))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}", "1"),
				new Binding("Key_1", 4.5d));
			Assert.Equal(this.Compute(4.5, 1), value);
		}

		[Fact]
		public void BinaryFunction_Two_Identifiers()
		{
			if (!this.IsValidInput(6, 3))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}", "@{Key_2}"),
				new Binding("Key_1", 6),
				new Binding("Key_2", 3));
			Assert.Equal(this.Compute(6, 3), value, 10);
		}


		// -----------------------------------------------
		// Recursive node type
		// -----------------------------------------------

		[Fact]
		public void BinaryFunction_Nested_Same_Function()
		{
			// f(f(a,b),c)
			if (!this.IsValidInput(6, 3) || !this.IsValidInput(this.Compute(6, 3), 2))
			{
				return;
			}

			double inner = this.Compute(6, 3);
			double expected = this.Compute(inner, 2);

			string formula = this.Formula(this.Formula("6", "3"), "2");
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(expected, value, 10);
		}

		[Fact]
		public void BinaryFunction_With_Add_As_Left_Argument()
		{
			if (!this.IsValidInput(3, 2))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("ADD(1,2)", "2"));
			Assert.Equal(this.Compute(3, 2), value);
		}

		[Fact]
		public void BinaryFunction_With_Add_As_Right_Argument()
		{
			if (!this.IsValidInput(6, 3))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("6", "ADD(1,2)"));
			Assert.Equal(this.Compute(6, 3), value);
		}

		[Fact]
		public void BinaryFunction_With_Sub_As_Left_Argument()
		{
			if (!this.IsValidInput(4, 2))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("SUB(6,2)", "2"));
			Assert.Equal(this.Compute(4, 2), value);
		}

		[Fact]
		public void BinaryFunction_With_Identifier_And_Function()
		{
			if (!this.IsValidInput(10, 3))
			{
				return;
			}

			double value = (double)this.EvaluateSuccess(
				this.Formula("@{Key_1}", "ADD(1,2)"),
				new Binding("Key_1", 10));
			Assert.Equal(this.Compute(10, 3), value, 10);
		}


		// -----------------------------------------------
		// Robustness
		// -----------------------------------------------

		public static IEnumerable<object[]> Number_Combinations()
		{
			object[] values =
			[
				0, 1, -1, 42, -100,
				0.1m, -0.1m, 1.5, -2.75,
				1000000, 999999999L, 0.0000001m
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
		[MemberData(nameof(Number_Combinations))]
		public void BinaryFunction_Many_Number_Combinations(object left, object right)
		{
			double l = Convert.ToDouble(left);
			double r = Convert.ToDouble(right);

			if (!this.IsValidInput(l, r))
			{
				return;
			}

			Expression expression = new Expression(this.Formula("@{A}", "@{B}"));
			_ = expression.RegisterBinding("A", left);
			_ = expression.RegisterBinding("B", right);
			_ = expression.Assemble();

			Result<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);
			_ = Assert.IsType<double>(result.Value);
			Assert.Equal(this.Compute(l, r), result.Value);
		}

		[Fact]
		public void BinaryFunction_Randomized_Combinations()
		{
			Random random = new Random(42);

			for (int i = 0; i < 10_000; i++)
			{
				double a = Math.Round((random.NextDouble() * 1000D) - 500D, 10);
				double b = Math.Round((random.NextDouble() * 1000D) - 500D, 10);

				if (!this.IsValidInput(a, b))
				{
					continue;
				}

				object value = (double)this.EvaluateSuccess(
					this.Formula(
						a.ToString(CultureInfo.InvariantCulture),
						b.ToString(CultureInfo.InvariantCulture)));

				Assert.Equal(this.Compute(a, b), value);
			}
		}
	}
}