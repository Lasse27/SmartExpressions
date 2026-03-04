using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NullNode : ExpressionNode
	{
		private const string Keyword = "NULL";

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new NullNode());
		}

		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
			=> Operation<object>.Success(null);

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => Keyword;
	}
}