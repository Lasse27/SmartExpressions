using System.Diagnostics;
using System.Globalization;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record PiNode : ExpressionNode
	{
		private const string Keyword = "PI";

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new PiNode());
		}

		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
			=> Operation<object>.Success(Math.PI);

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;

		/// <inheritdoc/>
		public override string ToString() => Keyword;
	}
}
