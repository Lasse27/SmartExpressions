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


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			// Conditional blocks
			List<ConditionalBlock> blocks = new List<ConditionalBlock>();
			Result result = ParseConditionalBlocks(parser, blocks);
			if (result.Status == Status.Fail)
			{
				return NodeResult.Fail(result.Message);
			}

			// Else block
			result = parser.Check(TokenType.Keyword);
			if (result.Status == Status.Fail)
			{
				return NodeResult.Fail(result.Message);
			}

			result = parser.Check(TokenType.LBrace);
			if (result.Status == Status.Fail)
			{
				return NodeResult.Fail(result.Message);
			}

			NodeResult @else = parser.ParseExpression();
			if (@else.IsFail()) { return @else; }

			result = parser.Check(TokenType.RBrace);
			if (result.Status == Status.Fail)
			{
				return NodeResult.Fail(result.Message);
			}

			// Build and return
			ExpressionNode node = new IfElifElseNode(blocks, @else.GetValue());
			return NodeResult.Ok(node);
		}


		private static Result ParseConditionalBlocks(Parser parser, List<ConditionalBlock> blocks)
		{
			/* 
			 * Condition 
			 */

			Result check = parser.Check(TokenType.Keyword);
			if (check.Status == Status.Fail)
			{
				return Result.Fail(check.Message);
			}

			check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Fail)
			{
				return Result.Fail(check.Message);
			}

			NodeResult condition = parser.ParseExpression();
			if (condition.IsFail())
			{
				return Result.Fail(condition.GetMessage());
			}

			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Fail)
			{
				return Result.Fail(check.Message);
			}

			/* 
			 * Expression 
			 */

			check = parser.Check(TokenType.LBrace);
			if (check.Status == Status.Fail)
			{
				return Result.Fail(check.Message);
			}

			NodeResult expression = parser.ParseExpression();
			if (expression.IsFail())
			{
				return Result.Fail(expression.GetMessage());
			}

			check = parser.Check(TokenType.RBrace);
			if (check.Status == Status.Fail)
			{
				return Result.Fail(check.Message);
			}

			// Add and check if recursive call needed
			blocks.Add(new ConditionalBlock(condition.GetValue(), expression.GetValue()));
			if (parser.PointerIsAtEnd())
			{
				return Result.Fail($"Unexpected end of input after {expression}. Expected: else/elif");
			}

			Token next = parser.PeakAtPointer();
			return next.Type == TokenType.Keyword && next.Lexeme.Equals("elif", StringComparison.OrdinalIgnoreCase)
				? ParseConditionalBlocks(parser, blocks)
				: Result.Ok();
		}


		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			for (int i = 0; i < this.Blocks.Count; i++)
			{
				ConditionalBlock block = this.Blocks[i];
				Result<bool> condition = ExpressionHelpers.ResolveBoolean(block.Condition.Evaluate(ctx));
				if (condition.Status == Status.Fail)
				{
					return EvaluationResult.Fail(condition.Message);
				}

				if (condition.Value == true)
				{
					ctx.BranchStack.Push("if#" + i);
					return block.Expression.Evaluate(ctx);
				}
			}

			ctx.BranchStack.Push("else");
			return this.Else.Evaluate(ctx);
		}


		/// <inheritdoc/>
		public override string ToString() => $"IF({this.Blocks}) ELSE({this.Else})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		private new string GetDebuggerDisplay() => this.ToString();
	}
}
