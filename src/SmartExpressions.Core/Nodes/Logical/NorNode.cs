using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Logical
{
	public record NorNode : ExpressionNode
	{
		public ExpressionNode Left { get; set; }
		public ExpressionNode Right { get; set; }

		public NorNode(ExpressionNode left, ExpressionNode right)
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

			ExpressionNode node = new NorNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			// Left operand
			Operation<object> rawLeft = this.Left.Evaluate(evaluator);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<bool> resolvedLeft = EvaluatorHelpers.ResolveBoolean(rawLeft, "Nor.1");
			if (resolvedLeft.Status == Status.Failure) { return Operation<object>.Failure(resolvedLeft.Message); }


			// Short circuit
			if (resolvedLeft.Value == true)
				return Operation<object>.Success(false);


			// Right operand
			Operation<object> rawRight = this.Left.Evaluate(evaluator);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Operation<bool> resolvedRight = EvaluatorHelpers.ResolveBoolean(rawRight, "Nor.2");
			if (resolvedRight.Status == Status.Failure) { return Operation<object>.Failure(resolvedRight.Message); }


			// Add adn return
			return Operation<object>.Success(!(resolvedLeft.Value || resolvedRight.Value));
		}
	}
}
