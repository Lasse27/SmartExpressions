using System;

using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SmartExpressions.Benchmark
{
	public class Program
	{
		private static void Main(string[] args)
		{
			BenchmarkSwitcher
				.FromAssembly(typeof(Program).Assembly)
				.Run(args);

			_ = Console.ReadKey();
		}
	}
}
