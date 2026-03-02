using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	/// <summary> Record class <see cref="ExpressionNode"/> which represents an evaluable node in the syntax tree of the expression. </summary>
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
