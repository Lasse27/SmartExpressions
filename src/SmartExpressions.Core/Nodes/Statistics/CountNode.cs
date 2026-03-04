using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record CountNode : CompositeFunction
	{
		private const string Keyword = "COUNT";

		/// <inheritDoc/>
		public CountNode(List<ExpressionNode> operands) : base(operands) { }


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new CountNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
			=> Operation<object>.Success(this.Operands.Count);

		/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
