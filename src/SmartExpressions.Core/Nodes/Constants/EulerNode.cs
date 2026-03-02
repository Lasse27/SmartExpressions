using System.Diagnostics;
using System.Globalization;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record EulerNode : ExpressionNode
	{
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new EulerNode());
		}

		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
			=> Operation<object>.Success(Math.E);

		/// <inheritdoc/>
		public override string ToString() => Math.E.ToString(CultureInfo.InvariantCulture);
	}
}
