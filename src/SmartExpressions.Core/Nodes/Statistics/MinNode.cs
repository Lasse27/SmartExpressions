using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record MinNode : CompositeFunction
	{
		private const string Keyword = "MIN";


		/// <inheritDoc/>
		public MinNode(List<ExpressionNode> operands) : base(operands) { }


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountKeyword(parser);
			if (operation.Status == Status.Fail)
			{
				return NodeResult.Fail(operation.Message);
			}

			ExpressionNode node = new MinNode(operation.Value);
			return NodeResult.Ok(node);
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			double min = double.MaxValue;
			for (int i = 0; i < this.Operands.Count; i++)
			{
				ExpressionNode operand = this.Operands[i];
				EvaluationResult raw = operand.Evaluate(ctx);
				Result<double> dec = ExpressionHelpers.ResolveNumeric(raw);
				if (dec.Status == Status.Fail)
				{
					return EvaluationResult.Fail(dec.Message);
				}
				if (dec.Value < min)
				{
					min = dec.Value;
				}
			}
			return EvaluationResult.Ok(ctx.CurrentPath, min);
		}

		/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
