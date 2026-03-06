using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Expressions;

namespace SmartExpressions.Core.Utility
{
	internal interface IEvaluable
	{
		/// <summary> Evaluates the object and returns the computed result. </summary>
		/// <returns> A <see cref="Result{T}"/> containing the evaluation result if successful; otherwise, failure information. </returns>
		Result<object> Evaluate(EvaluationContext ctx);
	}
}