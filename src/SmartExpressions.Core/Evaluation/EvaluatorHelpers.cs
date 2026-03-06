using System.Runtime.InteropServices;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Evaluation
{
	public static class EvaluatorHelpers
	{
		public static Result<double> ResolveDouble(Result<object> operand, string callerName)
		{
			if (operand.Status == Status.Failure)
			{
				return Result<double>.Failure(operand.Message);
			}

			// Switch types
			return operand.Value switch
			{
				bool bool_ => Result<double>.Success(bool_ == true ? 1 : 0),
				double double_ => Result<double>.Success(Convert.ToDouble( double_)),
				decimal decimal_ => Result<double>.Success(Convert.ToDouble(decimal_)),
				long long_ => Result<double>.Success(Convert.ToDouble(long_)),
				int int_ => Result<double>.Success(Convert.ToDouble(int_)),
				short short_ => Result<double>.Success(Convert.ToDouble(short_)),
				byte byte_ => Result<double>.Success(Convert.ToDouble(byte_)),
				null => Result<double>.Failure($"{callerName} does not support null."),
				_ => Result<double>.Failure($"{callerName} does not support type '{operand.Value.GetType().Name}'.")
			};
		}

		public static Result<bool> ResolveBoolean(Result<object> operand, string callerName)
		{
			if (operand.Status == Status.Failure)
			{
				return Result<bool>.Failure(operand.Message);
			}

			// Switch types
			return operand.Value switch
			{
				bool bool_ => Result<bool>.Success(bool_),
				double double_ => Result<bool>.Success(double_ != 0),
				decimal decimal_ => Result<bool>.Success(decimal_ != 0),
				long long_ => Result<bool>.Success(long_ != 0),
				int int_ => Result<bool>.Success(int_ != 0),
				short short_ => Result<bool>.Success(short_ != 0),
				byte byte_ => Result<bool>.Success(byte_ != 0),
				null => Result<bool>.Failure($"{callerName} does not support null."),
				_ => Result<bool>.Failure($"{callerName} does not support type '{operand.Value.GetType().Name}'.")
			};
		}
	}
}
