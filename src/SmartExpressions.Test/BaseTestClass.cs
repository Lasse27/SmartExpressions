using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace SmartExpressions.Test
{
	public class BaseTestClass(ITestOutputHelper outputHelper)
	{
		protected readonly ITestOutputHelper _outputHelper = outputHelper;
	}
}
