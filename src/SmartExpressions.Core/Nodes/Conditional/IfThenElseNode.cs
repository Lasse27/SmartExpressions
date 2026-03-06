using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
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

		public static Result<ExpressionNode> Parse(Parser parser)
		{
			/*
			 * IF
			 */
			Result check = parser.Check(TokenType.IfKeyWord);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			Result<ExpressionNode> condition = parser.ParseExpression();
			if (condition.Status == Status.Failure) { return condition; }

			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			/*
			 * THEN
			 */

			check = parser.Check(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			Result<ExpressionNode> @then = parser.ParseExpression();
			if (@then.Status == Status.Failure) { return @then; }

			check = parser.Check(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			/*
			 * ELSE
			 */

			check = parser.Check(TokenType.ElseKeyword);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			check = parser.Check(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			Result<ExpressionNode> @else = parser.ParseExpression();
			if (@else.Status == Status.Failure) { return @else; }

			check = parser.Check(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			// Build and return
			ExpressionNode node = new IfThenElseNode(condition.Value, then.Value, @else.Value);
			return Result<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			Result<object> result = this.Condition.Evaluate(ctx);
			Result<bool> condition = EvaluatorHelpers.ResolveBoolean(result, "IF");
			if (condition.Status == Status.Failure) { return Result<object>.Failure(condition.Message); }

			// Handle then else
			return condition.Value == true
				? this.Then.Evaluate(ctx)
				: this.Else.Evaluate(ctx);
		}


		/// <inheritdoc/>
		public override string ToString() => $"IF({this.Condition}) THEN({this.Then}) ELSE({this.Else})";
	}
}
