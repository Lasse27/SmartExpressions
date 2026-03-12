using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Expressions
{
	/// <summary> Abstrakte Basisklasse für alle Composite-Funktionen mit variabler Operandenzahl. </summary>
	public abstract class StatisticFunctionTestBase(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{
		/// <summary>Name der Funktion, z.B. "SUM", "AVG", "MIN", "MAX".</summary>
		protected abstract string FunctionName { get; }

		/// <summary>Erwartetes Ergebnis für eine gegebene Liste von Operanden.</summary>
		protected abstract double Compute(List<object> operands);

		/// <summary>
		/// Optionale Einschränkung der Eingaben für bestimmte Funktionen.
		/// Gibt false zurück wenn die Kombination übersprungen werden soll.
		/// </summary>
		protected virtual bool IsValidInput(List<object> operands) => true;

		private string Formula(List<object> operands)
		{
			IEnumerable<string?> decsStrings = operands.Select(o => o is double d ? d.ToString(CultureInfo.InvariantCulture) : o.ToString());
			return $"{this.FunctionName}({string.Join(',', decsStrings)})";
		}

		private string Formula(params object[] operands) => this.Formula(operands.ToList());

		private new object EvaluateSuccess(string formula, params Binding[] bindings)
		{
			Expression expression = new Expression(formula);
			foreach (Binding binding in bindings)
			{
				_ = expression.RegisterBinding(binding.Key, binding.Value);
			}
			_ = expression.Assemble();

			Progress<string> progress = new Progress<string>();
			progress.ProgressChanged += (s, e) => this._outputHelper.WriteLine(e);
			EvaluationResult result = expression.Evaluate(progress);
			if (result.IsFail())
			{
				this._outputHelper.WriteLine("Input: " + formula);
				this._outputHelper.WriteLine("Fail: " + result.GetMessage());
			}

			return result.GetValue();
		}


		// -----------------------------------------------
		// Determinism
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression(this.Formula(1, 2, 3));
			_ = expression.Assemble();

			EvaluationResult r1 = expression.Evaluate();
			EvaluationResult r2 = expression.Evaluate();

			Assert.Equal(r1.GetValue(), r2.GetValue());
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_Ignores_Whitespace()
		{
			string formula = $"  {this.FunctionName} ( 2 , 3,  4 )  ";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute([2, 3, 4]), value);
		}

		[Fact]
		public void CompositeFunction_Ignores_Linebreaks()
		{
			string formula = $"{this.FunctionName}\n\r( 2\t,\n3\n\r, 4 )";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute([2, 3, 4]), value);
		}

		[Fact]
		public void CompositeFunction_Is_Case_Insensitive()
		{
			string formula = $"{this.FunctionName.ToLowerInvariant()}(2,3,4)";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute([2, 3, 4]), value);
		}


		// -----------------------------------------------
		// Operandenzahl
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_Single_Operand()
		{
			if (!this.IsValidInput([5]))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(5)));
			Assert.Equal(this.Compute([5]), value);
		}

		[Fact]
		public void CompositeFunction_Two_Operands()
		{
			if (!this.IsValidInput([4, 6]))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(4, 6)));
			Assert.Equal(this.Compute([4, 6]), value);
		}

		[Fact]
		public void CompositeFunction_Three_Operands()
		{
			if (!this.IsValidInput([1, 2, 3]))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(1, 2, 3)));
			Assert.Equal(this.Compute([1, 2, 3]), value);
		}

		[Fact]
		public void CompositeFunction_Five_Operands()
		{
			List<object> operands = [10, 20, 30, 40, 50];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Ten_Operands()
		{
			List<object> operands = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}


		// -----------------------------------------------
		// Typen der Operanden
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_Integer_Operands()
		{
			List<object> operands = [1, 5, 9];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Float_Operands()
		{
			List<object> operands = [1.5, 2.5, 3.5];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Mixed_Integer_And_Float()
		{
			List<object> operands = [1, 2.5, 3];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Constants()
		{
			List<object> operands = ["PI", "E", "PI"];
			List<object> numericOperands = [Math.PI, Math.E, Math.PI];
			if (!this.IsValidInput(numericOperands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(numericOperands), value, 10);
		}

		[Fact]
		public void CompositeFunction_Mixed_Constants_And_Integers()
		{
			List<object> numericOperands = [Math.PI, 2, Math.E];
			if (!this.IsValidInput(numericOperands))
			{
				return;
			}

			string formula = this.Formula("PI", "2", "E");
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute(numericOperands), value, 10);
		}


		// -----------------------------------------------
		// Spezielle Werte
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_With_Zero()
		{
			List<object> operands = [0, 5, 10];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_All_Zeros()
		{
			List<object> operands = [0, 0, 0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Negative_Values()
		{
			List<object> operands = [-3, -1, -5];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Mixed_Positive_And_Negative()
		{
			List<object> operands = [-4, 2, -1, 6];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Large_Values()
		{
			List<object> operands = [1000000, 999999999, 500000];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Very_Small_Values()
		{
			List<object> operands = [0.0001, 0.0002, 0.0003];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			string formula = $"{this.FunctionName}(0.0001,0.0002,0.0003)";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute(operands), value, 10);
		}

		[Fact]
		public void CompositeFunction_All_Same_Values()
		{
			List<object> operands = [7, 7, 7, 7];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(this.Formula(operands)));
			Assert.Equal(this.Compute(operands), value);
		}


		// -----------------------------------------------
		// Identifier-Operanden
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_Single_Identifier()
		{
			if (!this.IsValidInput([6]))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(
				$"{this.FunctionName}(@{{Key_1}})",
				new Binding("Key_1", 6)));
			Assert.Equal(this.Compute([6.0]), value);
		}

		[Fact]
		public void CompositeFunction_All_Identifiers()
		{
			List<object> operands = [6.0, 3.0, 9.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(
				$"{this.FunctionName}(@{{Key_1}},@{{Key_2}},@{{Key_3}})",
				new Binding("Key_1", 6),
				new Binding("Key_2", 3),
				new Binding("Key_3", 9)));
			Assert.Equal(this.Compute(operands), value, 10);
		}

		[Fact]
		public void CompositeFunction_Mixed_Identifier_And_Literal()
		{
			List<object> operands = [10.0, 5.0, 8.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(
				$"{this.FunctionName}(@{{Key_1}},5,@{{Key_2}})",
				new Binding("Key_1", 10),
				new Binding("Key_2", 8)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Double_From_Binding()
		{
			List<object> operands = [4.5, 1.5, 2.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(
				$"{this.FunctionName}(@{{Key_1}},1.5,2)",
				new Binding("Key_1", 4.5d)));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Identifier_With_Constant()
		{
			List<object> operands = [5.0, Math.PI];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			double value = Convert.ToDouble(this.EvaluateSuccess(
				$"{this.FunctionName}(@{{Key_1}},PI)",
				new Binding("Key_1", 5)));
			Assert.Equal(this.Compute(operands), value, 10);
		}


		// -----------------------------------------------
		// Verschachtelung
		// -----------------------------------------------

		[Fact]
		public void CompositeFunction_Nested_Same_Function()
		{
			// f(f(a,b,c), d)
			List<object> inner = [1.0, 2.0, 3.0];
			double innerResult = this.Compute(inner);
			List<object> outer = [innerResult, 4.0];

			if (!this.IsValidInput(inner) || !this.IsValidInput(outer))
			{
				return;
			}

			string formula = $"{this.FunctionName}({this.FunctionName}(1,2,3),4)";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute(outer), value, 10);
		}

		[Fact]
		public void CompositeFunction_With_Add_As_Argument()
		{
			List<object> operands = [3.0, 10.0, 5.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			// ADD(1,2) = 3
			string formula = $"{this.FunctionName}(ADD(1,2),10,5)";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Sub_As_Argument()
		{
			List<object> operands = [4.0, 2.0, 6.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			// SUB(6,2) = 4
			string formula = $"{this.FunctionName}(SUB(6,2),2,6)";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Multiple_Nested_Functions()
		{
			// ADD(1,2)=3, ADD(4,1)=5, ADD(2,4)=6
			List<object> operands = [3.0, 5.0, 6.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			string formula = $"{this.FunctionName}(ADD(1,2),ADD(4,1),ADD(2,4))";
			double value = Convert.ToDouble(this.EvaluateSuccess(formula));
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_With_Identifier_And_Nested_Function()
		{
			List<object> operands = [10.0, 3.0, 7.0];
			if (!this.IsValidInput(operands))
			{
				return;
			}

			// ADD(1,2)=3
			string formula = $"{this.FunctionName}(@{{Key_1}},ADD(1,2),7)";
			double value = Convert.ToDouble(this.EvaluateSuccess(
				formula,
				new Binding("Key_1", 10)));
			Assert.Equal(this.Compute(operands), value, 10);
		}


		// -----------------------------------------------
		// Robustheit
		// -----------------------------------------------

		public static IEnumerable<object[]> Number_Combinations()
		{
			object[] values =
			[
				0, 1, -1, 42, -100,
				0.1m, -0.1m, 1.5, -2.75,
				1000000, 999999999L, 0.0000001m
			];

			// Tripel aus den Werten erzeugen (eingeschränkt auf ausgewählte Paare für Performance)
			foreach (object a in values)
			{
				foreach (object b in values)
				{
					foreach (object c in values)
					{
						yield return [a, b, c];
					}
				}
			}
		}

		[Theory]
		[MemberData(nameof(Number_Combinations))]
		public void CompositeFunction_Many_Number_Combinations(object a, object b, object c)
		{
			double da = Convert.ToDouble(a);
			double db = Convert.ToDouble(b);
			double dc = Convert.ToDouble(c);
			List<object> operands = [da, db, dc];

			if (!this.IsValidInput(operands))
			{
				return;
			}

			Expression expression = new Expression(
				$"{this.FunctionName}(@{{A}},@{{B}},@{{C}})");
			_ = expression.RegisterBinding("A", a);
			_ = expression.RegisterBinding("B", b);
			_ = expression.RegisterBinding("C", c);
			_ = expression.Assemble();

			EvaluationResult result = expression.Evaluate();

			double value = Convert.ToDouble(result.GetValue());
			Assert.Equal(this.Compute(operands), value);
		}

		[Fact]
		public void CompositeFunction_Randomized_Combinations_Three_Operands()
		{
			Random random = new Random(42);

			for (int i = 0; i < 10_000; i++)
			{
				double a = Math.Round((random.NextDouble() * 1000D) - 500D, 10);
				double b = Math.Round((random.NextDouble() * 1000D) - 500D, 10);
				double c = Math.Round((random.NextDouble() * 1000D) - 500D, 10);
				List<object> operands = [a, b, c];

				if (!this.IsValidInput(operands))
				{
					continue;
				}

				string formula = $"{this.FunctionName}(" +
					$"{a.ToString(CultureInfo.InvariantCulture)}," +
					$"{b.ToString(CultureInfo.InvariantCulture)}," +
					$"{c.ToString(CultureInfo.InvariantCulture)})";

				double value = Convert.ToDouble(this.EvaluateSuccess(formula));
				Assert.Equal(this.Compute(operands), value);
			}
		}

		[Fact]
		public void CompositeFunction_Randomized_Combinations_Variable_Length()
		{
			Random random = new Random(99);

			for (int i = 0; i < 1_000; i++)
			{
				int count = random.Next(2, 11); // 2 bis 10 Operanden
				List<object> operands = Enumerable
					.Range(0, count)
					.Select(_ => (object)Math.Round((random.NextDouble() * 200D) - 100D, 10))
					.ToList();

				if (!this.IsValidInput(operands))
				{
					continue;
				}

				string joined = string.Join(',',
					operands.Select(o => ((double)o).ToString(CultureInfo.InvariantCulture)));
				string formula = $"{this.FunctionName}({joined})";

				object value = Convert.ToDouble(this.EvaluateSuccess(formula));
				Assert.Equal(this.Compute(operands), value);
			}
		}
	}
}