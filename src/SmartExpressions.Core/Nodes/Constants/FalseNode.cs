using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record FalseNode : ExpressionNode
	{
		public static Result<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Result<ExpressionNode>.Success(new FalseNode());
		}

		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(false);

		/// <inheritdoc/>
		public override string ToString() => bool.FalseString;
	}
}
