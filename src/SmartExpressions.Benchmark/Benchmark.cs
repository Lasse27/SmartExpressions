using System;
using System.Security.Cryptography;

using BenchmarkDotNet.Attributes;

using Microsoft.VSDiagnostics;

namespace SmartExpressions.Benchmark
{
	// For more information on the VS BenchmarkDotNet Diagnosers see https://learn.microsoft.com/visualstudio/profiling/profiling-with-benchmark-dotnet
	[CPUUsageDiagnoser]
	public class Benchmark
	{
		[GlobalSetup]
		public void Setup()
		{
		}
	}
}
