using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record IdentifierNode : ExpressionNode
	{
		public string Key { get; set; }

		/// <inheritdoc/>
		public IdentifierNode(string key) => this.Key = key;


		public static Result<ExpressionNode> Get(Parser parser)
		{
			Token current = parser.PeakAtPointer();
			parser.AdvancePointer();
			return Result<ExpressionNode>.Success(new IdentifierNode(current.Lexeme));
		}

		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			if (ctx.Identifiers.TryGetValue(this.Key, out object? value))
			{
				ctx.Listener?.Report($"{this.Key} = {value}");
				return Result<object>.Success(value);
			}

			// Not registered => user error
			return Result<object>.Failure($"Unregistered key '{this.Key}' in expression. Make sure to bind the key before evaluating.");
		}

		/// <inheritdoc/>
		public override string ToString() => this.Key;

		/// <inheritdoc/>
		public override string GetKeyword() => this.Key;
	}
}
