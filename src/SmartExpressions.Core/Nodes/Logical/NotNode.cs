using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Logical
{
	public record NotNode : ExpressionNode
	{
		public ExpressionNode Operand { get; set; }

		public NotNode(ExpressionNode operand) => this.Operand = operand;

		
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			// Skip keyword ABS
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Get operand
			Operation<ExpressionNode> operand = parser.ParseExpression();
			if (operand.Status == Status.Failure) { return operand; }

			// Check for right parenthesis
			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Build and return
			ExpressionNode node = new NotNode(operand.Value);
			return Operation<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			Operation<object> raw = this.Operand.Evaluate(evaluator);
			if (raw.Status == Status.Failure)
			{
				return raw;
			}

			Operation<bool> resolved = EvaluatorHelpers.ResolveBoolean(raw, "Not");
			if (resolved.Status == Status.Failure)
			{
				return Operation<object>.Failure(resolved.Message);
			}

			// Invert and return
			return Operation<object>.Success(!resolved.Value);
		}
	}
}
