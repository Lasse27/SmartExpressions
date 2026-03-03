using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record AbsoluteNode : ExpressionNode
	{
		private const string Keyword = "ABS";

		public ExpressionNode Operand { get; set; }

		public AbsoluteNode(ExpressionNode operand) => this.Operand = operand;

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			// Skip keyword ABS
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			// Get operand
			Operation<ExpressionNode> operand = parser.ParseExpression();
			if (operand.Status == Status.Failure) { return operand; }

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			// Build and return
			ExpressionNode node = new AbsoluteNode(operand.Value);
			return Operation<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			Operation<object> raw = this.Operand.Evaluate(evaluator, listener);
			if (raw.Status == Status.Failure)
			{
				return raw;
			}

			Operation<double> resolved = EvaluatorHelpers.ResolveDouble(raw, Keyword);
			if (resolved.Status == Status.Failure)
			{
				return Operation<object>.Failure(resolved.Message);
			}

			double absolute = Math.Abs(resolved.Value);
			listener?.Report($"{this} = {absolute}");
			return Operation<object>.Success(absolute);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";
	}
}
