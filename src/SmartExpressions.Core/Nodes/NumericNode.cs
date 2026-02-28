using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record NumericNode : ExpressionNode
	{
		private decimal _value;

		public NumericNode(string value)
			=> this._value = 0;

		private static Operation TrySetValue(NumericNode node, Parser parser, string value)
		{
			if (!decimal.TryParse(value, out node._value))
			{
				return Operation.Failure($"Unparsable numeric value at token position {parser._pointer}.");
			}

			// Valid
			return Operation.Success();
		}

		public override Operation<object> Evaluate() => throw new NotImplementedException();

	}
}
