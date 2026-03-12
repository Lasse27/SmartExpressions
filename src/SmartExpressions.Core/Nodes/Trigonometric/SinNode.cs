using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Trigonometric
{
	public record SinNode : ExpressionNode
	{
		private const string Keyword = "sin";

		public ExpressionNode Operand { get; set; }

		/// <inheritdoc/>
		public SinNode(ExpressionNode operand) => this.Operand = operand;


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			NodeResult operand = ParserHelpers.ParseUnaryKeyword(parser);
			if (operand.IsFail()) { return operand; }
			ExpressionNode node = new SinNode(operand.GetValue());
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

			double absolute = Math.Sin(resolved.Value);
			ctx.Listener?.Report($"{this} = {absolute}");
			return EvaluationResult.Ok(ctx.CurrentPath, absolute);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
