using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Nodes
{
	public record NullNode : ExpressionNode
	{
		public override Operation<object> Evaluate() => throw new NotImplementedException();
	}
}