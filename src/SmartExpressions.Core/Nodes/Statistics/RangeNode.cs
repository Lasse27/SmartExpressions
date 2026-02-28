using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record RangeNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public RangeNode(List<ExpressionNode> operands)
			=> this.operands = operands;

		public override Operation<object> Evaluate() => throw new NotImplementedException();


		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new RangeNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
