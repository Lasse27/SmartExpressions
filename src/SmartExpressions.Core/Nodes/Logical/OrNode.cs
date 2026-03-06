using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Logical
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record OrNode : TwoOperandFunction
	{
		private const string Keyword = "OR";

		/// <inheritdoc/>
		public OrNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

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

			ExpressionNode node = new OrNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			// Left operand
			Result<object> rawLeft = this.Left.Evaluate(ctx);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Result<bool> resolvedLeft = EvaluatorHelpers.ResolveBoolean(rawLeft, Keyword);
			if (resolvedLeft.Status == Status.Failure) { return Result<object>.Failure(resolvedLeft.Message); }

			// Short circuit
			if (resolvedLeft.Value == true)
			{
				return Result<object>.Success(true);
			}

			// Right operand
			Result<object> rawRight = this.Right.Evaluate(ctx);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Result<bool> resolvedRight = EvaluatorHelpers.ResolveBoolean(rawRight, Keyword);
			if (resolvedRight.Status == Status.Failure) { return Result<object>.Failure(resolvedRight.Message); }

			// OR and return
			bool value = resolvedLeft.Value || resolvedRight.Value;
			ctx.Listener?.Report($"{this} = {value}");
			return Result<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
