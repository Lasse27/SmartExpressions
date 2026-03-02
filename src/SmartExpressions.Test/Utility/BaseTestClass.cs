using Xunit.Abstractions;

namespace SmartExpressions.Test
{
	public class BaseTestClass(ITestOutputHelper outputHelper)
	{
		protected readonly ITestOutputHelper _outputHelper = outputHelper;
	}
}
