using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record AverageNode : CompositeFunction
	{
		private const string Keyword = "AVG";

		/// <inheritDoc/>
		public AverageNode(List<ExpressionNode> operands) : base(operands) { }

		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new AverageNode(operation.Value);
			return Result<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			double sum = 0;
			for (int i = 0; i < this.Operands.Count; i++)
			{
				ExpressionNode operand = this.Operands[i];
				Result<object> raw = operand.Evaluate(ctx);
				Result<double> dec = ExpressionHelpers.ResolveNumeric(raw);
				if (dec.Status == Status.Failure)
				{
					return Result<object>.Failure(dec.Message);
				}
				sum += dec.Value;
			}
			return Result<object>.Success(sum / this.Operands.Count);
		}

		/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
