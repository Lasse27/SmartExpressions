using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public abstract record ExpressionNode : IEvaluable
	{
		/// <inheritdoc/>
		public abstract Operation<object> Evaluate();
	}
}
