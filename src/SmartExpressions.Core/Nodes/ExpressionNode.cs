using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public abstract record ExpressionNode : IEvaluable
	{
		/// <inheritdoc/>
		public abstract Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default);

		/// <inheritdoc/>
		protected string GetDebuggerDisplay() => this.ToString();

		/// <inheritdoc/>
		public override int GetHashCode() => base.GetHashCode();
	}
}
