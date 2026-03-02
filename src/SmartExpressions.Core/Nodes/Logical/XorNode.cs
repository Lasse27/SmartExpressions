using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Logical
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record XorNode : TwoOperandFunction
	{
		private const string Keyword = "XOR";

		/// <inheritdoc/>
		public XorNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

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

			ExpressionNode node = new XorNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			// Left operand
			Operation<object> rawLeft = this.Left.Evaluate(evaluator, listener);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<bool> resolvedLeft = EvaluatorHelpers.ResolveBoolean(rawLeft, Keyword);
			if (resolvedLeft.Status == Status.Failure) { return Operation<object>.Failure(resolvedLeft.Message); }

			// Right operand
			Operation<object> rawRight = this.Right.Evaluate(evaluator, listener);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Operation<bool> resolvedRight = EvaluatorHelpers.ResolveBoolean(rawRight, Keyword);
			if (resolvedRight.Status == Status.Failure) { return Operation<object>.Failure(resolvedRight.Message); }

			// XOR and return
			bool value = (resolvedLeft.Value && !resolvedRight.Value) || (!resolvedLeft.Value && resolvedRight.Value);
			listener?.Report($"{this} = {value}");
			return Operation<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
