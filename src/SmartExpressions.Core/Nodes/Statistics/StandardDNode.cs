using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record StandardDNode : CompositeFunction
	{
		private const string Keyword = "STD";

		/// <inheritDoc/>
		public StandardDNode(List<ExpressionNode> operands) : base(operands) { }


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new StandardDNode(operation.Value);
			return Result<ExpressionNode>.Success(node);
		}

		public override Result<object> Evaluate(EvaluationContext ctx) => throw new NotImplementedException();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
