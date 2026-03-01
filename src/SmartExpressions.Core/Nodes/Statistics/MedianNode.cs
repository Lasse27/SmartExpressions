using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record MedianNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public MedianNode(List<ExpressionNode> operands)
			=> this.operands = operands;

		public override Operation<object> Evaluate() => throw new NotImplementedException();


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new MedianNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
