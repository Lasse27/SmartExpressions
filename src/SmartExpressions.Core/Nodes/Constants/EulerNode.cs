using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	public record EulerNode : ExpressionNode
	{
		public static Operation<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new EulerNode());
		}

		public override Operation<object> Evaluate(Evaluator evaluator) 
			=> Operation<object>.Success(Math.E);
	}
}
