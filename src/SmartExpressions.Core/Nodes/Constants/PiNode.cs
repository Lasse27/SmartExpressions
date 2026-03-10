using System.Diagnostics;
using System.Globalization;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record PiNode : ExpressionNode
	{
		private const string Keyword = "pi";

		public static Result<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Result<ExpressionNode>.Success(new PiNode());
		}

		public override Result<object> Evaluate(EvaluationContext ctx)
			=> Result<object>.Success(Math.PI);

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => Keyword;
	}
}
