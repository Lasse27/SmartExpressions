using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record MinNode : CompositeFunction
	{
		private const string Keyword = "MIN";


		/// <inheritDoc/>
		public MinNode(List<ExpressionNode> operands) : base(operands) { }

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new MinNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			double min = double.MaxValue;
			for (int i = 0; i < this.Operands.Count; i++)
			{
				ExpressionNode operand = this.Operands[i];
				Operation<object> raw = operand.Evaluate(evaluator);
				Operation<double> dec = EvaluatorHelpers.ResolveDouble(raw, "Min" + i);
				if (dec.Status == Status.Failure)
				{
					return Operation<object>.Failure(dec.Message);
				}
				if (dec.Value < min)
				{
					min = dec.Value;
				}
			}
			return Operation<object>.Success(min);
		}

		/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
