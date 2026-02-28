using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Nodes
{
	public abstract record ExpressionNode : IEvaluable
	{
		/// <inheritdoc/>
		public abstract Operation<object> Evaluate();
	}
}
