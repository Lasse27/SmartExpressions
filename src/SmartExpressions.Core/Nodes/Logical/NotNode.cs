using System.Diagnostics;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Lexing;
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
		/// <returns> A <see cref="Operation{T}"/> object containing the parsed node or an error. </returns>
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			// Skip keyword ABS
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			// Get operand
			Operation<ExpressionNode> operand = parser.ParseExpression();
			if (operand.Status == Status.Failure) { return operand; }

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(check.Message);
			}

			// Build and return
			ExpressionNode node = new NotNode(operand.Value);
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

			Operation<bool> resolved = EvaluatorHelpers.ResolveBoolean(raw, Keyword);
			if (resolved.Status == Status.Failure)
			{
				return Operation<object>.Failure(resolved.Message);
			}

			// NOT and return
			bool value = !resolved.Value;
			listener?.Report($"{this} = {value}");
			return Operation<object>.Success(value);
		}

		/// <inheritdoc/>
		public override string ToString() => $"{Keyword}({this.Operand})";

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
}
