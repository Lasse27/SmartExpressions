using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Expressions
{
	public class AddFunctionTests
	{
		private static object EvaluateSuccess(string formula)
		{
			Expression expression = new Expression(formula);
			_ = expression.Assemble();
			Operation<object> result = expression.Evaluate();
			Assert.Equal(Status.Success, result.Status);
			return result.Value;
		}

		[Fact]
		public void Add_Function_Adds_Two_Integer()
		{
			object value = EvaluateSuccess("ADD(100,50)");
			_ = Assert.IsType<decimal>(value);
			Assert.Equal(150m, value);
		}

		[Fact]
		public void Add_Function_Adds_Two_Floats()
		{
			object value = EvaluateSuccess("ADD(1.5,1.5)");
			_ = Assert.IsType<decimal>(value);
			Assert.Equal(3.0m, value);
		}

		[Fact]
		public void Add_Function_Adds_Integer_And_Float()
		{
			object value = EvaluateSuccess("ADD(1,1.5)");
			_ = Assert.IsType<decimal>(value);
			Assert.Equal(2.5m, value);
		}

		[Fact]
		public void Add_Function_Adds_Integer_And_PI()
		{
			object value = EvaluateSuccess("ADD(1,PI)");
			_ = Assert.IsType<decimal>(value);
			Assert.Equal((decimal)(1 +Math.PI), (decimal)value, 10);
		}
	}
}
