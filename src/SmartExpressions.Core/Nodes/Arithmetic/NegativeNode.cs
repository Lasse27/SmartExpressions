using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NegativeNode : ExpressionNode
	{
		private const string Keyword = "NEG";
		public ExpressionNode Operand { get; set; }


		public NegativeNode(ExpressionNode operand)
			=> this.Operand = operand;


		public static Result<ExpressionNode> Get(Parser parser)
		{
			// Skip keyword NEG
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			// Get operand
			Result<ExpressionNode> operand = parser.ParseExpression();
			if (operand.Status == Status.Failure) { return operand; }

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(check.Message);
			}

			// Build and return
			ExpressionNode node = new NegativeNode(operand.Value);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			Result<object> raw = this.Operand.Evaluate(ctx);
			if (raw.Status == Status.Failure)
			{
				return raw;
			}

			Result<double> resolved = EvaluatorHelpers.ResolveDouble(raw, Keyword);
			if (resolved.Status == Status.Failure)
			{
				return Result<object>.Failure(resolved.Message);
			}

			double negatived = resolved.Value * (-1);
			ctx.Listener?.Report($"{this} = {negatived}");
			return Result<object>.Success(negatived);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";
	}
}
