using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record ModuloNode : ExpressionNode
	{
		public ExpressionNode Left { get; set; }
		public ExpressionNode Right { get; set; }

		public ModuloNode(ExpressionNode left, ExpressionNode right)
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

			ExpressionNode node = new ModuloNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			Operation<object> rawLeft = this.Left.Evaluate(evaluator);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<decimal> resolvedLeft = EvaluatorHelpers.ResolveDecimal(rawLeft, "Mod.1");
			if (resolvedLeft.Status == Status.Failure) { return Operation<object>.Failure(resolvedLeft.Message); }

			Operation<object> rawRight = this.Right.Evaluate(evaluator);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Operation<decimal> resolvedRight = EvaluatorHelpers.ResolveDecimal(rawRight, "Mod.2");
			if (resolvedRight.Status == Status.Failure) { return Operation<object>.Failure(resolvedRight.Message); }

			// Mod adn return
			return Operation<object>.Success(resolvedLeft.Value % resolvedRight.Value);
		}
	}
}
