using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Expressions
{
	public abstract record ExpressionNode : IEvaluable
	{
		/// <inheritdoc/>
		public Operation<object> Evaluate() => throw new NotImplementedException();
	}
}
