using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Trigonometric
{
	public record RadNode : ExpressionNode
	{
		private const string Keyword = "deg";

		public ExpressionNode Operand { get; set; }

		/// <inheritdoc/>
		public RadNode(ExpressionNode operand) => this.Operand = operand;

		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="Result{T}"/> object containing the parsed node or an error. </returns>
		public static Result<ExpressionNode> Get(Parser parser)
		{
			// Get operand
			Result<ExpressionNode> operand = ParserHelpers.ParseUnaryKeyword(parser);
			if (operand.Status == Status.Failure) { return operand; }
			ExpressionNode node = new RadNode(operand.Value);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			Result<double> resolved = ExpressionHelpers.ResolveNumeric(this.Operand.Evaluate(ctx));
			if (resolved.Status == Status.Failure)
			{
				return Result<object>.Failure(resolved.Message);
			}

			double absolute = resolved.Value * Math.PI / 180.0D;
			ctx.Listener?.Report($"{this} = {absolute}");
			return Result<object>.Success(absolute);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
