namespace SmartExpressions.Core.Utility
{

	public readonly record struct Result(Status Status, string Message = "") : IEquatable<Result>
	{
		public static Result Ok() => new Result(Status.Ok);

		public static Result Fail(string message) => new Result(Status.Fail, message);
	}



	public readonly record struct Result<T>(Status Status, T Value, string Message = "") : IEquatable<Result<T>>
	{
		public static Result<T> Ok(T value) => new Result<T>(Status.Ok, value);

		public static Result<T> Fail(string message = "") => new Result<T>(Status.Fail, default, message);
	}

	public enum Status
	{
		Fail,
		Pending,
		Ok,
	}
}