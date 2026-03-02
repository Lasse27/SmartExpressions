using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Comparison
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record LessThanNode : TwoOperandFunction
	{
		private const string Keyword = "LT";

		/// <inheritdoc/>
		public LessThanNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="Operation{T}"/> object containing the parsed node or an error. </returns>
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<DoubleOperand> dualOperand = ParserHelpers.ParseDualOperandKeyword(parser);
			if (dualOperand.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(dualOperand.Message);
			}

			ExpressionNode node = new LessThanNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			Operation<object> rawLeft = this.Left.Evaluate(evaluator, listener);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<decimal> resolvedLeft = EvaluatorHelpers.ResolveDecimal(rawLeft, Keyword);
			if (resolvedLeft.Status == Status.Failure) { return Operation<object>.Failure(resolvedLeft.Message); }

			Operation<object> rawRight = this.Right.Evaluate(evaluator, listener);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Operation<decimal> resolvedRight = EvaluatorHelpers.ResolveDecimal(rawRight, Keyword);
			if (resolvedRight.Status == Status.Failure) { return Operation<object>.Failure(resolvedRight.Message); }

			// Handle as decimals
			bool value = resolvedLeft.Value < resolvedRight.Value;
			listener?.Report($"{this} = {value}");
			return Operation<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
