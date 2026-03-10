using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Utility
{
	public class OperationTests
	{
		[Fact]
		public void Ok_Should_Create_Successful_Result()
		{
			Result result = Result.Success();

			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Message);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_With_Message()
		{
			Result result = Result.Failure("error");

			Assert.Equal(Status.Failure, result.Status);
			Assert.Equal("error", result.Message);
		}

		[Fact]
		public void Equality_Should_Work_For_Identical_Values()
		{
			Result r1 = Result.Failure("error");
			Result r2 = Result.Failure("error");

			Assert.Equal(r1, r2);
			Assert.True(r1.Equals(r2));
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Differences()
		{
			Result r1 = Result.Failure("error1");
			Result r2 = Result.Failure("error2");

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Default_Instance_Should_Be_Failed_With_Default_Values()
		{
			Result result = default;

			Assert.Equal(Status.Failure, result.Status);
			Assert.Null(result.Message);
		}
	}



	public class OperationGenericTests
	{
		[Fact]
		public void Ok_Should_Create_Successful_Result_With_Value()
		{
			Result<int> result = Result<int>.Success(42);

			Assert.Equal(Status.Success, result.Status);
			Assert.Equal(42, result.Value);
			Assert.Empty(result.Message);
		}

		[Fact]
		public void Ok_Should_Allow_Null_For_Reference_Type()
		{
			Result<string> result = Result<string>.Success(null);

			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Message);
			Assert.Null(result.Value);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_Without_Value()
		{
			Result<int> result = Result<int>.Failure("error");

			Assert.Equal(Status.Failure, result.Status);
			Assert.Equal(default, result.Value);
			Assert.Equal("error", result.Message);
		}


		[Fact]
		public void Equality_Should_Work_For_Generic_Type()
		{
			Result<int> r1 = Result<int>.Success(10);
			Result<int> r2 = Result<int>.Success(10);

			Assert.Equal(r1, r2);
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Value()
		{
			Result<int> r1 = Result<int>.Success(10);
			Result<int> r2 = Result<int>.Success(20);

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Success_State()
		{
			Result<int> r1 = Result<int>.Success(10);
			Result<int> r2 = Result<int>.Failure("error");

			Assert.NotEqual(r1, r2);
		}
	}
}