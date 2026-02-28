using SmartExpressions.Core.Parsing.Nodes;
using SmartExpressions.Core.Tokenization;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Conditional
{
	public record IfThenElseNode : ExpressionNode
	{
		private readonly ExpressionNode condition;
		private readonly ExpressionNode then;
		private readonly ExpressionNode @else;

		public IfThenElseNode(ExpressionNode condition, ExpressionNode @then, ExpressionNode @else)
		{
			this.condition = condition;
			this.then = then;
			this.@else = @else;
		}

		public override Operation<object> Evaluate() => throw new NotImplementedException();

		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			/*
			 * IF
			 */

			parser.AdvancePointer(); // Skip If keyword

			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip left paren

			Operation<ExpressionNode> condition = parser.ParseExpression();
			if (condition.Status == Status.Failure) { return condition; }

			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip right paren

			/*
			 * THEN
			 */

			check = parser.CheckCurrent(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip left brace

			Operation<ExpressionNode> @then = parser.ParseExpression();
			if (condition.Status == Status.Failure) { return condition; }

			check = parser.CheckCurrent(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip right brace

			/*
			 * ELSE
			 */

			check = parser.CheckCurrent(TokenType.ElseKeyword);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip If keyword

			check = parser.CheckCurrent(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip left brace

			Operation<ExpressionNode> @else = parser.ParseExpression();
			if (condition.Status == Status.Failure) { return condition; }

			check = parser.CheckCurrent(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer(); // Skip right brace

			// Build and return
			ExpressionNode node = new IfThenElseNode(condition.Value, then.Value, @else.Value);
			return Operation<ExpressionNode>.Success(node);
		}
	}
}
