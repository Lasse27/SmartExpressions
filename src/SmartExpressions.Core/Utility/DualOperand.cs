using SmartExpressions.Core.Nodes;

namespace SmartExpressions.Core.Utility
{
	public readonly record struct DualOperand(ExpressionNode Left, ExpressionNode Right)
	{
	}
}
