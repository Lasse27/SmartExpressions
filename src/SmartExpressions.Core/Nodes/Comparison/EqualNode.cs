using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Comparison
{

	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record EqualNode : TwoOperandFunction
	{
		private const string Keyword = "EQ";

		/// <inheritdoc/>
		public EqualNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

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

			ExpressionNode node = new EqualNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			Operation<object> rawLeft = this.Left.Evaluate(evaluator, listener);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<object> rawRight = this.Right.Evaluate(evaluator, listener);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			// Handle nulls
			if (rawLeft.Value == null)
			{
				return rawRight.Value == null
					? Operation<object>.Success(true)
					: Operation<object>.Success(false);
			}
			if (rawRight.Value == null)
			{
				return rawLeft.Value == null
					? Operation<object>.Success(true)
					: Operation<object>.Success(false);
			}

			// Handle and progress 
			bool value = rawLeft.Value.Equals(rawRight.Value);
			listener?.Report($"{this} = {value}");
			return Operation<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
