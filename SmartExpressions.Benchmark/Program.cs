using System;

using BenchmarkDotNet.Running;

namespace SmartExpressions.Benchmark
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			_ = BenchmarkRunner.Run(typeof(Program).Assembly);
			_ = Console.ReadKey();
		}
	}
}
