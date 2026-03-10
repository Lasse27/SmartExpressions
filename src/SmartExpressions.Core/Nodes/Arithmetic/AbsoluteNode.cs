using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record AbsoluteNode : ExpressionNode
	{
		private const string Keyword = "ABS";

		public ExpressionNode Operand { get; set; }

		/// <inheritdoc/>
		public AbsoluteNode(ExpressionNode operand) => this.Operand = operand;

		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="Result{T}"/> object containing the parsed node or an error. </returns>
		public static Result<ExpressionNode> Get(Parser parser)
		{
			// Skip keyword ABS
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			// Get operand
			Result<ExpressionNode> operand = parser.ParseExpression();
			if (operand.Status == Status.Failure) { return operand; }

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			// Build and return
			ExpressionNode node = new AbsoluteNode(operand.Value);
			return Result<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			Result<object> raw = this.Operand.Evaluate(ctx);
			if (raw.Status == Status.Failure)
			{
				return raw;
			}

			Result<double> resolved = ExpressionHelpers.ResolveNumeric(raw);
			if (resolved.Status == Status.Failure)
			{
				return Result<object>.Failure(resolved.Message);
			}

			double absolute = Math.Abs(resolved.Value);
			ctx.Listener?.Report($"{this} = {absolute}");
			return Result<object>.Success(absolute);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";
	}
}
