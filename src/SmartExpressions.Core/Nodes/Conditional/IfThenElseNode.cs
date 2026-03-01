using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Conditional
{
	public record IfThenElseNode : ExpressionNode
	{
		public ExpressionNode Condition { get; set; }
		public ExpressionNode Then { get; set; }
		public ExpressionNode Else { get; set; }

		public IfThenElseNode(ExpressionNode condition, ExpressionNode @then, ExpressionNode @else)
		{
			this.Condition = condition;
			this.Then = then;
			this.Else = @else;
		}

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


		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			Operation<object> result = this.Condition.Evaluate(evaluator);
			Operation<bool> condition = EvaluatorHelpers.ResolveBoolean(result, "IF");
			if (condition.Status == Status.Failure) { return Operation<object>.Failure(condition.Message); }

			// Handle then else
			return condition.Value == true 
				? this.Then.Evaluate(evaluator) 
				: this.Else.Evaluate(evaluator);
		}
	}
}
