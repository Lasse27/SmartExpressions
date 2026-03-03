using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record MultiplyNode : TwoOperandFunction
	{
		private const string Keyword = "MULT";

		/// <inheritdoc/>
		public MultiplyNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

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

			ExpressionNode node = new MultiplyNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			Operation<object> rawLeft = this.Left.Evaluate(evaluator, listener);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<double> resolvedLeft = EvaluatorHelpers.ResolveDouble(rawLeft, Keyword);
			if (resolvedLeft.Status == Status.Failure) { return Operation<object>.Failure(resolvedLeft.Message); }

			Operation<object> rawRight = this.Right.Evaluate(evaluator, listener);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Operation<double> resolvedRight = EvaluatorHelpers.ResolveDouble(rawRight, Keyword);
			if (resolvedRight.Status == Status.Failure) { return Operation<object>.Failure(resolvedRight.Message); }

			// Mult and return
			double mult = resolvedLeft.Value * resolvedRight.Value;
			listener?.Report($"{this} = {mult}");
			return Operation<object>.Success(mult);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
