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
		public Operation<SmallClass> Ok_SmallClass()
			=> Operation<SmallClass>.Success(_smallInstance);

		[Benchmark]
		public Operation<SmallClass> Ok_SmallClass_Null()
			=> Operation<SmallClass>.Success(null);

		[Benchmark]
		public Operation<SmallClass> Fail_SmallClass()
			=> Operation<SmallClass>.Failure("error");

		// -------------------------
		// Large reference type
		// -------------------------

		[Benchmark]
		public Operation<LargeClass> Ok_LargeClass()
			=> Operation<LargeClass>.Success(_largeInstance);

		[Benchmark]
		public Operation<LargeClass> Fail_LargeClass()
			=> Operation<LargeClass>.Failure("error");

		// -------------------------
		// Direct constructor comparison
		// -------------------------

		[Benchmark]
		public Operation<SmallClass> DirectCtor_SmallClass()
			=> new Operation<SmallClass>(Status.Success, _smallInstance);

		[Benchmark]
		public Operation<SmallClass> DirectCtor_Fail_SmallClass()
			=> new Operation<SmallClass>(Status.Failure, null, "error");

		[Benchmark]
		public Operation Ok_NonGeneric()
			=> Operation.Success();

		[Benchmark]
		public Operation Fail_NonGeneric()
			=> Operation.Failure("error");

		[Benchmark]
		public Operation<int> Ok_Generic()
			=> Operation<int>.Success(42);

		[Benchmark]
		public Operation<int> Fail_Generic()
			=> Operation<int>.Failure("error");
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