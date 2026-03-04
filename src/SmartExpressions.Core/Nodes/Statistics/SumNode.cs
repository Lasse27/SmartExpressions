using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record SumNode : CompositeFunction
	{
		private const string Keyword = "SUM";

		/// <inheritDoc/>
		public SumNode(List<ExpressionNode> operands) : base(operands) { }


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
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string>? listener = default)
		{
			double sum = 0;
			for (int i = 0; i < this.Operands.Count; i++)
			{
				ExpressionNode operand = this.Operands[i];
				Operation<object> raw = operand.Evaluate(evaluator);
				Operation<double> dec = EvaluatorHelpers.ResolveDouble(raw, Keyword + i);
				if (dec.Status == Status.Failure)
				{
					return Operation<object>.Failure(dec.Message);
				}
				sum += dec.Value;
			}
			return Operation<object>.Success(sum);
		}

		/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
