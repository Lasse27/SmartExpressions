using System.Diagnostics;
using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record EulerNode : ExpressionNode
	{
		public static Result<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Result<ExpressionNode>.Success(new EulerNode());
		}

		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(Math.E);

		/// <inheritdoc/>
		public override string ToString() => Math.E.ToString(CultureInfo.InvariantCulture);
	}
}
