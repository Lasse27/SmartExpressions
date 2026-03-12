using System.Globalization;

namespace SmartExpressions.Core.Utility
{
	/// <summary> 
	/// Provides helper methods used in the evaluation of expressions.
	/// </summary>
	public static class ExpressionHelpers
	{

		/// <summary>
		/// Resolves a numeric value from a passed <see cref="Result{T}"/> containing a primitive/convertible value.
		/// </summary>
		/// <param name="obj"> The object that should be converted to a numeric. </param>
		/// <returns> A <see cref="Result{T}"/> representing the conversion operation. </returns>
		public static Result<double> ResolveNumeric(EvaluationResult obj)
		{
			if (obj.IsFail())
			{
				return Result<double>.Fail(obj.GetMessage());
			}
			else
			{
				// Call unwrapped
				return ResolveNumeric(obj.GetValue());
			}
		}

		/// <summary>
		/// Resolves a numeric value from a passed <see cref="object"/> containing a primitive/convertible value.
		/// </summary>
		/// <param name="obj"> The object that should be converted to a numeric. </param>
		/// <returns> A <see cref="Result{T}"/> representing the conversion operation. </returns>
		public static Result<double> ResolveNumeric(object obj)
		{
			if (obj == null)
			{
				return Result<double>.Fail($"Can't resolve numeric value from null.");
			}
			try
			{
				double val = Convert.ToDouble(obj);
				return Result<double>.Ok(val);
			}
			catch (InvalidCastException ex)
			{
				return Result<double>.Fail($"Can't numeric value." + ex.Message);
			}
			catch (FormatException ex)
			{
				return Result<double>.Fail($"Can't numeric value." + ex.Message);
			}
			catch (OverflowException ex)
			{
				return Result<double>.Fail($"Can't numeric value." + ex.Message);
			}
		}

		/// <summary>
		/// Resolves a boolean value from a passed <see cref="Result{T}"/> containing a primitive/convertible value.
		/// </summary>
		/// <param name="obj"> The object that should be converted to a boolean. </param>
		/// <returns> A <see cref="Result{T}"/> representing the conversion operation. </returns>
		public static Result<bool> ResolveBoolean(EvaluationResult obj)
		{
			if (obj.IsFail())
			{
				return Result<bool>.Fail(obj.GetMessage());
			}
			else
			{
				// Call unwrapped
				return ResolveBoolean(obj.GetValue());
			}
		}

		/// <summary>
		/// Resolves a boolean value from a passed <see cref="object"/> containing a primitive/convertible value.
		/// </summary>
		/// <param name="obj"> The object that should be converted to a boolean. </param>
		/// <returns> A <see cref="Result{T}"/> representing the conversion operation. </returns>
		public static Result<bool> ResolveBoolean(object obj)
		{
			if (obj == null)
			{
				return Result<bool>.Fail("Can't resolve boolean value from null.");
			}

			// if obj != 0 then return true.
			return obj switch
			{
				bool b => Result<bool>.Ok(b),
				sbyte v => Result<bool>.Ok(v != 0),
				short v => Result<bool>.Ok(v != 0),
				int v => Result<bool>.Ok(v != 0),
				long v => Result<bool>.Ok(v != 0),
				byte v => Result<bool>.Ok(v != 0),
				ushort v => Result<bool>.Ok(v != 0),
				uint v => Result<bool>.Ok(v != 0),
				ulong v => Result<bool>.Ok(v != 0),
				float v => Result<bool>.Ok(v != 0f),
				double v => Result<bool>.Ok(v != 0d),
				decimal v => Result<bool>.Ok(v != 0m),
				char v => Result<bool>.Ok(v != '\0'),
				string s when bool.TryParse(s, out bool b) => Result<bool>.Ok(b),
				string s when double.TryParse(s, out double d) => Result<bool>.Ok(d != 0),

				// fallback
				IConvertible c => Result<bool>.Ok(c.ToBoolean(CultureInfo.InvariantCulture)),
				_ => Result<bool>.Fail($"Type '{obj.GetType()}' cannot be converted to boolean.")
			};
		}
	}
}
