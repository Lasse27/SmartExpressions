using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartExpressions.Core.Evaluation
{
	public readonly record struct EvaluatorOptions
	{
		public static readonly EvaluatorOptions Default = new();

		public EvaluatorOptions()
		{
		}

		public int DecimalPrecision { get; init; } = 10;

		public bool StrictTypes { get; init; } = true;

		public bool AllowImplicitNullCoercion { get; init; } = false;

		public bool StrictIdentifierResolution { get; init; } = true;

		public bool CaseSensitiveIdentifiers { get; init; } = false;

		public bool ShortCircuitLogical { get; init; } = true;

		public bool ThrowOnOverflow { get; init; } = true;

		public int MaxDepth { get; init; } = 100;

		public decimal PiOverride { get; init; } = (decimal)Math.PI;

		public decimal EulerOverride { get; init; } = (decimal)Math.E;
	}
}
