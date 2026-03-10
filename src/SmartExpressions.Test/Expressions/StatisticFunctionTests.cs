using Xunit.Abstractions;

namespace SmartExpressions.Test.Expressions
{
	public class SumFunctionTests(ITestOutputHelper o) : StatisticFunctionTestBase(o)
	{
		protected override string FunctionName => "SUM";
		protected override double Compute(List<object> operands)
		{
			double sum = 0;
			foreach (object operand in operands)
			{
				sum += Convert.ToDouble(operand);
			}
			return sum;
		}
	}

	public class MinFunctionTests(ITestOutputHelper o) : StatisticFunctionTestBase(o)
	{
		protected override string FunctionName => "MIN";
		protected override double Compute(List<object> operands)
		{
			double min = double.MaxValue;
			foreach (object operand in operands)
			{
				double d = Convert.ToDouble(operand);
				if (d < min)
				{
					min = d;
				}
			}
			return min;
		}
	}

	public class MaxFunctionTests(ITestOutputHelper o) : StatisticFunctionTestBase(o)
	{
		protected override string FunctionName => "MAX";
		protected override double Compute(List<object> operands)
		{
			double max = double.MinValue;
			foreach (object operand in operands)
			{
				double d = Convert.ToDouble(operand);
				if (d > max)
				{
					max = d;
				}
			}
			return max;
		}
	}

	public class CountFunctionTests(ITestOutputHelper o) : StatisticFunctionTestBase(o)
	{
		protected override string FunctionName => "COUNT";
		protected override double Compute(List<object> operands) => Convert.ToDouble(operands.Count);
	}

	public class AvgFunctionTests(ITestOutputHelper o) : StatisticFunctionTestBase(o)
	{
		protected override string FunctionName => "AVG";
		protected override double Compute(List<object> operands)
		{
			double sum = 0;
			foreach (object operand in operands)
			{
				sum += Convert.ToDouble(operand);
			}
			return sum / operands.Count;
		}
	}

	public class RangeFunctionTests(ITestOutputHelper o) : StatisticFunctionTestBase(o)
	{
		protected override string FunctionName => "RANGE";
		protected override double Compute(List<object> operands)
		{
			double min = double.MaxValue;
			double max = double.MinValue;
			foreach (object operand in operands)
			{
				double d = Convert.ToDouble(operand);
				if (d < min)
				{
					min = d;
				}
				if (d > max)
				{
					max = d;
				}
			}
			return max - min;
		}
	}
}
