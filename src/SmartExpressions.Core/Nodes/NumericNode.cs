using System.Diagnostics;
using System.Globalization;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NumericNode : ExpressionNode
	{
		public decimal Value { get; private set; }

		public NumericNode() => this.Value = 0;


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Token current = parser.PeakAtPointer();

			NumericNode node = new NumericNode();
			Result set = node.TrySetValue(parser._pointer, current.Lexeme);

			if (set.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(set.Message);
			}
			else
			{
				parser.AdvancePointer();
				return Result<ExpressionNode>.Success(node);
			}
		}

		private Result TrySetValue(int pointer, string value)
		{
			if (!decimal.TryParse(value, CultureInfo.InvariantCulture, out decimal val))
			{
				return Result.Failure($"Unparsable numeric value at token position {pointer}.");
			}

			this.Value = val;

			// Valid
			return Result.Success();
		}

		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(this.Value);

		/// <inheritdoc/>
		public override string ToString() => this.Value.ToString(CultureInfo.InvariantCulture);

		/// <inheritdoc/>
		private new string GetDebuggerDisplay() => this.ToString();
	}
}
