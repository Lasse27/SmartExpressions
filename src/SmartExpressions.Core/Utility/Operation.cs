namespace SmartExpressions.Core.Utility
{

	public readonly record struct Operation(Status Status, string Message = "") : IEquatable<Operation>
	{
		public static Operation Success()
			=> new Operation(Status.Success);

		public static Operation Failure(string message)
			=> new Operation(Status.Failure, message);
	}



	public readonly record struct Operation<T>(Status Status, T Value, string Message = "") : IEquatable<Operation<T>>
	{
		public static Operation<T> Success(T value)
			=> new Operation<T>(Status.Success, value);

		public static Operation<T> Failure(string message = "")
			=> new Operation<T>(Status.Failure, default, message);
	}

	public enum Status
	{
		Failure,
		Pending,
		Success,
	}
}