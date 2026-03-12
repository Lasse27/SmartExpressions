using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Nodes.Arithmetic;
using SmartExpressions.Core.Nodes.Comparison;
using SmartExpressions.Core.Nodes.Conditional;
using SmartExpressions.Core.Nodes.Constants;
using SmartExpressions.Core.Nodes.Logical;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Test.Parsing
{
	public class ParserTests
	{
		// ------------------------------------------------------------------ //
		//  Helper – Lex + Parse in einem Schritt
		// ------------------------------------------------------------------ //

		private static Result<ExpressionNode> Parse(string input)
		{
			Lexer lexer = new Lexer(input);
			Result<List<Token>> lexResult = lexer.Run();
			Assert.True(lexResult.Status == Status.Success, $"Lexer failed: {lexResult.Message}");

			Parser parser = new Parser(lexResult.Value);
			return parser.Run();
		}

		private static ExpressionNode ParseSuccess(string input)
		{
			Result<ExpressionNode> result = Parse(input);
			Assert.True(result.Status == Status.Success, $"Parser failed: {result.Message}");
			return result.Value;
		}

		private static void ParseFailure(string input)
		{
			Result<ExpressionNode> result = Parse(input);
			Assert.True(result.Status == Status.Failure, "Expected parser failure but got success.");
		}


		// ================================================================== //
		//  Constants & Literals
		// ================================================================== //

		[Fact]
		public void Parse_Numeric_ReturnsNumericNode()
		{
			ExpressionNode node = ParseSuccess("42");
			NumericNode numeric = Assert.IsType<NumericNode>(node);
			Assert.Equal(42, numeric.Value);
		}

		[Fact]
		public void Parse_NumericDecimal_ReturnsNumericNode()
		{
			ExpressionNode node = ParseSuccess("3.14");
			NumericNode numeric = Assert.IsType<NumericNode>(node);
			Assert.Equal(3.14, numeric.Value);
		}

		[Fact]
		public void Parse_True_ReturnsTrueNode()
		{
			ExpressionNode node = ParseSuccess("true");
			_ = Assert.IsType<TrueNode>(node);
		}

		[Fact]
		public void Parse_False_ReturnsFalseNode()
		{
			ExpressionNode node = ParseSuccess("false");
			_ = Assert.IsType<FalseNode>(node);
		}

		[Fact]
		public void Parse_Null_ReturnsNullNode()
		{
			ExpressionNode node = ParseSuccess("null");
			_ = Assert.IsType<NullNode>(node);
		}

		[Fact]
		public void Parse_Pi_ReturnsPiNode()
		{
			ExpressionNode node = ParseSuccess("pi");
			_ = Assert.IsType<PiNode>(node);
		}

		[Fact]
		public void Parse_Euler_ReturnsEulerNode()
		{
			ExpressionNode node = ParseSuccess("e");
			_ = Assert.IsType<EulerNode>(node);
		}

		[Fact]
		public void Parse_Identifier_ReturnsIdentifierNode()
		{
			ExpressionNode node = ParseSuccess("@{myVar}");
			IdentifierNode identifier = Assert.IsType<IdentifierNode>(node);
			Assert.Equal("myVar", identifier.Key);
		}


		// ================================================================== //
		//  Arithmetic
		// ================================================================== //

		[Fact]
		public void Parse_Add_ReturnsAddNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("add(1, 2)");

			AddNode add = Assert.IsType<AddNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(add.Left);
			NumericNode right = Assert.IsType<NumericNode>(add.Right);
			Assert.Equal(1, left.Value);
			Assert.Equal(2, right.Value);
		}

		[Fact]
		public void Parse_Sub_ReturnsSubtractNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("sub(10, 3)");

			SubtractNode sub = Assert.IsType<SubtractNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(sub.Left);
			NumericNode right = Assert.IsType<NumericNode>(sub.Right);
			Assert.Equal(10, left.Value);
			Assert.Equal(3, right.Value);
		}

		[Fact]
		public void Parse_Mult_ReturnsMultiplyNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("mult(4, 5)");

			MultiplyNode mult = Assert.IsType<MultiplyNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(mult.Left);
			NumericNode right = Assert.IsType<NumericNode>(mult.Right);
			Assert.Equal(4, left.Value);
			Assert.Equal(5, right.Value);
		}

		[Fact]
		public void Parse_Div_ReturnsDivideNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("div(9, 3)");

			DivideNode div = Assert.IsType<DivideNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(div.Left);
			NumericNode right = Assert.IsType<NumericNode>(div.Right);
			Assert.Equal(9, left.Value);
			Assert.Equal(3, right.Value);
		}

		[Fact]
		public void Parse_Mod_ReturnsModuloNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("mod(7, 2)");

			ModuloNode mod = Assert.IsType<ModuloNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(mod.Left);
			NumericNode right = Assert.IsType<NumericNode>(mod.Right);
			Assert.Equal(7, left.Value);
			Assert.Equal(2, right.Value);
		}

		[Fact]
		public void Parse_Pow_ReturnsPowerNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("pow(2, 8)");

			PowerNode pow = Assert.IsType<PowerNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(pow.Left);
			NumericNode right = Assert.IsType<NumericNode>(pow.Right);
			Assert.Equal(2, left.Value);
			Assert.Equal(8, right.Value);
		}

		[Fact]
		public void Parse_Root_ReturnsRootNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("root(27, 3)");

			RootNode root = Assert.IsType<RootNode>(node);
			NumericNode left = Assert.IsType<NumericNode>(root.Left);
			NumericNode right = Assert.IsType<NumericNode>(root.Right);
			Assert.Equal(27, left.Value);
			Assert.Equal(3, right.Value);
		}

		[Fact]
		public void Parse_Abs_ReturnsAbsoluteNodeWithCorrectOperand()
		{
			ExpressionNode node = ParseSuccess("abs(5)");

			AbsoluteNode abs = Assert.IsType<AbsoluteNode>(node);
			NumericNode operand = Assert.IsType<NumericNode>(abs.Operand);
			Assert.Equal(5, operand.Value);
		}

		[Fact]
		public void Parse_Neg_ReturnsNegativeNodeWithCorrectOperand()
		{
			ExpressionNode node = ParseSuccess("neg(5)");

			NegativeNode neg = Assert.IsType<NegativeNode>(node);
			NumericNode operand = Assert.IsType<NumericNode>(neg.Operand);
			Assert.Equal(5, operand.Value);
		}

		[Fact]
		public void Parse_Add_Nested_ReturnsCorrectTree()
		{
			// add(mult(2, 3), 4)  →  AddNode { Left: MultNode { 2, 3 }, Right: 4 }
			ExpressionNode node = ParseSuccess("add(mult(2, 3), 4)");

			AddNode add = Assert.IsType<AddNode>(node);

			MultiplyNode mult = Assert.IsType<MultiplyNode>(add.Left);
			Assert.Equal(2, Assert.IsType<NumericNode>(mult.Left).Value);
			Assert.Equal(3, Assert.IsType<NumericNode>(mult.Right).Value);

			Assert.Equal(4, Assert.IsType<NumericNode>(add.Right).Value);
		}


		// ================================================================== //
		//  Comparison
		// ================================================================== //

		[Fact]
		public void Parse_Eq_ReturnsEqualNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("eq(1, 1)");

			EqualNode eq = Assert.IsType<EqualNode>(node);
			Assert.Equal(1, Assert.IsType<NumericNode>(eq.Left).Value);
			Assert.Equal(1, Assert.IsType<NumericNode>(eq.Right).Value);
		}

		[Fact]
		public void Parse_Neq_ReturnsNotEqualNode()
		{
			ExpressionNode node = ParseSuccess("neq(1, 2)");
			_ = Assert.IsType<NotEqualNode>(node);
		}

		[Fact]
		public void Parse_Lt_ReturnsLessThanNode()
		{
			ExpressionNode node = ParseSuccess("lt(1, 2)");
			_ = Assert.IsType<LessThanNode>(node);
		}

		[Fact]
		public void Parse_Gt_ReturnsGreaterThanNode()
		{
			ExpressionNode node = ParseSuccess("gt(2, 1)");
			_ = Assert.IsType<GreaterThanNode>(node);
		}

		[Fact]
		public void Parse_Lte_ReturnsLessThanEqualNode()
		{
			ExpressionNode node = ParseSuccess("lte(1, 1)");
			_ = Assert.IsType<LessThanEqualNode>(node);
		}

		[Fact]
		public void Parse_Gte_ReturnsGreaterThanEqualNode()
		{
			ExpressionNode node = ParseSuccess("gte(2, 2)");
			_ = Assert.IsType<GreaterThanEqualNode>(node);
		}


		// ================================================================== //
		//  Logical
		// ================================================================== //

		[Fact]
		public void Parse_And_ReturnsAndNodeWithCorrectOperands()
		{
			ExpressionNode node = ParseSuccess("and(true, false)");

			AndNode and = Assert.IsType<AndNode>(node);
			Assert.IsType<TrueNode>(and.Left);
			Assert.IsType<FalseNode>(and.Right);
		}

		[Fact]
		public void Parse_Or_ReturnsOrNode()
		{
			ExpressionNode node = ParseSuccess("or(true, false)");
			_ = Assert.IsType<OrNode>(node);
		}

		[Fact]
		public void Parse_Not_ReturnsNotNodeWithCorrectOperand()
		{
			ExpressionNode node = ParseSuccess("not(true)");

			NotNode not = Assert.IsType<NotNode>(node);
			Assert.IsType<TrueNode>(not.Operand);
		}

		[Fact]
		public void Parse_Xor_ReturnsXorNode()
		{
			ExpressionNode node = ParseSuccess("xor(true, false)");
			_ = Assert.IsType<XorNode>(node);
		}

		[Fact]
		public void Parse_Nand_ReturnsNandNode()
		{
			ExpressionNode node = ParseSuccess("nand(true, true)");
			_ = Assert.IsType<NandNode>(node);
		}

		[Fact]
		public void Parse_Nor_ReturnsNorNode()
		{
			ExpressionNode node = ParseSuccess("nor(false, false)");
			_ = Assert.IsType<NorNode>(node);
		}

		[Fact]
		public void Parse_Xnor_ReturnsXnorNode()
		{
			ExpressionNode node = ParseSuccess("xnor(true, true)");
			_ = Assert.IsType<XnorNode>(node);
		}


		// ================================================================== //
		//  Conditional
		// ================================================================== //

		[Fact]
		public void Parse_If_ReturnsIfThenElseNodeWithCorrectBranches()
		{
			// if(true) { 1 } else { 2 }
			ExpressionNode node = ParseSuccess("if(true) { 1 } else { 2 }");

			IfElifElseNode ifNode = Assert.IsType<IfElifElseNode>(node);
			Assert.IsType<TrueNode>(ifNode.Blocks[0].Condition);
			Assert.Equal(1, Assert.IsType<NumericNode>(ifNode.Blocks[0].Expression).Value);
			Assert.Equal(2, Assert.IsType<NumericNode>(ifNode.Else).Value);
		}

		[Fact]
		public void Parse_If_WithNestedCondition_ReturnsCorrectTree()
		{
			// if(eq(1, 1)) { true } else { false }
			ExpressionNode node = ParseSuccess("if(eq(1, 1)) { true } else { false }");

			IfElifElseNode ifNode = Assert.IsType<IfElifElseNode>(node);
			EqualNode eq = Assert.IsType<EqualNode>(ifNode.Blocks[0].Condition);
			Assert.Equal(1, Assert.IsType<NumericNode>(eq.Left).Value);
			Assert.Equal(1, Assert.IsType<NumericNode>(eq.Right).Value);
			Assert.IsType<TrueNode>(ifNode.Blocks[0].Expression);
			Assert.IsType<FalseNode>(ifNode.Else);
		}

		[Fact]
		public void Parse_If_WithNestedArithmeticInBranches_ReturnsCorrectTree()
		{
			// if(true) { add(1, 2) } else { sub(5, 3) }
			ExpressionNode node = ParseSuccess("if(true) { add(1, 2) } else { sub(5, 3) }");

			IfElifElseNode ifNode = Assert.IsType<IfElifElseNode>(node);
			Assert.IsType<TrueNode>(ifNode.Blocks[0].Condition);
			Assert.IsType<AddNode>(ifNode.Blocks[0].Expression);
			Assert.IsType<SubtractNode>(ifNode.Else);
		}


		// ================================================================== //
		//  Failure cases
		// ================================================================== //

		[Fact]
		public void Parse_EmptyInput_ReturnsNullNode()
		{
			// Empty token list → NullNode per Run() guard
			Parser parser = new Parser(new List<Token>());
			Result<ExpressionNode> result = parser.Run();
			Assert.True(result.Status == Status.Success);
			_ = Assert.IsType<NullNode>(result.Value);
		}

		[Fact]
		public void Parse_Add_MissingRParen_ReturnsFailure() => ParseFailure("add(1, 2");

		[Fact]
		public void Parse_Add_MissingComma_ReturnsFailure() => ParseFailure("add(1 2)");

		[Fact]
		public void Parse_Add_MissingBothOperands_ReturnsFailure() => ParseFailure("add()");

		[Fact]
		public void Parse_If_MissingElseBranch_ReturnsFailure() => ParseFailure("if(true) { 1 }");

		[Fact]
		public void Parse_If_MissingCondition_ReturnsFailure() => ParseFailure("if() { 1 } else { 2 }");
	}
}