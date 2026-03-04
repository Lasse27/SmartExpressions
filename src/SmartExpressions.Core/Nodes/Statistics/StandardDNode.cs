using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Nodes.Statistics
{
	public record StandardDNode : CompositeFunction
	{
		private const string Keyword = "STD";

		/// <inheritDoc/>
		public StandardDNode(List<ExpressionNode> operands) : base(operands) { }


		public static Operation<ExpressionNode> Get(Parser parser)
		{
			Operation<List<ExpressionNode>> operation = ParserHelpers.ParseNCountOperandKeyword(parser);
			if (operation.Status == Status.Failure)
			{
				return Operation<ExpressionNode>.Failure(operation.Message);
			}

			ExpressionNode node = new StandardDNode(operation.Value);
			return Operation<ExpressionNode>.Success(node);
		}

		public override Operation<object> Evaluate(Evaluator evaluator, IProgress<string> listener = default) => throw new NotImplementedException();
	}

	/// <inheritdoc/>
		public override string ToString() => base.ToString();

		/// <inheritdoc/>
		public override string GetKeyword() => Keyword;
	}
