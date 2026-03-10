
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;
using SmartExpressions.Test.Utility;

using Xunit.Abstractions;

namespace SmartExpressions.Test.Expressions
{
	public class RegisteredFunctionsTests(ITestOutputHelper outputHelper) : BaseTestClass(outputHelper)
	{
		[Fact]
		public void Registered_Function_Evaluates_Sinus_Correctly()
		{
			Expression expression = new Expression("func_sin(23)");


			_ = expression.RegisterFunction("func_sin", objects =>
			{
				Result<double> result = ExpressionHelpers.ResolveNumeric(objects[0]);
				if (result.Status == Status.Failure)
				{
					return Result<object>.Failure("Unable to resolve 'func_sin'. " + result.Message);
				}
				double value = result.Value;
				return Result<object>.Success(Math.Sin(value));
			});


			Progress<string> listener = new Progress<string>();
			listener.ProgressChanged += (_, e) => this._outputHelper.WriteLine(e);
			Result<object> result = expression.Evaluate(listener);
			if (result.Status == Status.Failure)
			{
				this._outputHelper.WriteLine(result.Message);
				throw new Exception(result.Message);
			}

			Assert.Equal(Status.Success, result.Status);
			Assert.Equal(Math.Sin(23), result.Value);
		}

		[Fact]
		public void Registered_Function_Evaluates_Cosinus_In_Sinus_Correctly()
		{
			Expression expression = new Expression("func_sin(func_cos(23))");


			_ = expression.RegisterFunction("func_sin", objects =>
			{
				Result<double> result = ExpressionHelpers.ResolveNumeric(objects[0]);
				if (result.Status == Status.Failure)
				{
					return Result<object>.Failure("Unable to resolve 'func_sin'. " + result.Message);
				}
				double value = result.Value;
				return Result<object>.Success(Math.Sin(value));
			});


			_ = expression.RegisterFunction("func_cos", objects =>
			{
				Result<double> result = ExpressionHelpers.ResolveNumeric(objects[0]);
				if (result.Status == Status.Failure)
				{
					return Result<object>.Failure("Unable to resolve 'func_cos'. " + result.Message);
				}
				double value = result.Value;
				return Result<object>.Success(Math.Cos(value));
			});


			Progress<string> listener = new Progress<string>();
			listener.ProgressChanged += (_, e) => this._outputHelper.WriteLine(e);
			Result<object> result = expression.Evaluate(listener);
			if (result.Status == Status.Failure)
			{
				this._outputHelper.WriteLine(result.Message);
				throw new Exception(result.Message);
			}

			Assert.Equal(Status.Success, result.Status);
			Assert.Equal(Math.Sin(Math.Cos(23)), result.Value);
		}

		[Fact]
		public void Registered_Function_Evaluates_Cosinus_Of_Identifier_In_Sinus_Correctly()
		{
			Expression expression = new Expression("func_sin(func_cos(@{VALUE_1}))");


			_ = expression.RegisterFunction("func_sin", objects =>
			{
				Result<double> result = ExpressionHelpers.ResolveNumeric(objects[0]);
				if (result.Status == Status.Failure)
				{
					return Result<object>.Failure("Unable to resolve 'func_sin'. " + result.Message);
				}
				double value = result.Value;
				return Result<object>.Success(Math.Sin(value));
			});


			_ = expression.RegisterFunction("func_cos", objects =>
			{
				Result<double> result = ExpressionHelpers.ResolveNumeric(objects[0]);
				if (result.Status == Status.Failure)
				{
					return Result<object>.Failure("Unable to resolve 'func_cos'. " + result.Message);
				}
				double value = result.Value;
				return Result<object>.Success(Math.Cos(value));
			});

			expression.RegisterBinding("VALUE_1", Math.PI);

			Progress<string> listener = new Progress<string>();
			listener.ProgressChanged += (_, e) => this._outputHelper.WriteLine(e);
			Result<object> result = expression.Evaluate(listener);
			if (result.Status == Status.Failure)
			{
				this._outputHelper.WriteLine(result.Message);
				throw new Exception(result.Message);
			}

			Assert.Equal(Status.Success, result.Status);
			Assert.Equal(Math.Sin(Math.Cos(Math.PI)), result.Value);
		}
	}
}
