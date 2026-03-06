using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

using Xunit.Abstractions;

using static SmartExpressions.Test.Expressions.AddFunctionTests;

namespace SmartExpressions.Test.Utility
{
	public class BaseTestClass(ITestOutputHelper outputHelper)
	{
		protected readonly ITestOutputHelper _outputHelper = outputHelper;

		public object EvaluateSuccess(string formula, params Binding[] bindings)
		{
			Expression expression = new Expression(formula);
			foreach (Binding binding in bindings)
			{
				_ = expression.RegisterBinding(binding.Key, binding.Value);
			}
			_ = expression.Assemble();

			Progress<string> progress = new Progress<string>();
			progress.ProgressChanged += (s, e) => this._outputHelper.WriteLine(e);
			Result<object> result = expression.Evaluate(progress);
			if (result.Status == Status.Failure)
			{
				this._outputHelper.WriteLine("Input: " + formula);
				this._outputHelper.WriteLine("Fail: " + result.Message);
			}

			Assert.Equal(Status.Success, result.Status);
			return result.Value;
		}
	}
}
