using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Utility
{
	public class OperationTests
	{
		[Fact]
		public void Ok_Should_Create_Okful_Result()
		{
			Result result = Result.Ok();

			Assert.Equal(Status.Ok, result.Status);
			Assert.Empty(result.Message);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_With_Message()
		{
			Result result = Result.Fail("error");

			Assert.Equal(Status.Fail, result.Status);
			Assert.Equal("error", result.Message);
		}

		[Fact]
		public void Equality_Should_Work_For_Identical_Values()
		{
			Result r1 = Result.Fail("error");
			Result r2 = Result.Fail("error");

			Assert.Equal(r1, r2);
			Assert.True(r1.Equals(r2));
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Differences()
		{
			Result r1 = Result.Fail("error1");
			Result r2 = Result.Fail("error2");

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Default_Instance_Should_Be_Failed_With_Default_Values()
		{
			Result result = default;

			Assert.Equal(Status.Fail, result.Status);
			Assert.Null(result.Message);
		}
	}



	public class OperationGenericTests
	{
		[Fact]
		public void Ok_Should_Create_Okful_Result_With_Value()
		{
			Result<int> result = Result<int>.Ok(42);

			Assert.Equal(Status.Ok, result.Status);
			Assert.Equal(42, result.Value);
			Assert.Empty(result.Message);
		}

		[Fact]
		public void Ok_Should_Allow_Null_For_Reference_Type()
		{
			Result<string> result = Result<string>.Ok(null);

			Assert.Equal(Status.Ok, result.Status);
			Assert.Empty(result.Message);
			Assert.Null(result.Value);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_Without_Value()
		{
			Result<int> result = Result<int>.Fail("error");

			Assert.Equal(Status.Fail, result.Status);
			Assert.Equal(default, result.Value);
			Assert.Equal("error", result.Message);
		}


		[Fact]
		public void Equality_Should_Work_For_Generic_Type()
		{
			Result<int> r1 = Result<int>.Ok(10);
			Result<int> r2 = Result<int>.Ok(10);

			Assert.Equal(r1, r2);
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Value()
		{
			Result<int> r1 = Result<int>.Ok(10);
			Result<int> r2 = Result<int>.Ok(20);

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Ok_State()
		{
			Result<int> r1 = Result<int>.Ok(10);
			Result<int> r2 = Result<int>.Fail("error");

			Assert.NotEqual(r1, r2);
		}
	}
}