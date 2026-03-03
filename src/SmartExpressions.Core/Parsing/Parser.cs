using System.Runtime.CompilerServices;

using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Nodes.Arithmetic;
using SmartExpressions.Core.Nodes.Comparison;
using SmartExpressions.Core.Nodes.Conditional;
using SmartExpressions.Core.Nodes.Constants;
using SmartExpressions.Core.Nodes.Logical;
using SmartExpressions.Core.Nodes.Statistics;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public class Parser
	{
		internal readonly List<Token> _input;
		internal ExpressionNode _root;
		internal int _pointer;
		internal int _length;


		public Parser(List<Token> input)
		{
			ArgumentNullException.ThrowIfNull(input);
			this._input = input;
			this._root = null;
			this._length = this._input.Count;
			this._pointer = 0;
		}


		public void Reset()
		{
			this._root = null;
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


		public Operation Check(TokenType expected)
		{
			if (this.PointerIsAtEnd())
			{
				return Operation.Failure($"Unexpected end of parser input. Expected: {expected}.");
			}

			Token current = this.PeakAtPointer();
			if (current.Type != expected)
			{
				return Operation.Failure($"Expected {expected} at index {current.Position} of parser input. Actual: {current.Type}.");
			}

			this.AdvancePointer();

			// Valid check
			return Operation.Success();
		}


		public Operation<ExpressionNode> Run()
		{
			// Guard against stupidity
			if (this._input.Count == 0)
			{
				return Operation<ExpressionNode>.Success(new NullNode());
			}

			this.Reset();
			Operation<ExpressionNode> result = this.ParseExpression();
			return result.Status != Status.Success
				? result
				: Operation<ExpressionNode>.Success(result.Value);
		}

		public Operation<ExpressionNode> ParseExpression()
		{
			if (this.PointerIsAtEnd())
			{
				return Operation<ExpressionNode>.Failure("Unexpected end of input.");
			}

			Token current = this.PeakAtPointer();
			return current.Type switch
			{
				TokenType.Numeric => NumericNode.Get(this),
				TokenType.Identifier => IdentifierNode.Get(this),

				// Conditional keywords
				TokenType.IfKeyWord => IfThenElseNode.Parse(this),

				// Arithmetic keywords
				TokenType.AbsKeyword => AbsoluteNode.Get(this),
				TokenType.AddKeyWord => AddNode.Get(this),
				TokenType.DivKeyWord => DivideNode.Get(this),
				TokenType.ModKeyWord => ModuloNode.Get(this),
				TokenType.MultKeyWord => MultiplyNode.Get(this),
				TokenType.NegKeyWord => NegativeNode.Get(this),
				TokenType.PowerKeyWord => PowerNode.Get(this),
				TokenType.RootKeyWord => RootNode.Get(this),
				TokenType.SubKeyWord => SubtractNode.Get(this),

				// Logical keywords
				TokenType.AndKeyWord => AndNode.Get(this),
				TokenType.OrKeyWord => OrNode.Get(this),
				TokenType.NandKeyWord => NandNode.Get(this),
				TokenType.NorKeyWord => NorNode.Get(this),
				TokenType.NotKeyWord => NotNode.Get(this),
				TokenType.XnorKeyWord => XnorNode.Get(this),
				TokenType.XorKeyWord => XorNode.Get(this),

				// Comparison keywords
				TokenType.EqualKeyWord => EqualNode.Get(this),
				TokenType.NotEqualKeyWord => NotEqualNode.Get(this),
				TokenType.LessThanKeyWord => LessThanNode.Get(this),
				TokenType.LessThanEqualKeyWord => LessThanEqualNode.Get(this),
				TokenType.GreaterThanKeyWord => GreaterThanNode.Get(this),
				TokenType.GreaterThanEqualKeyWord => GreaterThanEqualNode.Get(this),

				// Statistic keywords
				TokenType.SumKeyWord => SumNode.Get(this),
				TokenType.AvgKeyWord => AverageNode.Get(this),
				TokenType.StDKeyWord => StandardDNode.Get(this),
				TokenType.CountKeyWord => CountNode.Get(this),
				TokenType.MinKeyWord => MinNode.Get(this),
				TokenType.MaxKeyWord => MaxNode.Get(this),
				TokenType.RangeKeyWord => RangeNode.Get(this),

				// Constants
				TokenType.NullKeyword => NullNode.Get(this),
				TokenType.TrueKeyword => TrueNode.Get(this),
				TokenType.FalseKeyword => FalseNode.Get(this),
				TokenType.EulerKeyword => EulerNode.Get(this),
				TokenType.PiKeyword => PiNode.Get(this),

				// Default
				_ => Operation<ExpressionNode>.Failure($"Unknown token at position {this._pointer}."),
			};
		}
	}
}
