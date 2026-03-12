using System.Diagnostics;
using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NumericNode : ExpressionNode
	{
		public double Value { get; private set; }

		public NumericNode() => this.Value = 0;


		public static NodeResult Get(Parser parser)
		{
			Token current = parser.PeakAtPointer();

			NumericNode node = new NumericNode();
			Result set = node.TrySetValue(parser._pointer, current.Lexeme);

			if (set.Status == Status.Fail)
			{
				return NodeResult.Fail(set.Message);
			}
			else
			{
				parser.AdvancePointer();
				return NodeResult.Ok(node);
			}
		}

		private Result TrySetValue(int pointer, string value)
		{
			if (!double.TryParse(value, CultureInfo.InvariantCulture, out double val))
			{
				return Result.Fail($"Unparsable numeric value at token position {pointer}.");
			}

			this.Value = val;

			// Valid
			return Result.Ok();
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
			=> EvaluationResult.Ok(ctx.CurrentPath, this.Value);

		/// <inheritdoc/>
		public override string ToString() => this.Value.ToString(CultureInfo.InvariantCulture);

		/// <inheritdoc/>
		private new string GetDebuggerDisplay() => this.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => this.Value.ToString(CultureInfo.InvariantCulture);
	}
}
