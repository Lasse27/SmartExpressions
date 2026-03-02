using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Evaluation
{
	public static class EvaluatorHelpers
	{
		public static Operation<decimal> ResolveDecimal(Operation<object> operand, string callerName)
		{
			if (operand.Status == Status.Failure)
			{
				return Operation<decimal>.Failure(operand.Message);
			}

			// Switch types
			return operand.Value switch
			{
				bool bool_ => Operation<decimal>.Success(bool_ == true ? 1 : 0),
				double double_ => Operation<decimal>.Success((decimal)double_),
				decimal decimal_ => Operation<decimal>.Success(decimal_),
				long long_ => Operation<decimal>.Success(long_),
				int int_ => Operation<decimal>.Success(int_),
				short short_ => Operation<decimal>.Success(short_),
				byte byte_ => Operation<decimal>.Success(byte_),
				null => Operation<decimal>.Failure($"{callerName} does not support null."),
				_ => Operation<decimal>.Failure($"{callerName} does not support type '{operand.Value.GetType().Name}'.")
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
				double double_ => Operation<bool>.Success(double_ > 0),
				decimal decimal_ => Operation<bool>.Success(decimal_ > 0),
				long long_ => Operation<bool>.Success(long_ > 0),
				int int_ => Operation<bool>.Success(int_ > 0),
				short short_ => Operation<bool>.Success(short_ > 0),
				byte byte_ => Operation<bool>.Success(byte_ > 0),
				null => Operation<bool>.Failure($"{callerName} does not support null."),
				_ => Operation<bool>.Failure($"{callerName} does not support type '{operand.Value.GetType().Name}'.")
			};
		}
	}
}
