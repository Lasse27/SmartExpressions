using Xunit.Abstractions;

namespace SmartExpressions.Test.Expressions
{
	public class SinFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "SIN";
		protected override double Compute(double operand) => Math.Sin(operand);
	}

	public class CosFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "COS";
		protected override double Compute(double operand) => Math.Cos(operand);
	}

	public class TanFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "TAN";
		protected override double Compute(double operand) => Math.Tan(operand);
	}

	public class ASinFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "ASIN";
		protected override double Compute(double operand) => Math.Asin(operand);
		protected override bool IsValidInput(double operand) => operand is >= -1 and <= 1;
	}

	public class ACosFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "ACOS";
		protected override double Compute(double operand) => Math.Acos(operand);
		protected override bool IsValidInput(double operand) => operand is >= -1 and <= 1;
	}

	public class ATanFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "ATAN";
		protected override double Compute(double operand) => Math.Atan(operand);
	}

	public class SinhFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "SINH";
		protected override double Compute(double operand) => Math.Sinh(operand);
	}

	public class CoshFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "COSH";
		protected override double Compute(double operand) => Math.Cosh(operand);
	}

	public class TanhFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "TANH";
		protected override double Compute(double operand) => Math.Tanh(operand);
	}

	public class DegFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "DEG";
		protected override double Compute(double operand) => operand * 180.0 / Math.PI;
	}

	public class RadFunctionTests(ITestOutputHelper o) : TrigonometricFunctionTestBase(o)
	{
		protected override string FunctionName => "RAD";
		protected override double Compute(double operand) => operand * Math.PI / 180.0;
	}
}