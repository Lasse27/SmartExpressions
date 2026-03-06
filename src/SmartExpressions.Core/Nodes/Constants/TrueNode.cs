using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record TrueNode : ExpressionNode
	{
		public static Result<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Result<ExpressionNode>.Success(new TrueNode());
		}

		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(true);

		/// <inheritdoc/>
		public override string ToString() => bool.TrueString;
	}
}
