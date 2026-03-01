using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Evaluation
{
	public class Evaluator
	{
		public IDictionary<string, object> Bindings { get; }

		public EvaluatorOptions Options { get; }

		public Evaluator(EvaluatorOptions options, IDictionary<string, object> bindings)
		{
			this.Options = options;
			this.Bindings = bindings;
		}


		public Operation<object> Run(ExpressionNode node)
		{
			ArgumentNullException.ThrowIfNull(node);
			return node.Evaluate(this);
		}
	}
}
