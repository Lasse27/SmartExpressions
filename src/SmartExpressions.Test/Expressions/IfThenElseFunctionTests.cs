using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Expressions
{
	public partial class IfThenElseFunctionTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
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
			return result.Value;
		}


		[Fact]
		public void If_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression("IF(true) { 1 } ELSE { 2 }");
			_ = expression.Assemble();

			Operation<object> r1 = expression.Evaluate();
			Operation<object> r2 = expression.Evaluate();

			Assert.Equal(r1.Value, r2.Value);
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void If_Ignores_Whitespace()
		{
			object value = this.EvaluateSuccess("  IF ( true ) { 1 } ELSE { 2 }  ");
			Assert.Equal(1m, value);
		}

		[Fact]
		public void If_Ignores_Linebreaks()
		{
			object value = this.EvaluateSuccess("IF(\ntrue\n)\n{\n1\n}\nELSE\n{\n2\n}");
			Assert.Equal(1m, value);
		}

		[Fact]
		public void If_Is_Case_Insensitive()
		{
			object value = this.EvaluateSuccess("if(true) { 1 } else { 2 }");
			Assert.Equal(1m, value);
		}


		// -----------------------------------------------
		// Branch selection – true condition
		// -----------------------------------------------

		[Fact]
		public void If_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(true) { 1 } ELSE { 2 }");
			Assert.Equal(1m, value);
		}

		[Fact]
		public void If_True_Returns_Then_Branch_Integer()
		{
			object value = this.EvaluateSuccess("IF(true) { 100 } ELSE { 200 }");
			Assert.Equal(100m, value);
		}

		[Fact]
		public void If_True_Returns_Then_Branch_Float()
		{
			object value = this.EvaluateSuccess("IF(true) { 1.5 } ELSE { 2.5 }");
			Assert.Equal(1.5m, value);
		}

		[Fact]
		public void If_True_Returns_Then_Branch_Negative()
		{
			object value = this.EvaluateSuccess("IF(true) { -5 } ELSE { 99 }");
			Assert.Equal(-5m, value);
		}

		[Fact]
		public void If_True_Returns_Then_Branch_Zero()
		{
			object value = this.EvaluateSuccess("IF(true) { 0 } ELSE { 99 }");
			Assert.Equal(0m, value);
		}


		// -----------------------------------------------
		// Branch selection – false condition
		// -----------------------------------------------

		[Fact]
		public void If_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(false) { 1 } ELSE { 2 }");
			Assert.Equal(2m, value);
		}

		[Fact]
		public void If_False_Returns_Else_Branch_Integer()
		{
			object value = this.EvaluateSuccess("IF(false) { 100 } ELSE { 200 }");
			Assert.Equal(200m, value);
		}

		[Fact]
		public void If_False_Returns_Else_Branch_Float()
		{
			object value = this.EvaluateSuccess("IF(false) { 1.5 } ELSE { 2.5 }");
			Assert.Equal(2.5m, value);
		}

		[Fact]
		public void If_False_Returns_Else_Branch_Negative()
		{
			object value = this.EvaluateSuccess("IF(false) { 99 } ELSE { -5 }");
			Assert.Equal(-5m, value);
		}

		[Fact]
		public void If_False_Returns_Else_Branch_Zero()
		{
			object value = this.EvaluateSuccess("IF(false) { 99 } ELSE { 0 }");
			Assert.Equal(0m, value);
		}


		// -----------------------------------------------
		// Condition – comparison functions
		// -----------------------------------------------

		[Fact]
		public void If_EQ_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(EQ(1,1)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_EQ_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(EQ(1,2)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_NEQ_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(NEQ(1,2)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_NEQ_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(NEQ(1,1)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_GT_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(GT(5,3)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_GT_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(GT(3,5)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_LT_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(LT(3,5)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_LT_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(LT(5,3)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_GTE_Equal_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(GTE(5,5)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_LTE_Equal_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(LTE(5,5)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}


		// -----------------------------------------------
		// Condition – logical functions
		// -----------------------------------------------

		[Fact]
		public void If_AND_BothTrue_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(AND(true,true)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_AND_OneFalse_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(AND(true,false)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_OR_OneTrue_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(OR(false,true)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_OR_BothFalse_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(OR(false,false)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_NOT_True_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(NOT(true)) { 10 } ELSE { 20 }");
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_NOT_False_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(NOT(false)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}


		// -----------------------------------------------
		// Branch expressions – arithmetic functions
		// -----------------------------------------------

		[Fact]
		public void If_True_Evaluates_Add_In_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(true) { ADD(1,2) } ELSE { 99 }");
			Assert.Equal(3m, value);
		}

		[Fact]
		public void If_False_Evaluates_Add_In_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(false) { 99 } ELSE { ADD(1,2) }");
			Assert.Equal(3m, value);
		}

		public static IEnumerable<object[]> Get_True_Branch_Arithmetic()
		{
			return
			[
				["IF(true) { ABS(-99.9)  } ELSE { 0 }", 99.9m ],
				["IF(true) { ADD(3,4)    } ELSE { 0 }", 7m    ],
				["IF(true) { SUB(10,3)   } ELSE { 0 }", 7m    ],
				["IF(true) { MULT(3,4)   } ELSE { 0 }", 12m   ],
				["IF(true) { DIV(10,2)   } ELSE { 0 }", 5m    ],
				["IF(true) { MOD(10,3)   } ELSE { 0 }", 1m    ],
				["IF(true) { NEG(5)      } ELSE { 0 }", -5m   ],
				["IF(true) { POW(2,8)    } ELSE { 0 }", 256m  ],
				["IF(true) { ROOT(9,2)   } ELSE { 0 }", 3m    ],
			];
		}

		[Theory]
		[MemberData(nameof(Get_True_Branch_Arithmetic))]
		public void If_True_Evaluates_Arithmetic_In_Then_Branch(string formula, decimal expected)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(expected, value);
		}

		public static IEnumerable<object[]> Get_False_Branch_Arithmetic()
		{
			return
			[
				["IF(false) { 0 } ELSE { ABS(-99.9)  }", 99.9m ],
				["IF(false) { 0 } ELSE { ADD(3,4)    }", 7m    ],
				["IF(false) { 0 } ELSE { SUB(10,3)   }", 7m    ],
				["IF(false) { 0 } ELSE { MULT(3,4)   }", 12m   ],
				["IF(false) { 0 } ELSE { DIV(10,2)   }", 5m    ],
				["IF(false) { 0 } ELSE { MOD(10,3)   }", 1m    ],
				["IF(false) { 0 } ELSE { NEG(5)      }", -5m   ],
				["IF(false) { 0 } ELSE { POW(2,8)    }", 256m  ],
				["IF(false) { 0 } ELSE { ROOT(9,2)   }", 3m    ],
			];
		}

		[Theory]
		[MemberData(nameof(Get_False_Branch_Arithmetic))]
		public void If_False_Evaluates_Arithmetic_In_Else_Branch(string formula, decimal expected)
		{
			object value = this.EvaluateSuccess(formula);
			Assert.Equal(expected, value);
		}


		// -----------------------------------------------
		// Identifier node type
		// -----------------------------------------------

		[Fact]
		public void If_Identifier_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(@{Cond}) { 10 } ELSE { 20 }",
				new Binding("Cond", true));
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_Identifier_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(@{Cond}) { 10 } ELSE { 20 }",
				new Binding("Cond", false));
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_EQ_With_Identifier_True_Returns_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(EQ(@{Val},5)) { 10 } ELSE { 20 }",
				new Binding("Val", 5m));
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_EQ_With_Identifier_False_Returns_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(EQ(@{Val},5)) { 10 } ELSE { 20 }",
				new Binding("Val", 3m));
			Assert.Equal(20m, value);
		}

		[Fact]
		public void If_True_Returns_Identifier_From_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(true) { @{Key_1} } ELSE { 99 }",
				new Binding("Key_1", 42m));
			Assert.Equal(42m, value);
		}

		[Fact]
		public void If_False_Returns_Identifier_From_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(false) { 99 } ELSE { @{Key_1} }",
				new Binding("Key_1", 42m));
			Assert.Equal(42m, value);
		}

		[Fact]
		public void If_True_Evaluates_Add_With_Identifier_In_Then_Branch()
		{
			object value = this.EvaluateSuccess("IF(true) { ADD(@{Key_1},10) } ELSE { 99 }",
				new Binding("Key_1", 5m));
			Assert.Equal(15m, value);
		}

		[Fact]
		public void If_False_Evaluates_Add_With_Identifier_In_Else_Branch()
		{
			object value = this.EvaluateSuccess("IF(false) { 99 } ELSE { ADD(@{Key_1},10) }",
				new Binding("Key_1", 5m));
			Assert.Equal(15m, value);
		}

		[Fact]
		public void If_With_All_Identifiers_Condition_True()
		{
			object value = this.EvaluateSuccess(
				"IF(EQ(@{A},@{B})) { @{Then} } ELSE { @{Else} }",
				new Binding("A", 3m),
				new Binding("B", 3m),
				new Binding("Then", 100m),
				new Binding("Else", 200m));
			Assert.Equal(100m, value);
		}

		[Fact]
		public void If_With_All_Identifiers_Condition_False()
		{
			object value = this.EvaluateSuccess(
				"IF(EQ(@{A},@{B})) { @{Then} } ELSE { @{Else} }",
				new Binding("A", 1m),
				new Binding("B", 2m),
				new Binding("Then", 100m),
				new Binding("Else", 200m));
			Assert.Equal(200m, value);
		}


		// -----------------------------------------------
		// Recursive node type
		// -----------------------------------------------

		[Fact]
		public void If_Nested_In_Then_Branch_Inner_True_Evaluates_Correctly()
		{
			// IF(true) { IF(true) { 1 } ELSE { 2 } } ELSE { 3 }  =>  1
			object value = this.EvaluateSuccess(
				"IF(true) { IF(true) { 1 } ELSE { 2 } } ELSE { 3 }");
			Assert.Equal(1m, value);
		}

		[Fact]
		public void If_Nested_In_Then_Branch_Inner_False_Evaluates_Correctly()
		{
			// IF(true) { IF(false) { 1 } ELSE { 2 } } ELSE { 3 }  =>  2
			object value = this.EvaluateSuccess(
				"IF(true) { IF(false) { 1 } ELSE { 2 } } ELSE { 3 }");
			Assert.Equal(2m, value);
		}

		[Fact]
		public void If_Nested_In_Else_Branch_Inner_True_Evaluates_Correctly()
		{
			// IF(false) { 1 } ELSE { IF(true) { 2 } ELSE { 3 } }  =>  2
			object value = this.EvaluateSuccess(
				"IF(false) { 1 } ELSE { IF(true) { 2 } ELSE { 3 } }");
			Assert.Equal(2m, value);
		}

		[Fact]
		public void If_Nested_In_Else_Branch_Inner_False_Evaluates_Correctly()
		{
			// IF(false) { 1 } ELSE { IF(false) { 2 } ELSE { 3 } }  =>  3
			object value = this.EvaluateSuccess(
				"IF(false) { 1 } ELSE { IF(false) { 2 } ELSE { 3 } }");
			Assert.Equal(3m, value);
		}

		[Fact]
		public void If_Nested_Condition_With_Arithmetic_Evaluates_Correctly()
		{
			// IF(EQ(ADD(1,1),2)) { 10 } ELSE { 20 }  =>  10
			object value = this.EvaluateSuccess(
				"IF(EQ(ADD(1,1),2)) { 10 } ELSE { 20 }");
			Assert.Equal(10m, value);
		}

		[Fact]
		public void If_Is_Composable_As_Argument_To_Add()
		{
			// ADD( IF(true){1}ELSE{2} , IF(false){10}ELSE{20} )  =>  1+20 = 21
			object value = this.EvaluateSuccess(
				"ADD(IF(true) { 1 } ELSE { 2 }, IF(false) { 10 } ELSE { 20 })");
			Assert.Equal(21m, value);
		}


		// -----------------------------------------------
		// Short-circuit – only the active branch is evaluated
		// -----------------------------------------------

		[Fact]
		public void If_True_Does_Not_Require_Else_Branch_Binding()
		{
			// ELSE branch references an unbound key – must not affect a true condition
			object value = this.EvaluateSuccess(
				"IF(true) { 1 } ELSE { @{UnboundKey} }");
			Assert.Equal(1m, value);
		}

		[Fact]
		public void If_False_Does_Not_Require_Then_Branch_Binding()
		{
			// THEN branch references an unbound key – must not affect a false condition
			object value = this.EvaluateSuccess(
				"IF(false) { @{UnboundKey} } ELSE { 2 }");
			Assert.Equal(2m, value);
		}


		// -----------------------------------------------
		// Robustness
		// -----------------------------------------------

		public static IEnumerable<object[]> If_Bool_And_Value_Combinations()
		{
			bool[] conditions = [true, false];
			object[] values =
			[
				0m,
				1m,
				-1m,
				42m,
				0.1m,
				-0.1m,
				1.5m,
				1000000m,
				0.0000001m
			];

			foreach (bool cond in conditions)
			{
				foreach (object a in values)
				{
					foreach (object b in values)
					{
						yield return [cond, a, b];
					}
				}
			}
		}

		[Theory]
		[MemberData(nameof(If_Bool_And_Value_Combinations))]
		public void If_Many_Bool_And_Value_Combinations(bool condition, object thenVal, object elseVal)
		{
			Expression expression = new Expression("IF(@{Cond}) { @{Then} } ELSE { @{Else} }");
			_ = expression.Bind("Cond", condition);
			_ = expression.Bind("Then", thenVal);
			_ = expression.Bind("Else", elseVal);
			_ = expression.Assemble();

			Operation<object> result = expression.Evaluate();

			Assert.Equal(Status.Success, result.Status);

			decimal expected = condition
				? Convert.ToDecimal(thenVal)
				: Convert.ToDecimal(elseVal);

			_ = Assert.IsType<decimal>(result.Value);
			Assert.Equal(expected, (decimal)result.Value);
		}

		[Fact]
		public void If_Randomized_Combinations()
		{
			Random random = new Random(42);

			for (int i = 0; i < 10_000; i++)
			{
				bool condition = random.Next(2) == 1;
				decimal thenVal = (decimal)((random.NextDouble() * 1000) - 500);
				decimal elseVal = (decimal)((random.NextDouble() * 1000) - 500);

				string condStr = condition ? "true" : "false";
				object value = this.EvaluateSuccess(
					$"IF({condStr}) " +
					$"{{ {thenVal.ToString(CultureInfo.InvariantCulture)} }} " +
					$"ELSE " +
					$"{{ {elseVal.ToString(CultureInfo.InvariantCulture)} }}");

				decimal expected = condition ? thenVal : elseVal;
				Assert.Equal(expected, (decimal)value);
			}
		}
	}
}