using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record FalseNode : ExpressionNode
	{

		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			parser.AdvancePointer();
			return NodeResult.Ok(new FalseNode());
		}

		public override EvaluationResult Evaluate(EvaluationContext ctx)
			=> EvaluationResult.Ok(ctx.CurrentPath, false);

		/// <inheritdoc/>
		public override string GetKeyword() => bool.FalseString;

		/// <inheritdoc/>
		public override string ToString() => bool.FalseString;
	}
}
