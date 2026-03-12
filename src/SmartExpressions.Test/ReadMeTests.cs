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
			EvaluationResult operation = expression.Evaluate();

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(2D, operation.GetValue());

			// Output
			_outputHelper.WriteLine(operation.GetValue().ToString());
		}

		[Fact]
		public void Simple_Nested_Expression()
		{
			Expression expression = new Expression("Add(1, MULT(5,5))");
			EvaluationResult operation = expression.Evaluate();

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(26D, operation.GetValue());

			// Output
			_outputHelper.WriteLine(operation.GetValue().ToString());
		}

		[Fact]
		public void Simple_Expression_With_Whitespace()
		{
			Expression expression = new Expression("Add  (1   , MULT    (5,      5))");
			EvaluationResult operation = expression.Evaluate();

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(26D, operation.GetValue());

			// Output
			_outputHelper.WriteLine(operation.GetValue().ToString());
		}

		[Fact]
		public void Expression_With_Identifier()
		{
			Expression expression = new Expression("Add(@{Key_1}, 25)");
			_ = expression.RegisterBinding("Key_1", 66);
			EvaluationResult operation = expression.Evaluate();

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(91D, operation.GetValue());

			// Output
			_outputHelper.WriteLine(operation.GetValue().ToString());
		}

		[Fact]
		public void Expression_With_Rebound_Identifier()
		{
			Expression expression = new Expression("Add(@{Key_1}, 25)");
			_ = expression.RegisterBinding("Key_1", 66);
			EvaluationResult operation = expression.Evaluate();

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(91D, operation.GetValue());

			// Output
			_outputHelper.WriteLine(operation.GetValue().ToString());

			_ = expression.RegisterBinding("Key_1", 60);
			EvaluationResult operation2 = expression.Evaluate();

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(85D, operation2.GetValue());

			// Output
			_outputHelper.WriteLine(operation2.GetValue().ToString());
		}

		[Fact]
		public void Simple_Expression_With_Progress()
		{
			Progress<string> progress = new Progress<string>();
			progress.ProgressChanged += (_, e) => _outputHelper.WriteLine(e);

			Expression expression = new Expression("Add(SUB(2,1),MULT(5,5))");
			EvaluationResult operation = expression.Evaluate(progress);

			// Assert
			Assert.NotNull(operation.GetValue());
			Assert.Equal(26D, operation.GetValue());

			// Output
			_outputHelper.WriteLine(operation.GetValue().ToString());

			// Console output
			// SUB(2, 1) = 1
			// ADD(SUB(2, 1), MULT(5, 5)) = 26
			// MULT(5, 5) = 25
			// 26
		}
	}
}
