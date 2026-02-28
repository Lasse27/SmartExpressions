using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Logical
{
	public record NotNode : ExpressionNode
	{
		private readonly ExpressionNode _left;
		private readonly ExpressionNode _right;

		public NotNode(ExpressionNode left, ExpressionNode right)
		{
			this._left = left;
			this._right = right;
		}

		public override Operation<object> Evaluate() => throw new NotImplementedException();


		public static Operation<ExpressionNode> Parse(Parser parser)
		{

		}
	}
}
