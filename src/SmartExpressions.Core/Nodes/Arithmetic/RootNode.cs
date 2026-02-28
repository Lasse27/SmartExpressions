using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record RootNode : ExpressionNode
	{
		private readonly ExpressionNode operand;

		public RootNode(ExpressionNode operand) => this.operand = operand;

		public override Operation<object> Evaluate() => throw new NotImplementedException();

		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			Operation<DualOperand> dualOperand = ParserHelpers.ParseDualOperandKeyword(parser);
			if (dualOperand.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(dualOperand.Message);
			}

			ExpressionNode node = new RootNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
