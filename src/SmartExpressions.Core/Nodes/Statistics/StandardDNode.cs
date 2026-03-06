using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record StandardDNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public StandardDNode(List<ExpressionNode> operands)
			=> this.operands = operands;


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new StandardDNode(operation.Value);
			return Result<ExpressionNode>.Success(node);
		}

		public override Result<object> Evaluate(EvaluationContext ctx) => throw new NotImplementedException();
	}
}
