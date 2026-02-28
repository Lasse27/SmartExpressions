using SmartExpressions.Core.Nodes;

namespace SmartExpressions.Core.Utility
{
	public interface IParsable
	{
		ExpressionNode Parse(ParserContext parserContext);
	}
}