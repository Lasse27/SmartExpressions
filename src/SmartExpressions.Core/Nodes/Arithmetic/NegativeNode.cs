using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Arithmetic
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NegativeNode : ExpressionNode
	{
		private const string Keyword = "NEG";
		public ExpressionNode Operand { get; set; }


		public NegativeNode(ExpressionNode operand)
			=> this.Operand = operand;


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			// Get operand
			NodeResult operand = ParserHelpers.ParseUnaryKeyword(parser);
			if (operand.IsFail()) { return operand; }
			ExpressionNode node = new NegativeNode(operand.GetValue());
			return NodeResult.Ok(node);
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			EvaluationResult raw = this.Operand.Evaluate(ctx);
			if (raw.IsFail()) { return raw; }

			Result<double> resolved = ExpressionHelpers.ResolveNumeric(raw);
			if (resolved.Status == Status.Fail)
			{
				return EvaluationResult.Fail(resolved.Message);
			}

			double negatived = resolved.Value * (-1);
			ctx.Listener?.Report($"{this} = {negatived}");
			return EvaluationResult.Ok(ctx.CurrentPath, negatived);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
