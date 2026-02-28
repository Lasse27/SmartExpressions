using SmartExpressions.Core.Parsing.Nodes;
using SmartExpressions.Core.Tokenization;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Arithmetic
{
	public record DivideNode : ExpressionNode
	{
		private readonly ExpressionNode _left;
		private readonly ExpressionNode _right;

		public DivideNode(ExpressionNode left, ExpressionNode right)
		{
			this._left = left;
			this._right = right;
		}

		public override Operation<object> Evaluate() => throw new NotImplementedException();


		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			// Skip keyword DIV
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Get operand
			Operation<ExpressionNode> left = parser.ParseExpression();
			if (left.Status == Status.Failure) { return left; }
			parser.AdvancePointer();

			// Check for comma
			check = parser.CheckCurrent(TokenType.Comma);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Get operand
			Operation<ExpressionNode> right = parser.ParseExpression();
			if (right.Status == Status.Failure) { return right; }
			parser.AdvancePointer();

			// Check for right parenthesis
			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Build and return
			ExpressionNode node = new DivideNode(left.Value, right.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
