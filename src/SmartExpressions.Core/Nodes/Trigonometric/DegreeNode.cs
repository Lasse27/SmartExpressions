using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Trigonometric
{
	public record DegreeNode : ExpressionNode
	{
		private const string Keyword = "deg";

		public ExpressionNode Operand { get; set; }

		/// <inheritdoc/>
		public DegreeNode(ExpressionNode operand) => this.Operand = operand;


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			NodeResult operand = ParserHelpers.ParseUnaryKeyword(parser);
			if (operand.IsFail()) { return operand; }
			ExpressionNode node = new DegreeNode(operand.GetValue());
			return NodeResult.Ok(node);
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			Result<double> resolved = ExpressionHelpers.ResolveNumeric(this.Operand.Evaluate(ctx));
			if (resolved.Status == Status.Fail)
			{
				return EvaluationResult.Fail(resolved.Message);
			}

			double absolute = resolved.Value * 180.0D / Math.PI;
			ctx.Listener?.Report($"{this} = {absolute}");
			return EvaluationResult.Ok(ctx.CurrentPath, absolute);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
