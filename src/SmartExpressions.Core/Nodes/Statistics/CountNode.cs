using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record CountNode : CompositeFunction
	{
		private const string Keyword = "COUNT";

		/// <inheritDoc/>
		public CountNode(List<ExpressionNode> operands) : base(operands) { }


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountKeyword(parser);
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
