using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NullNode : ExpressionNode
	{
		public static Result<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Result<ExpressionNode>.Success(new NullNode());
		}

		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(null);

		/// <inheritdoc/>
		public override string ToString() => "NULL";
	}
}