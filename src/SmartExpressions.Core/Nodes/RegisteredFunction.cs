using SmartExpressions.Core.Expressions;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes
{
	public record RegisteredFunction : ExpressionNode
	{
		private readonly string key;
		private readonly List<ExpressionNode> operands;

		public RegisteredFunction(string key, List<ExpressionNode> value)
		{
			this.key = key;
			this.operands = value;
		}


		/// <summary> Gets the node from the current position of the parser and updates the parser position. </summary>
		/// <param name="parser"> The parser that is checked for the node. </param>
		/// <returns> A <see cref="Result{T}"/> object containing the parsed node or an error. </returns>
		public static Result<ExpressionNode> Get(Parser parser)
		{
			string key = parser.PeakAtPointer().Lexeme;
			Result<List<ExpressionNode>> operation = ParserHelpers.ParseNCountKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Result<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new RegisteredFunction(key, operation.Value);
			return Result<ExpressionNode>.Success(node);
		}


		/// <inheritdoc/>
		public override Result<object> Evaluate(EvaluationContext ctx)
		{
			if (ctx.Functions.TryGetValue(this.key, out EvalFunction? result))
			{
				List<object> objs = new List<object>(this.operands.Count);
				foreach (ExpressionNode op in this.operands)
				{
					Result<object> innerResult = op.Evaluate(ctx);
					if (innerResult.Status == Status.Failure)
					{
						return innerResult;
					}

					objs.Add(innerResult.Value);
				}
				return result.Invoke(objs);
			}
			return Result<object>.Failure($"Can't evaluate unknown function '{this.GetKeyword()}'.");
		}

		/// <summary> Gets the related keyword for the node. </summary>
		/// <returns> A <see cref="string"/> representing the nodes keyword. </returns>
		public override string GetKeyword() => this.key;
	}
}
