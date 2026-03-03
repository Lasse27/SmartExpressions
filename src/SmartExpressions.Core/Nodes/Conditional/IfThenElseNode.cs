using SmartExpressions.Core.Evaluation;
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

		public static Operation<ExpressionNode> Parse(Parser parser)
		{
			/*
			 * IF
			 */
			Operation check = parser.Check(TokenType.IfKeyWord);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			Operation<ExpressionNode> condition = parser.ParseExpression();
			if (condition.Status == Status.Failure) { return condition; }

			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			/*
			 * THEN
			 */

			check = parser.Check(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			Operation<ExpressionNode> @then = parser.ParseExpression();
			if (@then.Status == Status.Failure) { return @then; }

			check = parser.Check(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			/*
			 * ELSE
			 */

			check = parser.Check(TokenType.ElseKeyword);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			check = parser.Check(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			Operation<ExpressionNode> @else = parser.ParseExpression();
			if (@else.Status == Status.Failure) { return @else; }

			check = parser.Check(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			// Build and return
			ExpressionNode node = new IfThenElseNode(condition.Value, then.Value, @else.Value);
			return Operation<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string>? listener = default)
		{
			Operation<object> result = this.Condition.Evaluate(evaluator, listener);
			Operation<bool> condition = EvaluatorHelpers.ResolveBoolean(result, "IF");
			if (condition.Status == Status.Failure) { return Operation<object>.Failure(condition.Message); }

			// Handle then else
			return condition.Value == true
				? this.Then.Evaluate(evaluator, listener)
				: this.Else.Evaluate(evaluator, listener);
		}


		/// <inheritdoc/>
		public override string ToString() => $"IF({this.Condition}) THEN({this.Then}) ELSE({this.Else})";
	}
}
