using SmartExpressions.Core.Expressions;

namespace SmartExpressions.Core.Utility
{
	internal interface IEvaluable
	{
		/// <summary> Evaluates the object and returns the computed result. </summary>
		/// <returns> A <see cref="EvaluationResult"/> containing the evaluation result if successful; otherwise, failure information. </returns>
		EvaluationResult Evaluate(EvaluationContext ctx);
	}
}