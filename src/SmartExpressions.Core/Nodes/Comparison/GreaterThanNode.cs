using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Comparison
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record GreaterThanNode : TwoOperandFunction
	{
		private const string Keyword = "GT";

		/// <inheritdoc/>
		public GreaterThanNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="Result{T}"/> object containing the parsed node or an error. </returns>
		public static Result<ExpressionNode> Get(Parser parser)
		{
			Result<BinaryOperand> dualOperand = ParserHelpers.ParseDualOperandKeyword(parser);
			if (dualOperand.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(dualOperand.Message);
			}

			ExpressionNode node = new GreaterThanNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			Result<object> rawLeft = this.Left.Evaluate(ctx);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Result<double> resolvedLeft = EvaluatorHelpers.ResolveDouble(rawLeft, Keyword);
			if (resolvedLeft.Status == Status.Failure) { return Result<object>.Failure(resolvedLeft.Message); }

			Result<object> rawRight = this.Right.Evaluate(ctx);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Result<double> resolvedRight = EvaluatorHelpers.ResolveDouble(rawRight, Keyword);
			if (resolvedRight.Status == Status.Failure) { return Result<object>.Failure(resolvedRight.Message); }

			// Handle as decimals
			bool value = resolvedLeft.Value > resolvedRight.Value;
			ctx.Listener?.Report($"{this} = {value}");
			return Result<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
