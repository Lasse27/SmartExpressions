using System;

using BenchmarkDotNet.Attributes;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Benchmark.Utility
{
	[MemoryDiagnoser]
	public class SmartResultBenchmarks
	{
		private static readonly SmallClass _smallInstance = new SmallClass(42);
		private static readonly LargeClass _largeInstance = new LargeClass(123);
		private static readonly Exception _exception = new InvalidOperationException("test");

		// -------------------------
		// Small reference type
		// -------------------------

		[Benchmark]
		public SmartResult<SmallClass> Ok_SmallClass()
			=> SmartResult<SmallClass>.Ok(_smallInstance);

		[Benchmark]
		public SmartResult<SmallClass> Ok_SmallClass_Null()
			=> SmartResult<SmallClass>.Ok(null);

		[Benchmark]
		public SmartResult<SmallClass> Fail_SmallClass()
			=> SmartResult<SmallClass>.Fail("error");

		[Benchmark]
		public SmartResult<SmallClass> Fail_SmallClass_With_Exception()
			=> SmartResult<SmallClass>.Fail("error", _exception);

		// -------------------------
		// Large reference type
		// -------------------------

		[Benchmark]
		public SmartResult<LargeClass> Ok_LargeClass()
			=> SmartResult<LargeClass>.Ok(_largeInstance);

		[Benchmark]
		public SmartResult<LargeClass> Fail_LargeClass()
			=> SmartResult<LargeClass>.Fail("error");

		// -------------------------
		// Direct constructor comparison
		// -------------------------

		[Benchmark]
		public SmartResult<SmallClass> DirectCtor_SmallClass()
			=> new SmartResult<SmallClass>(true, _smallInstance);

		[Benchmark]
		public SmartResult<SmallClass> DirectCtor_Fail_SmallClass()
			=> new SmartResult<SmallClass>(false, null, "error", null);

		[Benchmark]
		public SmartResult Ok_NonGeneric()
			=> SmartResult.Ok();

		[Benchmark]
		public SmartResult Fail_NonGeneric()
			=> SmartResult.Fail("error");

		[Benchmark]
		public SmartResult Fail_With_Exception_NonGeneric()
			=> SmartResult.Fail("error", _exception);

		[Benchmark]
		public SmartResult<int> Ok_Generic()
			=> SmartResult<int>.Ok(42);

		[Benchmark]
		public SmartResult<int> Fail_Generic()
			=> SmartResult<int>.Fail("error");

		[Benchmark]
		public SmartResult<int> Fail_Generic_With_Exception()
			=> SmartResult<int>.Fail("error", _exception);
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