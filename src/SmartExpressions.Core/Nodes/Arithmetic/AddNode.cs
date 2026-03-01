using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record AddNode : ExpressionNode
	{
		public ExpressionNode Left { get; set; }
		public ExpressionNode Right { get; set; }

		public AddNode(ExpressionNode left, ExpressionNode right)
		{
			this.Left = left;
			this.Right = right;
		}

		public override Operation<object> Evaluate() => throw new NotImplementedException();


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<DualOperand> dualOperand = ParserHelpers.ParseDualOperandKeyword(parser);
			if (dualOperand.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(dualOperand.Message);
			}

			ExpressionNode node = new AddNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
