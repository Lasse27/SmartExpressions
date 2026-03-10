using System.Collections.Frozen;
using System.Runtime.CompilerServices;

using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Nodes.Arithmetic;
using SmartExpressions.Core.Nodes.Comparison;
using SmartExpressions.Core.Nodes.Conditional;
using SmartExpressions.Core.Nodes.Constants;
using SmartExpressions.Core.Nodes.Logical;
using SmartExpressions.Core.Nodes.Statistics;
using SmartExpressions.Core.Nodes.Trigonometric;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public class Parser
	{
		private static readonly FrozenDictionary<string, Func<Parser, Result<ExpressionNode>>> buildInFactories;

		static Parser()
		{
			Dictionary<string, Func<Parser, Result<ExpressionNode>>> dict = new()
			{
				// Constants
				["e"] = EulerNode.Get,
				["pi"] = PiNode.Get,
				["true"] = TrueNode.Get,
				["false"] = FalseNode.Get,
				["null"] = NullNode.Get,

				// Arithmetic
				["add"] = AddNode.Get,
				["sub"] = SubtractNode.Get,
				["mult"] = MultiplyNode.Get,
				["div"] = DivideNode.Get,
				["abs"] = AbsoluteNode.Get,
				["neg"] = NegativeNode.Get,
				["root"] = RootNode.Get,
				["pow"] = PowerNode.Get,
				["mod"] = ModuloNode.Get,

				// trigonometric
				["sin"] = SinNode.Get,
				["cos"] = CosNode.Get,
				["tan"] = TanNode.Get,
				["sinh"] = SinhNode.Get,
				["cosh"] = CoshNode.Get,
				["tanh"] = TanhNode.Get,
				["asin"] = ASinNode.Get,
				["acos"] = ACosNode.Get,
				["atan"] = ATanNode.Get,

				// Comparison
				["eq"] = EqualNode.Get,
				["neq"] = NotEqualNode.Get,
				["gt"] = GreaterThanNode.Get,
				["gte"] = GreaterThanEqualNode.Get,
				["lt"] = LessThanNode.Get,
				["lte"] = LessThanEqualNode.Get,

				// Conditional
				["if"] = IfThenElseNode.Get,

				// Logical
				["and"] = AndNode.Get,
				["nand"] = NandNode.Get,
				["nor"] = NorNode.Get,
				["not"] = NotNode.Get,
				["or"] = OrNode.Get,
				["xnor"] = XnorNode.Get,
				["xor"] = XorNode.Get,

				// Statistics
				["avg"] = AverageNode.Get,
				["count"] = CountNode.Get,
				["max"] = MaxNode.Get,
				["min"] = MinNode.Get,
				["range"] = RangeNode.Get,
				["std"] = StandardDNode.Get,
				["sum"] = SumNode.Get,
			};
			buildInFactories = dict.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
		}

		internal readonly List<Token> _input;
		internal int _pointer;
		internal int _length;



		public Parser(List<Token> input)
		{
			ArgumentNullException.ThrowIfNull(input);
			this._input = input;
			this._length = this._input.Count;
			this._pointer = 0;
		}


		public void Reset()
		{
			this._length = this._input.Count;
			this._pointer = 0;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AdvancePointer() => this._pointer++;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RevertPointer() => this._pointer--;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool PointerIsAtEnd() => this._pointer >= this._length;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool PointerCanAdvance() => this._pointer + 1 < this._length;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Token PeakAtPointer() => this._input[this._pointer];


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Token PeakAtNext() => this._input[this._pointer + 1];


		public Result Check(TokenType expected)
		{
			if (this.PointerIsAtEnd())
			{
				return Result.Failure($"Unexpected end of parser input. Expected: {expected}.");
			}

			Token current = this.PeakAtPointer();
			if (current.Type != expected)
			{
				return Result.Failure($"Expected {expected} at index {current.Position} of parser input. Actual: {current.Type}.");
			}

			this.AdvancePointer();

			// Valid check
			return Result.Success();
		}


		public Result<ExpressionNode> Run()
		{
			// Guard against stupidity
			if (this._input.Count == 0)
			{
				return Result<ExpressionNode>.Success(new NullNode());
			}

			this.Reset();
			Result<ExpressionNode> result = this.ParseExpression();
			return result.Status != Status.Success
				? result
				: Result<ExpressionNode>.Success(result.Value);
		}

		public Result<ExpressionNode> ParseExpression()
		{
			if (this.PointerIsAtEnd())
			{
				return Result<ExpressionNode>.Failure("Unexpected end of input.");
			}

			Token current = this.PeakAtPointer();
			return current.Type switch
			{
				TokenType.Numeric => NumericNode.Get(this),
				TokenType.Identifier => IdentifierNode.Get(this),
				TokenType.Keyword => this.ResolveKeywordToken(),

				// Default
				_ => Result<ExpressionNode>.Failure($"Unexpected token at token-position {this._pointer}."),
			};
		}

		private Result<ExpressionNode> ResolveKeywordToken()
		{
			Token token = this.PeakAtPointer();

			// function is built in - run respective factory
			if (buildInFactories.TryGetValue(token.Lexeme, out Func<Parser, Result<ExpressionNode>>? value))
			{
				return value.Invoke(this);
			}

			// function is not built in - assume registered and try to resolve that
			return RegisteredFunction.Get(this);
		}
	}
}
