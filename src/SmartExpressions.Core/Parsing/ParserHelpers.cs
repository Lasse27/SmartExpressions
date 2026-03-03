using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public static partial class ParserHelpers
	{
		public static Operation<DoubleOperand> ParseDualOperandKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<DoubleOperand>.Failure(check.Message);
			}

			// Get operand
			Operation<ExpressionNode> left = parser.ParseExpression(); // points to next token
			if (left.Status == Status.Failure)
			{
				return Operation<DoubleOperand>.Failure(left.Message);
			}

			// Check for comma
			check = parser.Check(TokenType.Comma);
			if (check.Status == Status.Failure)
			{
				return Operation<DoubleOperand>.Failure(check.Message);
			}

			// Get operand
			Operation<ExpressionNode> right = parser.ParseExpression(); // points to next token
			if (right.Status == Status.Failure)
			{
				return Operation<DoubleOperand>.Failure(right.Message);
			}

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<DoubleOperand>.Failure(check.Message);
			}

			return Operation<DoubleOperand>.Success(new(left.Value, right.Value));
		}

		public static Operation<List<ExpressionNode>> ParseNCountOperandKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Operation check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Operation<List<ExpressionNode>>.Failure(check.Message);
			}

			// Gather all operands separated by commas
			List<ExpressionNode> operands = new List<ExpressionNode>(2);
			Operation operandParsing = ParseAndAddOperands(parser, operands);
			if (operandParsing.Status == Status.Failure)
			{
				return Operation<List<ExpressionNode>>.Failure(check.Message);
			}

			// Check right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Operation<List<ExpressionNode>>.Failure(check.Message);
			}

			return Operation<List<ExpressionNode>>.Success(operands);
		}

		public static Operation ParseAndAddOperands(Parser parser, List<ExpressionNode> operands)
		{
			// Get operand
			Operation<ExpressionNode> expr = parser.ParseExpression();
			if (expr.Status == Status.Failure) { return Operation.Failure(expr.Message); }
			operands.Add(expr.Value); // Parse value

			// Recurse if comma
			Operation check = parser.Check(TokenType.Comma);
			if (check.Status == Status.Success)
			{
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
