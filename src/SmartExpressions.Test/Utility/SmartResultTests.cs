using System;

using Xunit;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Utility
{
	public class SmartResultTests
	{
		[Fact]
		public void Ok_Should_Create_Successful_Result()
		{
			SmartResult result = SmartResult.Ok();

			Assert.True(result.Success);
			Assert.Null(result.Message);
			Assert.Null(result.Exception);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_With_Message()
		{
			SmartResult result = SmartResult.Fail("error");

			Assert.False(result.Success);
			Assert.Equal("error", result.Message);
			Assert.Null(result.Exception);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_With_Message_And_Exception()
		{
			InvalidOperationException ex = new InvalidOperationException("failure");
			SmartResult result = SmartResult.Fail("error", ex);

			Assert.False(result.Success);
			Assert.Equal("error", result.Message);
			Assert.Same(ex, result.Exception);
		}

		[Fact]
		public void Equality_Should_Work_For_Identical_Values()
		{
			SmartResult r1 = SmartResult.Fail("error");
			SmartResult r2 = SmartResult.Fail("error");

			Assert.Equal(r1, r2);
			Assert.True(r1.Equals(r2));
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Differences()
		{
			SmartResult r1 = SmartResult.Fail("error1");
			SmartResult r2 = SmartResult.Fail("error2");

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Default_Instance_Should_Be_Failed_With_Default_Values()
		{
			SmartResult result = default;

			Assert.False(result.Success);
			Assert.Null(result.Message);
			Assert.Null(result.Exception);
		}
	}



	public class SmartResultGenericTests
	{
		[Fact]
		public void Ok_Should_Create_Successful_Result_With_Value()
		{
			SmartResult<int> result = SmartResult<int>.Ok(42);

			Assert.True(result.Success);
			Assert.Equal(42, result.Value);
			Assert.Null(result.Message);
			Assert.Null(result.Exception);
		}

		[Fact]
		public void Ok_Should_Allow_Null_For_Reference_Type()
		{
			SmartResult<string> result = SmartResult<string>.Ok(null);

			Assert.True(result.Success);
			Assert.Null(result.Value);
			Assert.Null(result.Message);
			Assert.Null(result.Exception);
		}

		[Fact]
		public void Fail_Should_Create_Failed_Result_Without_Value()
		{
			SmartResult<int> result = SmartResult<int>.Fail("error");

			Assert.False(result.Success);
			Assert.Equal(default, result.Value);
			Assert.Equal("error", result.Message);
			Assert.Null(result.Exception);
		}

		[Fact]
		public void Fail_Should_Preserve_Exception()
		{
			ArgumentException ex = new ArgumentException("invalid");

			SmartResult<string> result = SmartResult<string>.Fail("error", ex);

			Assert.False(result.Success);
			Assert.Null(result.Value);
			Assert.Equal("error", result.Message);
			Assert.Same(ex, result.Exception);
		}

		[Fact]
		public void Equality_Should_Work_For_Generic_Type()
		{
			SmartResult<int> r1 = SmartResult<int>.Ok(10);
			SmartResult<int> r2 = SmartResult<int>.Ok(10);

			Assert.Equal(r1, r2);
			Assert.True(r1 == r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Value()
		{
			SmartResult<int> r1 = SmartResult<int>.Ok(10);
			SmartResult<int> r2 = SmartResult<int>.Ok(20);

			Assert.NotEqual(r1, r2);
			Assert.True(r1 != r2);
		}

		[Fact]
		public void Equality_Should_Detect_Different_Success_State()
		{
			SmartResult<int> r1 = SmartResult<int>.Ok(10);
			SmartResult<int> r2 = SmartResult<int>.Fail("error");

			Assert.NotEqual(r1, r2);
		}

		[Fact]
		public void Default_Instance_Should_Have_Default_State()
		{
			SmartResult<int> result = default;

			Assert.False(result.Success);
			Assert.Equal(default, result.Value);
			Assert.Null(result.Message);
			Assert.Null(result.Exception);
		}

		[Fact]
		public void Equality_Should_Consider_Exception_Instance()
		{
			InvalidOperationException ex1 = new InvalidOperationException("x");
			InvalidOperationException ex2 = new InvalidOperationException("x");

			SmartResult<int> r1 = SmartResult<int>.Fail("error", ex1);
			SmartResult<int> r2 = SmartResult<int>.Fail("error", ex2);

			// Different exception instances -> not equal
			Assert.NotEqual(r1, r2);
		}
	}
}