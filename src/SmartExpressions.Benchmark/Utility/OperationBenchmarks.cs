using BenchmarkDotNet.Attributes;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Benchmark.Utility
{
	[MemoryDiagnoser]
	public class OperationBenchmarks
	{
		private static readonly SmallClass _smallInstance = new SmallClass(42);
		private static readonly LargeClass _largeInstance = new LargeClass(123);

		// -------------------------
		// Small reference type
		// -------------------------

		[Benchmark]
		public Result<SmallClass> Ok_SmallClass()
			=> Result<SmallClass>.Ok(_smallInstance);

		[Benchmark]
		public Result<SmallClass> Ok_SmallClass_Null()
			=> Result<SmallClass>.Ok(null);

		[Benchmark]
		public Result<SmallClass> Fail_SmallClass()
			=> Result<SmallClass>.Fail("error");

		// -------------------------
		// Large reference type
		// -------------------------

		[Benchmark]
		public Result<LargeClass> Ok_LargeClass()
			=> Result<LargeClass>.Ok(_largeInstance);

		[Benchmark]
		public Result<LargeClass> Fail_LargeClass()
			=> Result<LargeClass>.Fail("error");

		// -------------------------
		// Direct constructor comparison
		// -------------------------

		[Benchmark]
		public Result<SmallClass> DirectCtor_SmallClass()
			=> new Result<SmallClass>(Status.Ok, _smallInstance);

		[Benchmark]
		public Result<SmallClass> DirectCtor_Fail_SmallClass()
			=> new Result<SmallClass>(Status.Fail, null, "error");

		[Benchmark]
		public Result Ok_NonGeneric()
			=> Result.Ok();

		[Benchmark]
		public Result Fail_NonGeneric()
			=> Result.Fail("error");

		[Benchmark]
		public Result<int> Ok_Generic()
			=> Result<int>.Ok(42);

		[Benchmark]
		public Result<int> Fail_Generic()
			=> Result<int>.Fail("error");
	}

	public sealed class SmallClass(int value)
	{
		public int Value { get; } = value;
	}

	public sealed class LargeClass
	{
		public int A, B, C, D, E, F, G, H;
		public string Text;

		public LargeClass(int seed)
		{
			this.A = this.B = this.C = this.D = this.E = this.F = this.G = this.H = seed;
			this.Text = "payload";
		}
	}
}