using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Conditional
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record IfElifElseNode : ExpressionNode
	{
		private const string Keyword = "IF";

		public List<ConditionalBlock> Blocks { get; set; }

		public ExpressionNode Else { get; set; }


		public IfElifElseNode(List<ConditionalBlock> blocks, ExpressionNode @else)
		{
			this.Blocks = blocks;
			this.Else = @else;
		}


		public static Result<ExpressionNode> Get(Parser parser)
		{
			// Conditional blocks
			List<ConditionalBlock> blocks = new List<ConditionalBlock>();
			Result result = ParseConditionalBlocks(parser, blocks);
			if (result.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(result.Message);
			}

			// Else block
			result = parser.Check(TokenType.Keyword);
			if (result.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(result.Message);
			}

			result = parser.Check(TokenType.LBrace);
			if (result.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(result.Message);
			}

			Result<ExpressionNode> @else = parser.ParseExpression();
			if (@else.Status == Status.Failure) { return @else; }

			result = parser.Check(TokenType.RBrace);
			if (result.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(result.Message);
			}

			// Build and return
			ExpressionNode node = new IfElifElseNode(blocks, @else.Value);
			return Result<ExpressionNode>.Success(node);
		}


		private static Result ParseConditionalBlocks(Parser parser, List<ConditionalBlock> blocks)
		{
			/* 
			 * Condition 
			 */

			Result check = parser.Check(TokenType.Keyword);
			if (check.Status == Status.Failure)
			{
				return Result.Failure(check.Message);
			}

			check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Result.Failure(check.Message);
			}

			Result<ExpressionNode> condition = parser.ParseExpression();
			if (condition.Status == Status.Failure)
			{
				return Result.Failure(condition.Message);
			}

			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Result.Failure(check.Message);
			}

			/* 
			 * Expression 
			 */

			check = parser.Check(TokenType.LBrace);
			if (check.Status == Status.Failure)
			{
				return Result.Failure(check.Message);
			}

			Result<ExpressionNode> expression = parser.ParseExpression();
			if (expression.Status == Status.Failure)
			{
				return Result.Failure(expression.Message);
			}

			check = parser.Check(TokenType.RBrace);
			if (check.Status == Status.Failure)
			{
				return Result.Failure(check.Message);
			}

			// Add and check if recursive call needed
			blocks.Add(new ConditionalBlock(condition.Value, expression.Value));
			if (parser.PointerIsAtEnd())
			{
				return Result.Failure($"Unexpected end of input after {expression}. Expected: else/elif");
			}

			Token next = parser.PeakAtPointer();
			return next.Type == TokenType.Keyword && next.Lexeme.Equals("elif", StringComparison.OrdinalIgnoreCase)
				? ParseConditionalBlocks(parser, blocks)
				: Result.Success();
		}


		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			for (int i = 0; i < this.Blocks.Count; i++)
			{
				ConditionalBlock block = this.Blocks[i];
				Result<bool> condition = ExpressionHelpers.ResolveBoolean(block.Condition.Evaluate(ctx));
				if (condition.Status == Status.Failure)
				{
					return Result<object>.Failure(condition.Message);
				}

				if (condition.Value == true)
				{
					return block.Expression.Evaluate(ctx);
				}
			}

			return this.Else.Evaluate(ctx);
		}


		/// <inheritdoc/>
		public override string ToString() => $"IF({this.Blocks}) ELSE({this.Else})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		private new string GetDebuggerDisplay() => this.ToString();
	}
}
