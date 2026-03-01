using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public static partial class ParserHelpers
	{
		public static Operation<DualOperand> ParseDualOperandKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<DualOperand>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Get operand
			Operation<ExpressionNode> left = parser.ParseExpression(); // points to next token
			if (left.Status == Status.Failure)
			{
				return Operation<DualOperand>.Failure(left.Message);
			}

			// Check for comma
			check = parser.CheckCurrent(TokenType.Comma);
			if (check.Status == Status.Failure)
			{
				return Operation<DualOperand>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Get operand
			Operation<ExpressionNode> right = parser.ParseExpression(); // points to next token
			if (right.Status == Status.Failure)
			{
				return Operation<DualOperand>.Failure(right.Message);
			}

			// Check for right parenthesis
			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<DualOperand>.Failure(check.Message);
			}
			parser.AdvancePointer();

			return Operation<DualOperand>.Success(new(left.Value, right.Value));
		}

		public static Operation<List<ExpressionNode>> ParseNCountOperandKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.CheckCurrent(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<List<ExpressionNode>>.Failure(check.Message);
			}
			parser.AdvancePointer();

			// Gather all operands separated by commas
			List<ExpressionNode> operands = new List<ExpressionNode>(2);
			Operation operandParsing = ParseAndAddOperands(parser, operands);
			if (operandParsing.Status == Status.Failure)
			{
				return Operation<List<ExpressionNode>>.Failure(check.Message);
			}

			// Check right parenthesis
			check = parser.CheckCurrent(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<List<ExpressionNode>>.Failure(check.Message);
			}
			parser.AdvancePointer();

			return Operation<List<ExpressionNode>>.Success(operands);
		}

		public static Operation ParseAndAddOperands(Parser parser, List<ExpressionNode> operands)
		{
			// Get operand
			Operation<ExpressionNode> expr = parser.ParseExpression();
			if (expr.Status == Status.Failure) { return Operation.Failure(expr.Message); }
			operands.Add(expr.Value); // Parse value

			// Recurse if comma
			Operation check = parser.CheckCurrent(TokenType.Comma);
			if (check.Status == Status.Success)
			{
				parser.AdvancePointer(); // Skip comma 

				// Call recursively
				Operation recurseAdd = ParseAndAddOperands(parser, operands);
				if (recurseAdd.Status == Status.Failure)
				{
					return recurseAdd;
				}
			}
			return Operation.Success();
		}
	}
}
