using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Utility
{
	public class OperationTests
	{
		[Fact]
		public void Ok_Should_Create_Successful_Result()
		{
			Operation result = Operation.Success();

			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Message);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_With_Message()
		{
			Operation result = Operation.Failure("error");

			Assert.Equal(Status.Failure, result.Status);
			Assert.Equal("error", result.Message);
		}

		[Fact]
		public void Equality_Should_Work_For_Identical_Values()
		{
			Operation r1 = Operation.Failure("error");
			Operation r2 = Operation.Failure("error");

			Assert.Equal(r1, r2);
			Assert.True(r1.Equals(r2));
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Differences()
		{
			Operation r1 = Operation.Failure("error1");
			Operation r2 = Operation.Failure("error2");

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Default_Instance_Should_Be_Failed_With_Default_Values()
		{
			Operation result = default;

			Assert.Equal(Status.Failure, result.Status);
			Assert.Null(result.Message);
		}
	}



	public class OperationGenericTests
	{
		[Fact]
		public void Ok_Should_Create_Successful_Result_With_Value()
		{
			Operation<int> result = Operation<int>.Success(42);

			Assert.Equal(Status.Success, result.Status);
			Assert.Equal(42, result.Value);
			Assert.Empty(result.Message);
		}

		[Fact]
		public void Ok_Should_Allow_Null_For_Reference_Type()
		{
			Operation<string> result = Operation<string>.Success(null);

			Assert.Equal(Status.Success, result.Status);
			Assert.Empty(result.Message);
			Assert.Null(result.Value);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_Without_Value()
		{
			Operation<int> result = Operation<int>.Failure("error");

			Assert.Equal(Status.Failure, result.Status);
			Assert.Equal(default, result.Value);
			Assert.Equal("error", result.Message);
		}


		[Fact]
		public void Equality_Should_Work_For_Generic_Type()
		{
			Operation<int> r1 = Operation<int>.Success(10);
			Operation<int> r2 = Operation<int>.Success(10);

			Assert.Equal(r1, r2);
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Value()
		{
			Operation<int> r1 = Operation<int>.Success(10);
			Operation<int> r2 = Operation<int>.Success(20);

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Success_State()
		{
			Operation<int> r1 = Operation<int>.Success(10);
			Operation<int> r2 = Operation<int>.Failure("error");

			Assert.NotEqual(r1, r2);
		}
	}
}