using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartExpressions.Core.Expressions
{
	public record ExpressionSettings
	{
		/// <summary> Gets or sets wether to assemble the expression on the first call of <see cref="Expression.Evaluate(IProgress{string})"/>. </summary>
		public bool AssembleOnEvaluation { get; set; } = true;

		/// <summary> Gets or sets the digit count when rounding decimals. </summary>
		public int RoundingDigits { get; set; } = 10;

		/// <summary> Gets or sets wether to short circuit expressions and not evaluate them completely. </summary>
		public bool ShortCircuitExpressions { get; set; } = true;
	}
}
