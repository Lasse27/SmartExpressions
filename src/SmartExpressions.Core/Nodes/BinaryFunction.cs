using System.Diagnostics;

namespace SmartExpressions.Core.Nodes
{
	/// <summary> Record class <see cref="BinaryFunction"/> which represents a node that handles two operands. </summary>
	/// <param name="Left">The first operand of the function.</param>
	/// <param name="Right">The second operand of the function.</param>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public abstract record BinaryFunction(ExpressionNode Left, ExpressionNode Right) : ExpressionNode
	{


		/// <inheritdoc/>
		public override string ToString() => $"{this.GetKeyword()}({this.Left}, {this.Right})";
	}
}
