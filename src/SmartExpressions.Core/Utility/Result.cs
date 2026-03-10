namespace SmartExpressions.Core.Utility
{

	public readonly record struct Result(Status Status, string Message = "") : IEquatable<Result>
	{
		public static Result Success() => new Result(Status.Success);

		public static Result Failure(string message) => new Result(Status.Failure, message);
	}



	public readonly record struct Result<T>(Status Status, T Value, string Message = "") : IEquatable<Result<T>>
	{
		public static Result<T> Success(T value) => new Result<T>(Status.Success, value);

		public static Result<T> Failure(string message = "") => new Result<T>(Status.Failure, default, message);
	}

	public enum Status
	{
		Failure,
		Pending,
		Success,
	}
}