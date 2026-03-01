using SmartExpressions.Core.Evaluation;

namespace SmartExpressions.Core.Utility
{
	public interface IEvaluable
	{
		/// <summary> Evaluates the object and returns the computed result. </summary>
		/// <returns> A <see cref="Operation{T}"/> containing the evaluation result if successful; otherwise, failure information. </returns>
		Operation<object> Evaluate(Evaluator evaluator);
	}
}