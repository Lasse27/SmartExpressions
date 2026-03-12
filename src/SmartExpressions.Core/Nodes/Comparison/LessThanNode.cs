using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Comparison
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record LessThanNode : BinaryFunction
	{
		private const string Keyword = "LT";

		/// <inheritdoc/>
		public LessThanNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			Result<BinaryOperand> dualOperand = ParserHelpers.ParseBinaryKeyword(parser);
			if (dualOperand.Status == Status.Fail)
			{
				return NodeResult.Fail(dualOperand.Message);
			}

			ExpressionNode node = new LessThanNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return NodeResult.Ok(node);
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			// First operand
			EvaluationResult rawLeft = this.Left.Evaluate(ctx);
			if (rawLeft.IsFail()) { return rawLeft; }
			Result<double> resolvedLeft = ExpressionHelpers.ResolveNumeric(rawLeft);
			if (resolvedLeft.Status == Status.Fail) { return EvaluationResult.Fail(resolvedLeft.Message); }

			// Second operand
			EvaluationResult rawRight = this.Right.Evaluate(ctx);
			if (rawRight.IsFail()) { return rawRight; }
			Result<double> resolvedRight = ExpressionHelpers.ResolveNumeric(rawRight);
			if (resolvedRight.Status == Status.Fail) { return EvaluationResult.Fail(resolvedRight.Message); }

			// Handle as decimals
			bool value = resolvedLeft.Value < resolvedRight.Value;
			ctx.Listener?.Report($"{this} = {value}");
			return EvaluationResult.Ok(ctx.CurrentPath, value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
