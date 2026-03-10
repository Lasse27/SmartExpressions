using System.Diagnostics;

namespace SmartExpressions.Core.Expressions
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public readonly record struct ExpressionSettings
	{
		/// <summary> 
		/// Gets or sets wether to assemble the expression on the first call of <see cref="Expression.Evaluate(IProgress{string})"/>. 
		/// </summary>
		public bool AssembleOnEvaluation { get; }

		/// <summary> 
		/// Gets or sets the digit count when rounding decimals. 
		/// </summary>
		public int RoundingDigits { get; }

		/// <summary> 
		/// Gets or sets wether to short circuit expressions and not evaluate them completely. 
		/// </summary>
		public bool ShortCircuitExpressions { get; }


		/// <summary>
		/// Initializes a new instance of <see cref="ExpressionSettings"/>.
		/// </summary>
		public ExpressionSettings()
		{
			this.AssembleOnEvaluation = true;
			this.RoundingDigits = 10;
			this.ShortCircuitExpressions = true;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ExpressionSettings"/>.
		/// </summary>
		public ExpressionSettings(bool assembleOnEvaluation, int roundingDigits, bool shortCircuitExpressions)
		{
			this.AssembleOnEvaluation = assembleOnEvaluation;
			this.RoundingDigits = roundingDigits;
			this.ShortCircuitExpressions = shortCircuitExpressions;
		}


		private string GetDebuggerDisplay() => this.ToString();
	}
}
