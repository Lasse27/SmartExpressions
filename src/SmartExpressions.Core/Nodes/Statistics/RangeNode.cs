using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record RangeNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public RangeNode(List<ExpressionNode> operands)
			=> this.operands = operands;


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
			double min = double.MinValue;
			double max = double.MaxValue;
			for (int i = 0; i < this.operands.Count; i++)
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
	}
}
