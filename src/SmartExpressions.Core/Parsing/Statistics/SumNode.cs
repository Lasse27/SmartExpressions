using SmartExpressions.Core.Parsing.Nodes;
using SmartExpressions.Core.Tokenization;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing.Statistics
{
	public class AverageNode
	{
		private static Operation AddOperandExpressions(Parser parser, List<ExpressionNode> operands)
		{
			// Get operand
			Operation<ExpressionNode> expr = parser.ParseExpression();
			if (expr.Status == Status.Failure) { return Operation.Failure(expr.Message); }
			operands.Add(expr.Value); // Parse value

			// Recurse if comma
			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Success)
			{
				parser.AdvancePointer(); // Skip comma 

				// Call recursively
				Operation recurseAdd = AddOperandExpressions(parser, operands);
				if (recurseAdd.Status == Status.Failure)
				{
					return recurseAdd;
				}
			}
			return Operation.Success();
		}
	}
}
