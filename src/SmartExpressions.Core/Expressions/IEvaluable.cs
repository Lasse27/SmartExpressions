using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Expressions
{
	public interface IEvaluable
	{
		/// <summary> Evaluates the object and returns the computed result. </summary>
		/// <returns> A <see cref="Operation{T}"/> containing the evaluation result if successful; otherwise, failure information. </returns>
		Operation<object> Evaluate();
	}
}