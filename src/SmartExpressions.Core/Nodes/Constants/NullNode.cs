using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Constants
{
	public record NullNode : ExpressionNode
	{
		public override Operation<object> Evaluate() => throw new NotImplementedException();

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new NullNode());
		}
	}
}