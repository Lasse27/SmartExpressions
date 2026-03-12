using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public static partial class ParserHelpers
	{
		public static NodeResult ParseUnaryKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Fail)
			{
				return NodeResult.Fail(check.Message);
			}

			// Get operand
			NodeResult operand = parser.ParseExpression(); // points to next token
			if (operand.IsFail())
			{
				return NodeResult.Fail(operand.GetMessage());
			}

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			return check.Status == Status.Fail
				? NodeResult.Fail(check.Message)
				: NodeResult.Ok(operand.GetValue());
		}

		public static Result<BinaryOperand> ParseBinaryKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Fail)
			{
				return Result<BinaryOperand>.Fail(check.Message);
			}

			// Get operand
			NodeResult left = parser.ParseExpression(); // points to next token
			if (left.IsFail())
			{
				return Result<BinaryOperand>.Fail(left.GetMessage());
			}

			// Check for comma
			check = parser.Check(TokenType.Comma);
			if (check.Status == Status.Fail)
			{
				return Result<BinaryOperand>.Fail(check.Message);
			}

			// Get operand
			NodeResult right = parser.ParseExpression(); // points to next token
			if (right.IsFail())
			{
				return Result<BinaryOperand>.Fail(right.GetMessage());
			}

			// Check for right parenthesis
			check = parser.Check(TokenType.RParen);
			return check.Status == Status.Fail
				? Result<BinaryOperand>.Fail(check.Message)
				: Result<BinaryOperand>.Ok(new(left.GetValue(), right.GetValue()));
		}

		public static Result<List<ExpressionNode>> ParseNCountKeyword(Parser parser)
		{
			// Skip keyword
			parser.AdvancePointer();

			// Check for left parenthesis
			Result check = parser.Check(TokenType.LParen);
			if (check.Status == Status.Fail)
			{
				return Result<List<ExpressionNode>>.Fail(check.Message);
			}

			// Gather all operands separated by commas
			List<ExpressionNode> operands = new List<ExpressionNode>(2);
			Result operandParsing = ParseAndAddOperands(parser, operands);
			if (operandParsing.Status == Status.Fail)
			{
				return Result<List<ExpressionNode>>.Fail(check.Message);
			}

			// Check right parenthesis
			check = parser.Check(TokenType.RParen);
			return check.Status == Status.Fail
				? Result<List<ExpressionNode>>.Fail(check.Message)
				: Result<List<ExpressionNode>>.Ok(operands);
		}

		public static Result ParseAndAddOperands(Parser parser, List<ExpressionNode> operands)
		{
			// Get operand
			NodeResult expr = parser.ParseExpression();
			if (expr.IsFail()) { return Result.Fail(expr.GetMessage()); }
			operands.Add(expr.GetValue()); // Parse value

			// Recurse if comma
			Result check = parser.Check(TokenType.Comma);
			if (check.Status == Status.Ok)
			{
				// Call recursively
				Result recurseAdd = ParseAndAddOperands(parser, operands);
				if (recurseAdd.Status == Status.Fail)
				{
					return recurseAdd;
				}
			}
			return Result.Ok();
		}
	}
}
