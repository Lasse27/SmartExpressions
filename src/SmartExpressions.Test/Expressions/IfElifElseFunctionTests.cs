using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test.Expressions
{
	public class IfElifElseFunctionTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{

		// -----------------------------------------------
		// Determinism
		// -----------------------------------------------

		[Fact]
		public void IfElifElse_Evaluate_Is_Deterministic()
		{
			Expression expression = new Expression("if (true) { 1 } elif (eq(1,1)) { 2 } else { 0 }");
			_ = expression.Assemble();

			Result<object> r1 = expression.Evaluate();
			Result<object> r2 = expression.Evaluate();

			Assert.Equal(r1.Value, r2.Value);
		}


		// -----------------------------------------------
		// Input formats
		// -----------------------------------------------

		[Fact]
		public void IfElifElse_Ignores_Whitespace()
		{
			string formula = "IF (true)            { 1         }   elif (eq(1,1)) { 2 }       else      { 0             }";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(1, value, 10);
		}

		[Fact]
		public void IfElifElse_Ignores_Linebreaks()
		{
			string formula = "IF (true)\n\r{ 1\n\r} elif\n\r(eq(1,1))\t\r\n{ 2 }\nelse\n\r\t\t{ 0 }";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(1, value, 10);
		}

		[Fact]
		public void IfElifElse_Is_Case_Insensitive()
		{
			string formula = "if (true) { 1 } eLiF (eq(1,1)) { 2 } eLsE { 0 }";
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(1, value, 10);
		}

		// -----------------------------------------------
		// Same node type
		// -----------------------------------------------

		[Theory]
		[InlineData("if (true) { 25 } elif (false) { 2 } else { 0 }", 25)]
		[InlineData("if (false) { 1 } elif (true) { 25 } else { 0 }", 25)]
		[InlineData("if (false) { 1 } elif (false) { 2 } else { 25 }", 25)]
		public void IfElifElse_With_Integer(string formula, int output)
		{
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(output, value, 10);
		}

		[Theory]
		[InlineData("if (true) { 25.55 } elif (false) { 2 } else { 0 }", 25.55)]
		[InlineData("if (false) { 1 } elif (true) { 25.55 } else { 0 }", 25.55)]
		[InlineData("if (false) { 1 } elif (false) { 2 } else { 25.55 }", 25.55)]
		public void IfElifElse_With_Float(string formula, double output)
		{
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(output, value, 10);
		}

		[Theory]
		[InlineData("if (true) { 0 } elif (false) { 2 } else { 3}", 0)]
		[InlineData("if (false) { 1 } elif (true) { 0 } else { 3 }", 0)]
		[InlineData("if (false) { 1 } elif (false) { 2 } else { 0 }", 0)]
		public void IfElifElse_With_Zero(string formula, double output)
		{
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(output, value, 10);
		}

		[Theory]
		[InlineData("if (true) { -10 } elif (false) { 2 } else { 3}", -10)]
		[InlineData("if (false) { 1 } elif (true) { -20 } else { 3 }", -20)]
		[InlineData("if (false) { 1 } elif (false) { 2 } else { -30 }", -30)]
		public void IfElifElse_With_Negative(string formula, double output)
		{
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(output, value, 10);
		}

		[Theory]
		[InlineData("if (true) { PI } elif (false) { 2 } else { 3}", Math.PI)]
		[InlineData("if (false) { 1 } elif (true) { PI } else { 3 }", Math.PI)]
		[InlineData("if (false) { 1 } elif (false) { 2 } else { PI }", Math.PI)]
		public void IfElifElse_With_PI_Constant(string formula, double output)
		{
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(output, value, 10);
		}

		[Theory]
		[InlineData("if (true) { E } elif (false) { 2 } else { 3}", Math.E)]
		[InlineData("if (false) { 1 } elif (true) { E } else { 3 }", Math.E)]
		[InlineData("if (false) { 1 } elif (false) { 2 } else { E }", Math.E)]
		public void IfElifElse_With_E_Constant(string formula, double output)
		{
			double value = (double)this.EvaluateSuccess(formula);
			Assert.Equal(output, value, 10);
		}
	}
}
