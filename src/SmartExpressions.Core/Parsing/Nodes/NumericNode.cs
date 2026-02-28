using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Nodes
{
	public record NumericNode : ExpressionNode
	{
		public override Operation<object> Evaluate() => throw new NotImplementedException();
	}
}
