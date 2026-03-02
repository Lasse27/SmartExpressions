using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Lexing;
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


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			// Skip keyword NEG
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Get operand
			Operation<ExpressionNode> operand = parser.ParseExpression();
			if (operand.Status == Status.Failure) { return operand; }

			// Check for right parenthesis
			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Build and return
			ExpressionNode node = new NegativeNode(operand.Value);
			return Operation<ExpressionNode>.Success(node);
		}

		/// <inheritdoc/>
		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default)
		{
			Operation<object> raw = this.Operand.Evaluate(evaluator, listener);
			if (raw.Status == Status.Failure)
			{
				return raw;
			}

			Operation<decimal> resolved = EvaluatorHelpers.ResolveDecimal(raw, Keyword);
			if (resolved.Status == Status.Failure)
			{
				return Operation<object>.Failure(resolved.Message);
			}

			decimal negatived = resolved.Value * (-1);
			listener?.Report($"{this} = {negatived}");
			return Operation<object>.Success(negatived);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";
	}
}
