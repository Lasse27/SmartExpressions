namespace SmartExpressions.Core.Expressions
{
	public readonly struct EvaluationContext
	{
		/// <summary> Gets or sets the settings of the evaluation. </summary>
		public ExpressionSettings Settings { get; }

		/// <summary> Gets or sets the identifiers registered for the evaluation. </summary>
		public Dictionary<string, object> Identifiers { get; }

		/// <summary> Gets or sets the functions registered for the evaluation. </summary>
		public Dictionary<string, EvalFunction> Functions { get; }

		/// <summary> Gets or sets the progress reporter for the evaluation. </summary>
		public IProgress<string>? Listener { get; }


		public EvaluationContext()
		{
			this.Settings = new ExpressionSettings();
			this.Identifiers = [];
			this.Functions = [];
			this.Listener = null;
		}

		public EvaluationContext(
			ExpressionSettings settings,
			Dictionary<string, object> identifiers,
			Dictionary<string, EvalFunction> functions,
			IProgress<string>? listener = default)
		{
			this.Settings = settings;
			this.Identifiers = identifiers;
			this.Functions = functions;
			this.Listener = listener;
		}
	}
}
