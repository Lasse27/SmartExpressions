using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record MinNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public MinNode(List<ExpressionNode> operands)
			=> this.operands = operands;

		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new MinNode(operation.Value);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			double min = double.MaxValue;
			for (int i = 0; i < this.operands.Count; i++)
			{
				ExpressionNode operand = this.operands[i];
				Result<object> raw = operand.Evaluate(ctx);
				Result<double> dec = EvaluatorHelpers.ResolveDouble(raw, "Min" + i);
				if (dec.Status == Status.Failure)
				{
					return Result<object>.Failure(dec.Message);
				}
				if (dec.Value < min)
				{
					min = dec.Value;
				}
			}
			return Result<object>.Success(min);
		}
	}
}
