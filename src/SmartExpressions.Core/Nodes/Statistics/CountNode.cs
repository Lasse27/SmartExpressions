using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record CountNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public CountNode(List<ExpressionNode> operands)
			=> this.operands = operands;


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new CountNode(operation.Value);
			return Result<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(this.operands.Count);
	}
}
