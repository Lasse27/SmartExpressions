namespace SmartExpressions.Core.Nodes.Conditional
{
	public readonly record struct ConditionalBlock(ExpressionNode Condition, ExpressionNode Expression);
}
