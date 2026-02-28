using SmartExpressions.Core.Parsing.Nodes;
using SmartExpressions.Core.Tokenization;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Arithmetic
{
	public record RootNode : ExpressionNode
	{
		private readonly ExpressionNode operand;

		public RootNode(ExpressionNode operand) => this.operand = operand;

		public override Operation<object> Evaluate() => throw new NotImplementedException();

		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			// Skip keyword ROOT
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
			ExpressionNode node = new RootNode(operand.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
