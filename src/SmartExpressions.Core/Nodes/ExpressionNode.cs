using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	/// <summary> Record class <see cref="ExpressionNode"/> which represents an evaluable node in the syntax tree of the expression. </summary>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public abstract record ExpressionNode : IEvaluable
	{
		/// <inheritdoc/>
		public abstract Result<object> Evaluate(EvaluationContext ctx);

		/// <inheritdoc/>
		protected string GetDebuggerDisplay() => this.ToString();

		/// <inheritdoc/>
		public override int GetHashCode() => base.GetHashCode();
	}
}
