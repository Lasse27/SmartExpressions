using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test
{
	public class ReadMeTests : BaseTestClass
	{
		public ReadMeTests(ITestOutputHelper outputHelper) : base(outputHelper)
		{
		}

		[Fact]
		public void Expression_With_Identifier()
		{
			Expression expression = new Expression("Add(@{Key_1}, 25)");
			_ = expression.Bind("Key_1", 66);
			Operation<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal((decimal)91, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());
		}

		[Fact]
		public void Expression_With_Rebound_Identifier()
		{
			Expression expression = new Expression("Add(@{Key_1}, 25)");
			_ = expression.Bind("Key_1", 66);
			Operation<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal((decimal)91, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());

			_ = expression.Bind("Key_1", 60);
			Operation<object> operation2 = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation2.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal((decimal)85, operation2.Value);

			// Output
			_outputHelper.WriteLine(operation2.Value.ToString());
		}
	}
}
