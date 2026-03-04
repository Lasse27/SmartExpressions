namespace SmartExpressions.Core.Nodes
{
	/// <summary> Record class <see cref="BinaryFunction"/> which represents a node that handles a collection of operands. </summary>
	/// <param name="Operands">The collection of operands of the function.</param>
	public abstract record CompositeFunction(List<ExpressionNode> Operands) : ExpressionNode
	{
		/// <inheritdoc/>
		public override string ToString() => $"{this.GetKeyword()}({string.Join(',', this.Operands)})";
	}
}
