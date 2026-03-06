using SmartExpressions.Core.Nodes;

namespace SmartExpressions.Core.Utility
{
	public readonly record struct BinaryOperand(ExpressionNode Left, ExpressionNode Right)
	{
	}
}
