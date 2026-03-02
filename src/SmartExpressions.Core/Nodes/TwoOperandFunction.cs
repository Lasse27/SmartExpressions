using System.Diagnostics;

namespace SmartExpressions.Core.Nodes
{
	/// <summary> Record class <see cref="TwoOperandFunction"/> which represents a node that handles two operands. </summary>
	/// <param name="Left">The first operand of the function.</param>
	/// <param name="Right">The second operand of the function.</param>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public abstract record TwoOperandFunction(ExpressionNode Left, ExpressionNode Right) : ExpressionNode
	{
		/// <summary> Gets the related keyword for the node. </summary>
		/// <returns> A <see cref="string"/> representing the nodes keyword. </returns>
		public abstract string GetKeyword();

		/// <inheritdoc/>
		public override string ToString() => $"{this.GetKeyword()}({this.Left}, {this.Right})";
	}
}
