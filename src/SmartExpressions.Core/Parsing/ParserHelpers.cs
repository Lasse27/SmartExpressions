using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public static partial class ParserHelpers
	{
		public static Result<BinaryOperand> ParseDualOperandKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Result<BinaryOperand>.Failure(check.Message);
			}

			// Get operand
			Result<ExpressionNode> left = parser.ParseExpression(); // points to next token
			if (left.Status == Status.Failure)
			{
				return Result<BinaryOperand>.Failure(left.Message);
			}

			// Check for comma
			check = parser.Check(TokenType.Comma);
			if (check.Status == Status.Failure)
			{
				return Result<BinaryOperand>.Failure(check.Message);
			}

			// Get operand
			Result<ExpressionNode> right = parser.ParseExpression(); // points to next token
			if (right.Status == Status.Failure)
			{
				return Result<BinaryOperand>.Failure(right.Message);
			}

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Result<BinaryOperand>.Failure(check.Message);
			}

			return Result<BinaryOperand>.Success(new(left.Value, right.Value));
		}

		public static Result<List<ExpressionNode>> ParseNCountOperandKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Failure)
			{
				return Result<List<ExpressionNode>>.Failure(check.Message);
			}

			// Gather all operands separated by commas
			List<ExpressionNode> operands = new List<ExpressionNode>(2);
			Result operandParsing = ParseAndAddOperands(parser, operands);
			if (operandParsing.Status == Status.Failure)
			{
				return Result<List<ExpressionNode>>.Failure(check.Message);
			}

			// Check right parenthesis
			check = parser.Check(TokenType.RParen);
			if (check.Status == Status.Failure)
			{
				return Result<List<ExpressionNode>>.Failure(check.Message);
			}

			return Result<List<ExpressionNode>>.Success(operands);
		}

		public static Result ParseAndAddOperands(Parser parser, List<ExpressionNode> operands)
		{
			// Get operand
			Result<ExpressionNode> expr = parser.ParseExpression();
			if (expr.Status == Status.Failure) { return Result.Failure(expr.Message); }
			operands.Add(expr.Value); // Parse value

			// Recurse if comma
			Result check = parser.Check(TokenType.Comma);
			if (check.Status == Status.Success)
			{
				// Call recursively
				Result recurseAdd = ParseAndAddOperands(parser, operands);
				if (recurseAdd.Status == Status.Failure)
				{
					return recurseAdd;
				}
			}
			return Result.Success();
		}
	}
}
