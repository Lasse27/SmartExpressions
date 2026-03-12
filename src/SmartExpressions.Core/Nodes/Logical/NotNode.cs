using System.Diagnostics;

using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Logical
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public record NotNode : ExpressionNode
	{
		private const string Keyword = "NOT";

		public ExpressionNode Operand { get; set; }

		/// <inheritdoc/>
		public NotNode(ExpressionNode operand) => this.Operand = operand;


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="NodeResult"/> object containing the parsed node or an error. </returns>
		public static NodeResult Get(Parser parser)
		{
			NodeResult operand = ParserHelpers.ParseUnaryKeyword(parser);
			if (operand.IsFail()) { return operand; }
			ExpressionNode node = new NotNode(operand.GetValue());
			return NodeResult.Ok(node);
		}


		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			EvaluationResult raw = this.Operand.Evaluate(ctx);
			if (raw.IsFail()) { return raw; }

			Result<bool> resolved = ExpressionHelpers.ResolveBoolean(raw);
			if (resolved.Status == Status.Fail)
			{
				return EvaluationResult.Fail(resolved.Message);
			}

			// NOT and return
			bool value = !resolved.Value;
			ctx.Listener?.Report($"{this} = {value}");
			return EvaluationResult.Ok(ctx.CurrentPath, value);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
