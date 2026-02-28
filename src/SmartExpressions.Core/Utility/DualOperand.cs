using SmartExpressions.Core.Nodes;

namespace SmartExpressions.Core.Utility
{
	public readonly struct DualOperand(ExpressionNode left, ExpressionNode right)
	{
		public ExpressionNode Left { get; } = left;
		public ExpressionNode Right { get; } = right;
	}
}
