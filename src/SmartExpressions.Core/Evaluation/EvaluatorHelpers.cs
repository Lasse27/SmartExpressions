using System.Runtime.InteropServices;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Evaluation
{
	public static class EvaluatorHelpers
	{
		public static Operation<double> ResolveDouble(Operation<object> operand, string callerName)
		{
			if (operand.Status == Status.Failure)
			{
				return Operation<double>.Failure(operand.Message);
			}

			// Switch types
			return operand.Value switch
			{
				bool bool_ => Operation<double>.Success(bool_ == true ? 1 : 0),
				double double_ => Operation<double>.Success(Convert.ToDouble( double_)),
				decimal decimal_ => Operation<double>.Success(Convert.ToDouble(decimal_)),
				long long_ => Operation<double>.Success(Convert.ToDouble(long_)),
				int int_ => Operation<double>.Success(Convert.ToDouble(int_)),
				short short_ => Operation<double>.Success(Convert.ToDouble(short_)),
				byte byte_ => Operation<double>.Success(Convert.ToDouble(byte_)),
				null => Operation<double>.Failure($"{callerName} does not support null."),
				_ => Operation<double>.Failure($"{callerName} does not support type '{operand.Value.GetType().Name}'.")
			};
		}

		public static Operation<bool> ResolveBoolean(Operation<object> operand, string callerName)
		{
			if (operand.Status == Status.Failure)
			{
				return Operation<bool>.Failure(operand.Message);
			}

			// Switch types
			return operand.Value switch
			{
				bool bool_ => Operation<bool>.Success(bool_),
				double double_ => Operation<bool>.Success(double_ != 0),
				decimal decimal_ => Operation<bool>.Success(decimal_ != 0),
				long long_ => Operation<bool>.Success(long_ != 0),
				int int_ => Operation<bool>.Success(int_ != 0),
				short short_ => Operation<bool>.Success(short_ != 0),
				byte byte_ => Operation<bool>.Success(byte_ != 0),
				null => Operation<bool>.Failure($"{callerName} does not support null."),
				_ => Operation<bool>.Failure($"{callerName} does not support type '{operand.Value.GetType().Name}'.")
			};
		}
	}
}
