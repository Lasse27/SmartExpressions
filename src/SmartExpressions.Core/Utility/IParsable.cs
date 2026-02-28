using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Parsing.Nodes;

namespace SmartExpressions.Core.Utility
{
	public interface IParsable
	{
		ExpressionNode Parse(ParserContext parserContext);
	}
}