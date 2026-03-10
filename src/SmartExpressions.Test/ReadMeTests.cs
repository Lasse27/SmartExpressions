using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test
{
	public class ReadMeTests : BaseTestClass
	{
		public ReadMeTests(ITestOutputHelper outputHelper) : base(outputHelper)
		{
		}

		[Fact]
		public void Simple_Expression()
		{
			Expression expression = new Expression("Add(1, 1)");
			Result<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(2D, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());
		}

		[Fact]
		public void Simple_Nested_Expression()
		{
			Expression expression = new Expression("Add(1, MULT(5,5))");
			Result<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(26D, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());
		}

		[Fact]
		public void Simple_Expression_With_Whitespace()
		{
			Expression expression = new Expression("Add  (1   , MULT    (5,      5))");
			Result<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(26D, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());
		}

		[Fact]
		public void Expression_With_Identifier()
		{
			Expression expression = new Expression("Add(@{Key_1}, 25)");
			_ = expression.RegisterBinding("Key_1", 66);
			Result<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(91D, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());
		}

		[Fact]
		public void Expression_With_Rebound_Identifier()
		{
			Expression expression = new Expression("Add(@{Key_1}, 25)");
			_ = expression.RegisterBinding("Key_1", 66);
			Result<object> operation = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(91D, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());

			_ = expression.RegisterBinding("Key_1", 60);
			Result<object> operation2 = expression.Evaluate();

			// Assert
			Assert.Equal(Status.Success, operation2.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(85D, operation2.Value);

			// Output
			_outputHelper.WriteLine(operation2.Value.ToString());
		}

		[Fact]
		public void Simple_Expression_With_Progress()
		{
			Progress<string> progress = new Progress<string>();
			progress.ProgressChanged += (_, e) => _outputHelper.WriteLine(e);

			Expression expression = new Expression("Add(SUB(2,1),MULT(5,5))");
			Result<object> operation = expression.Evaluate(progress);

			// Assert
			Assert.Equal(Status.Success, operation.Status);
			Assert.NotNull(operation.Value);
			Assert.Equal(26D, operation.Value);

			// Output
			_outputHelper.WriteLine(operation.Value.ToString());

			// Console output
			// SUB(2, 1) = 1
			// ADD(SUB(2, 1), MULT(5, 5)) = 26
			// MULT(5, 5) = 25
			// 26
		}
	}
}
