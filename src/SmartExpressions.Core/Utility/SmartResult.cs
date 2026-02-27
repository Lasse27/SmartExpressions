namespace SmartExpressions.Core.Utility
{
	/// <summary> Represents the outcome of an operation without a return value. </summary>
	/// <param name="Success">Indicates whether the operation completed successfully.</param>
	/// <param name="Message">Optional descriptive message associated with the result.</param>
	/// <param name="Exception">Optional exception describing the failure cause.</param>
	public readonly record struct SmartResult(bool Success, string? Message = default, Exception? Exception = default) : IEquatable<SmartResult>
	{
		/// <summary> Creates a successful result. </summary>
		/// <returns>A <see cref="SmartResult"/> representing a successful operation.</returns>
		public static SmartResult Ok() => new SmartResult(true);


		/// <summary> Creates a failed result. </summary>
		/// <param name="message">Optional descriptive error message.</param>
		/// <param name="exception">Optional exception describing the failure.</param>
		/// <returns>A <see cref="SmartResult"/> representing a failed operation.</returns>
		public static SmartResult Fail(string message, Exception? exception = default)
			=> new SmartResult(false, message, exception);
	}



	/// <summary> Represents the outcome of an operation that returns a value. </summary>
	/// <typeparam name="T">The type of the result value.</typeparam>
	/// <param name="Success">Indicates whether the operation completed successfully.</param>
	/// <param name="Value">The value returned by the operation if successful.</param>
	/// <param name="Message">Optional descriptive message associated with the result.</param>
	/// <param name="Exception">Optional exception describing the failure cause.</param>
	public readonly record struct SmartResult<T>(bool Success, T? Value, string? Message = default, Exception? Exception = default) : IEquatable<SmartResult<T>>
	{
		/// <summary> Creates a successful result with the specified value. </summary>
		/// <param name="value">The value produced by the operation.</param>
		/// <returns>A <see cref="SmartResult{T}"/> representing a successful operation.</returns>
		public static SmartResult<T> Ok(T value) => new SmartResult<T>(true, value);


		/// <summary> Creates a failed result. </summary>
		/// <param name="message">Optional descriptive error message.</param>
		/// <param name="exception">Optional exception describing the failure.</param>
		/// <returns>A <see cref="SmartResult{T}"/> representing a failed operation.</returns>
		public static SmartResult<T> Fail(string? message = default, Exception? exception = default)
			=> new SmartResult<T>(false, default, message, exception);
	}
}