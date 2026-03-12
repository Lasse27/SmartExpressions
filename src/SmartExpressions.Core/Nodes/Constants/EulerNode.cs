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
		private const string Keyword = "e";


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			parser.AdvancePointer();
			return NodeResult.Ok(new EulerNode());
		}

		public override EvaluationResult Evaluate(EvaluationContext ctx)
			=> EvaluationResult.Ok(ctx.CurrentPath, Math.E);

		/// <inheritdoc/>
		public override string ToString() => Keyword;

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
