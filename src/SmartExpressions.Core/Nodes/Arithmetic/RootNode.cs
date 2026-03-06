using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record RootNode : TwoOperandFunction
	{
		private const string Keyword = "ROOT";

		/// <inheritdoc/>
		public RootNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }

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

			ExpressionNode node = new RootNode(dualOperand.Value.Left, dualOperand.Value.Right);
			return Result<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			Result<object> rawLeft = this.Left.Evaluate(ctx);
			if (rawLeft.Status == Status.Failure) { return rawLeft; }

			Result<double> resolvedLeft = EvaluatorHelpers.ResolveDouble(rawLeft, Keyword);
			if (resolvedLeft.Status == Status.Failure) { return Result<object>.Failure(resolvedLeft.Message); }

			Result<object> rawRight = this.Right.Evaluate(ctx);
			if (rawRight.Status == Status.Failure) { return rawRight; }

			Result<double> resolvedRight = EvaluatorHelpers.ResolveDouble(rawRight, Keyword);
			if (resolvedRight.Status == Status.Failure) { return Result<object>.Failure(resolvedRight.Message); }

			// map to local vars
			double base_ = resolvedLeft.Value;
			double degree = resolvedRight.Value;

			// guards
			if (degree == 0)
			{
				return Result<object>.Failure("Root(base,degree) degree cannot be 0.");
			}
			if (base_ < 0 && degree % 2 == 0)
			{
				if (degree % 2 == 0)
				{
					return Result<object>.Failure("Root(base,degree) Root of a negative base is only defined for odd integer degrees.");
				}

				// Root and return
				double vDoub = Math.Pow(-base_, 1D / degree);
				ctx.Listener?.Report($"{this} = {-vDoub}");
				return Result<object>.Success(-vDoub);
			}

			// Root and return
			double value = Math.Pow(base_, 1D / degree);

			ctx.Listener?.Report($"{this} = {value}");
			return Result<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => base.ToString();
	}
}
