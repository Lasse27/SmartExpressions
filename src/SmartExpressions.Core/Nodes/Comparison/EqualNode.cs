using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Comparison
{

	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record EqualNode : BinaryFunction
	{
		private const string Keyword = "EQ";

		/// <inheritdoc/>
		public EqualNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }


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

			ExpressionNode node = new EqualNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return NodeResult.Ok(node);
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			EvaluationResult rawLeft = this.Left.Evaluate(ctx);
			if (rawLeft.IsFail()) { return rawLeft; }

			EvaluationResult rawRight = this.Right.Evaluate(ctx);
			if (rawRight.IsFail()) { return rawRight; }

			// Handle nulls
			if (rawLeft._value == null)
			{
				return rawRight._value == null
					? EvaluationResult.Ok(ctx.CurrentPath, true)
					: EvaluationResult.Ok(ctx.CurrentPath, false);
			}
			if (rawRight._value == null)
			{
				return rawLeft._value == null
					? EvaluationResult.Ok(ctx.CurrentPath, true)
					: EvaluationResult.Ok(ctx.CurrentPath, false);
			}

			// Handle and progress 
			bool value = rawLeft.GetValue().Equals(rawRight.GetValue());
			ctx.Listener?.Report($"{this} = {value}");
			return EvaluationResult.Ok(ctx.CurrentPath, value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
