using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record SumNode : ExpressionNode
	{
		private readonly List<ExpressionNode> operands;

		public SumNode(List<ExpressionNode> operands)
			=> this.operands = operands;


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new SumNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			double sum = 0;
			for (int i = 0; i < this.operands.Count; i++)
			{
				ExpressionNode operand = this.operands[i];
				Operation<object> raw = operand.Evaluate(evaluator);
				Operation<double> dec = EvaluatorHelpers.ResolveDouble(raw, "Sum" + i);
				if (dec.Status == Status.Failure)
				{
					return Operation<object>.Failure(dec.Message);
				}
				sum += dec.Value;
			}
			return Operation<object>.Success(sum);
		}
	}
}
