using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record IdentifierNode : ExpressionNode
	{
		public string Key { get; set; }

		public IdentifierNode(string key) 
			=> this.Key = key;

		public override Operation<object> Evaluate() => throw new NotImplementedException();

		public static Operation<ExpressionNode> Get(Parser parser)
		{
			IToken current = parser.PeakAtPointer();
			parser.AdvancePointer();
			return Operation<ExpressionNode>.Success(new IdentifierNode(current.Lexeme));
		}
	}
}
