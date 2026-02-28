using System.Runtime.CompilerServices;

using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Nodes.Arithmetic;
using SmartExpressions.Core.Nodes.Comparison;
using SmartExpressions.Core.Nodes.Conditional;
using SmartExpressions.Core.Nodes.Logical;
using SmartExpressions.Core.Nodes.Statistics;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Parsing
{
	public class Parser
	{
		internal readonly List<IToken> _input;
		internal ExpressionNode _root;
		internal int _pointer;
		internal int _length;


		public Parser(List<IToken> input)
		{
			ArgumentNullException.ThrowIfNull(input);
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
		public IToken PeakAtPointer() => this._input[this._pointer];


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IToken PeakAtNext() => this._input[this._pointer + 1];


		public Operation CheckCurrent(TokenType expected)
		{
			if (this.PointerIsAtEnd())
			{
				return Operation.Failure($"Unexpected end of parser input. Expected: {expected}.");
			}

			IToken current = this.PeakAtPointer();
			if (current.Type != expected)
			{
				return Operation.Failure($"Expected {expected} at index {current.Position} of parser input. Actual: {current.Type}.");
			}

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

			IToken current = this.PeakAtPointer();
			return current.Type switch
			{
				TokenType.Numeric => this.ParseNumber(),
				TokenType.Identifier => this.ParseIdentifier(),
				TokenType.Constant => this.ParseConstant(),

				// Conditional keywords
				TokenType.IfKeyWord => IfThenElseNode.Parse(this),

				// Arithmetic keywords
				TokenType.AbsKeyword => AbsoluteNode.Parse(this),
				TokenType.AddKeyWord => AddNode.Parse(this),
				TokenType.DivKeyWord => DivideNode.Parse(this),
				TokenType.ModKeyWord => ModuloNode.Parse(this),
				TokenType.MultKeyWord => MultiplyNode.Parse(this),
				TokenType.NegKeyWord => NegativeNode.Parse(this),
				TokenType.PowerKeyWord => PowerNode.Parse(this),
				TokenType.RootKeyWord => RootNode.Parse(this),
				TokenType.SubKeyWord => SubtractNode.Parse(this),

				// Logical keywords
				TokenType.AndKeyWord => AndNode.Parse(this),
				TokenType.NandKeyWord => NandNode.Parse(this),
				TokenType.NorKeyWord => NorNode.Parse(this),
				TokenType.NotKeyWord => NotNode.Parse(this),
				TokenType.XnorKeyWord => XnorNode.Parse(this),
				TokenType.XorKeyWord => XorNode.Parse(this),

				// Comparison keywords
				TokenType.EqualKeyWord => EqualNode.Parse(this),
				TokenType.NotEqualKeyWord => NotEqualNode.Parse(this),
				TokenType.LessThanKeyWord => LessThanNode.Parse(this),
				TokenType.LessThanEqualKeyWord => LessThanEqualNode.Parse(this),
				TokenType.GreaterThanKeyWord => GreaterThanNode.Parse(this),
				TokenType.GreaterThanEqualKeyWord => GreaterThanEqualNode.Parse(this),

				// Statistic keywords
				TokenType.SumKeyWord => SumNode.Parse(this),
				TokenType.AvgKeyWord => AverageNode.Parse(this),
				TokenType.StDKeyWord => StandardDNode.Parse(this),
				TokenType.CountKeyWord => CountNode.Parse(this),
				TokenType.MedianKeyWord => MedianNode.Parse(this),
				TokenType.MinKeyWord => MinNode.Parse(this),
				TokenType.MaxKeyWord => MaxNode.Parse(this),
				TokenType.RangeKeyWord => RangeNode.Parse(this),
			};
		}



		private Operation<ExpressionNode> ParseNumber() => throw new NotImplementedException();

		private Operation<ExpressionNode> ParseConstant() => throw new NotImplementedException();
		private Operation<ExpressionNode> ParseIdentifier() => throw new NotImplementedException();

		private static Operation<ExpressionNode> Fail(Operation op)
			=> Operation<ExpressionNode>.Failure(op.Message);
	}
}
