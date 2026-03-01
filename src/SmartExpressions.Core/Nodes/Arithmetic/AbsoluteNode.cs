using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record AbsoluteNode : ExpressionNode
	{
		public ExpressionNode Operand { get; set; }

		public AbsoluteNode(ExpressionNode operand) => this.Operand = operand;

		public override Operation<object> Evaluate() => throw new NotImplementedException();

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
			ExpressionNode node = new AbsoluteNode(operand.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
