using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record NegativeNode : ExpressionNode
	{
		private readonly ExpressionNode operand;

		public NegativeNode(ExpressionNode operand) => this.operand = operand;

		public override Operation<object> Evaluate() => throw new NotImplementedException();

		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			// Skip keyword NEG
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
			parser.AdvancePointer();

			// Check for right parenthesis
			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Build and return
			ExpressionNode node = new NegativeNode(operand.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
