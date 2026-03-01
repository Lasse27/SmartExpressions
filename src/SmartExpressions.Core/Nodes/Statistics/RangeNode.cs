using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record RangeNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public RangeNode(List<ExpressionNode> operands)
			=> this.operands = operands;


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new RangeNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			decimal min = decimal.MinValue;
			decimal max = decimal.MaxValue;
			for (int i = 0; i < this.operands.Count; i++)
			{
				ExpressionNode operand = this.operands[i];
				Operation<object> raw = operand.Evaluate(evaluator);
				Operation<decimal> dec = EvaluatorHelpers.ResolveDecimal(raw, "Range" + i);
				if (dec.Status == Status.Failure)
				{
					return Operation<object>.Failure(dec.Message);
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
			return Operation<object>.Success(max - min);
		}
	}
}
