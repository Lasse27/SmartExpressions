using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	public record RootNode : ExpressionNode
	{
		public ExpressionNode Left { get; set; }
		public ExpressionNode Right { get; set; }

		public RootNode(ExpressionNode left, ExpressionNode right)
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

			ExpressionNode node = new RootNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			Operation<object> rawLeft = this.Left.Evaluate(evaluator);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Operation<decimal> resolvedLeft = EvaluatorHelpers.ResolveDecimal(rawLeft, "Root.1");
			if (resolvedLeft.Status == Status.Failure) { return Operation<object>.Failure(resolvedLeft.Message); }

			Operation<object> rawRight = this.Left.Evaluate(evaluator);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Operation<decimal> resolvedRight = EvaluatorHelpers.ResolveDecimal(rawRight, "Root.2");
			if (resolvedRight.Status == Status.Failure) { return Operation<object>.Failure(resolvedRight.Message); }

			// map to local vars
			decimal base_ = resolvedLeft.Value;
			decimal degree = resolvedRight.Value;

			// guards
			if (degree == 0)
			{
				return Operation<object>.Failure("Root(base,degree) degree cannot be 0.");
			}
			if (base_ < 0 && degree % 2 == 0)
			{
				if (degree % 2 == 0)
				{
					return Operation<object>.Failure("Root(base,degree) Root of a negative base is only defined for odd integer degrees.");
				}

				double result = Math.Pow((double)-base_, (double)(1m / degree));
				return Operation<object>.Success(-(decimal)result);
			}

			// Root adn return
			return Operation<object>.Success((decimal)Math.Pow((double)base_, (double)(1m / degree)));
		}
	}
}
