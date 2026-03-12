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


		public static NodeResult Get(Parser parser)
		{
			Token current = parser.PeakAtPointer();
			parser.AdvancePointer();
			return NodeResult.Ok(new IdentifierNode(current.Lexeme));
		}

		/// <inheritdoc/>
		public override EvaluationResult Evaluate(EvaluationContext ctx)
		{
			if (ctx.Identifiers.TryGetValue(this.Key, out object? value))
			{
				ctx.Listener?.Report($"{this.Key} = {value}");
				return EvaluationResult.Ok(ctx.CurrentPath, value);
			}

			// Not registered => user error
			return EvaluationResult.Fail($"Unregistered key '{this.Key}' in expression. Make sure to bind the key before evaluating.");
		}

		/// <inheritdoc/>
		public override string ToString() => this.Key;

		/// <inheritdoc/>
		public override string GetKeyword() => this.Key;
	}
}
