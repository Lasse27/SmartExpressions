using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Comparison
{
	public record NotEqualNode : ExpressionNode
	{
		public ExpressionNode Left { get; set; }
		public ExpressionNode Right { get; set; }

		public NotEqualNode(ExpressionNode left, ExpressionNode right)
		{
			this.Left = left;
			this.Right = right;
		}

		
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<DualOperand> dualOperand = ParserHelpers.ParseDualOperandKeyword(parser);
			if (dualOperand.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(dualOperand.Message);
			}

			ExpressionNode node = new NotEqualNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			Operation<object> rawLeft = this.Left.Evaluate(evaluator);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<object> rawRight = this.Left.Evaluate(evaluator);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			// Handle nulls
			if (rawLeft.Value == null)
			{
				return rawRight.Value == null
					? Operation<object>.Success(false)
					: Operation<object>.Success(true);
			}
			if (rawRight.Value == null)
			{
				return rawLeft.Value == null
					? Operation<object>.Success(false)
					: Operation<object>.Success(true);
			}

			// Handle as decimals
			return Operation<object>.Success(!rawLeft.Value.Equals(rawRight.Value));
		}
	}
}
