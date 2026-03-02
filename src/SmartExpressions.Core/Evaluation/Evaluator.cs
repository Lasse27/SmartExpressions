using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Evaluation
{
	public class Evaluator(IDictionary<string, object> bindings)
	{
		public IDictionary<string, object> Bindings { get; } = bindings;

		public Operation<object> Run(ExpressionNode node)
		{
			ArgumentNullException.ThrowIfNull(node);
			return node.Evaluate(this);
		}
	}
}
