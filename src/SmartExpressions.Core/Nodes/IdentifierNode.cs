using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record IdentifierNode : ExpressionNode
	{
		public override Operation<object> Evaluate()
		{
			throw new NotImplementedException();
		}
	}
}
