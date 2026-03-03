using Xunit.Abstractions;

namespace SmartExpressions.Test.Expressions
{
	public partial class AddFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "ADD";
		protected override double Compute(object left, object right) => Convert.ToDouble(left) + Convert.ToDouble(right);
	}

	public class SubtractFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "SUB";
		protected override double Compute(object left, object right) => Convert.ToDouble(left) - Convert.ToDouble(right);
	}

	public class MultiplyFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "MULT";
		protected override double Compute(object left, object right) => Convert.ToDouble(left) * Convert.ToDouble(right);
	}

	public class DivideFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "DIV";
		protected override double Compute(object left, object right) => Convert.ToDouble(left) / Convert.ToDouble(right);
		protected override bool IsValidInput(object left, object right) => Convert.ToDouble(right) != 0;
	}

	public class ModuloFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "MOD";
		protected override double Compute(object left, object right) => Convert.ToDouble(left) % Convert.ToDouble(right);
		protected override bool IsValidInput(object left, object right) => Convert.ToDouble(right) != 0;
	}

	public class PowerFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "POW";
		protected override double Compute(object left, object right)
		{
			double dl = left is double ldec
				? ldec
				: Convert.ToDouble(left);

			double dr = right is double rdec
				? rdec
				: Convert.ToDouble(right);

			return Math.Pow(dl, dr);
		}
	}

	public class RootFunctionTests(ITestOutputHelper o) : ArithmeticFunctionTestBase(o)
	{
		protected override string FunctionName => "ROOT";
		protected override double Compute(object left, object right)
		{
			double dl = left is double ldec
				? ldec
				: Convert.ToDouble(left);

			double dr = right is double rdec
				? rdec
				: Convert.ToDouble(right);

			return Math.Pow(dl, 1.0 / dr);
		}

		protected override bool IsValidInput(object left, object right)
		{
			// 
			return Convert.ToDouble(left) >= 0 && Convert.ToDouble(right) != 0;
		}
	}
}