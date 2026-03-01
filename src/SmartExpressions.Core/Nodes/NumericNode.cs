using System.Globalization;

using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record NumericNode : ExpressionNode
	{
		public decimal Value { get; private set; }

		public NumericNode()
			=> this.Value = 0;


		public override Operation<object> Evaluate() => throw new NotImplementedException();


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			IToken current = parser.PeakAtPointer();

			NumericNode node = new NumericNode();
			Operation set = node.TrySetValue(parser._pointer, current.Lexeme);

			if (set.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(set.Message);
			}
			else
			{
				parser.AdvancePointer();
				return Operation<ExpressionNode>.Success(node);
			}
		}

		private Operation TrySetValue(int pointer, string value)
		{
			if (!decimal.TryParse(value, CultureInfo.InvariantCulture, out decimal val))
			{
				return Operation.Failure($"Unparsable numeric value at token position {pointer}.");
			}

			this.Value = val;

			// Valid
			return Operation.Success();
		}
	}
}
