namespace SmartExpressions.Core.Nodes
{
	/// <inheritdoc/>
	public abstract record CompositeFunction(List<ExpressionNode> Operands) : ExpressionNode
	{
		/// <inheritdoc/>
		public override string ToString() => $"{this.GetKeyword()}({string.Join(',', this.Operands)})";
	}
}
