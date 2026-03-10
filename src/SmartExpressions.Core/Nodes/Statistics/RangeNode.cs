using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record RangeNode : CompositeFunction
	{
		private const string Keyword = "RANGE";


		/// <inheritDoc/>
		public RangeNode(List<ExpressionNode> operands) : base(operands) { }


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new RangeNode(operation.Value);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			double min = double.MaxValue;
			double max = double.MinValue;
			for (int i = 0; i < this.Operands.Count; i++)
			{
				ExpressionNode operand = this.operands[i];
				Result<object> raw = operand.Evaluate(ctx);
				Result<double> dec = ExpressionHelpers.ResolveNumeric(raw);
				if (dec.Status == Status.Failure)
				{
					return Result<object>.Failure(dec.Message);
				}
				if (dec.Value > max)
				{
					max = dec.Value;
				}

				if (dec.Value < min)
				{
					min = dec.Value;
				}
			}
			return Result<object>.Success(max - min);
		}

		/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
