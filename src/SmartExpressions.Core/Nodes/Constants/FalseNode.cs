using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record FalseNode : ExpressionNode
	{
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new FalseNode());
		}

		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
			=> Operation<object>.Success(false);

		/// <inheritdoc/>
		public override string ToString() => bool.FalseString;
	}
}
