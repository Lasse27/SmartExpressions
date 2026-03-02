using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record IdentifierNode : ExpressionNode
	{
		public string Key { get; set; }

		public IdentifierNode(string key)
			=> this.Key = key;

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Token current = parser.PeakAtPointer();
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new IdentifierNode(current.Lexeme));
		}

		public override Operation<object> Evaluate(Evaluator evaluator)
		{
			if (evaluator.Bindings.TryGetValue(this.Key, out object value))
			{
				return Operation<object>.Success(value);
			}

			// Not registered => user error
			return Operation<object>.Failure($"Unregistered key '{this.Key}' in expression. Make sure to bind the key before evaluating.");
		}
	}
}
