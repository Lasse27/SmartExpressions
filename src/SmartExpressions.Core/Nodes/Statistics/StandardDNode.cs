using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record StandardDNode : CompositeFunction
	{
		private const string Keyword = "STD";

		/// <inheritDoc/>
		public StandardDNode(List<ExpressionNode> operands) : base(operands) { }


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

			ExpressionNode node = new StandardDNode(operation.Value);
			return NodeResult.Ok(node);
		}

		public override EvaluationResult Evaluate(EvaluationContext ctx) => throw new NotImplementedException();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
