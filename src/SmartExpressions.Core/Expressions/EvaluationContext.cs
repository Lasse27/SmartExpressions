using System.Diagnostics;

namespace SmartExpressions.Core.Expressions
{
	/// <summary>
	/// 
	/// </summary>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public readonly record struct EvaluationContext
	{
		/// <summary> 
		/// Gets or sets the settings of the evaluation. 
		/// </summary>
		public ExpressionSettings Settings { get; }

		/// <summary>
		/// Gets the current branch stack as strings.
		/// </summary>
		public Stack<string> BranchStack { get; }

		/// <summary>Aktueller Pfad als String, z.B. "if/elif#1/if"</summary>
		public string CurrentPath =>
			this.BranchStack.Count == 0 ? string.Empty : string.Join("/", this.BranchStack.Reverse());

		/// <summary> 
		/// Gets or sets the identifiers registered for the evaluation. 
		/// </summary>
		public Dictionary<string, object> Identifiers { get; }

		/// <summary>
		/// Gets or sets the functions registered for the evaluation. 
		/// </summary>
		public Dictionary<string, EvalFunction> Functions { get; }

		/// <summary> 
		/// Gets or sets the progress reporter for the evaluation. 
		/// </summary>
		public IProgress<string>? Listener { get; }


		/// <summary>
		/// Initializes a new instance of <see cref="EvaluationContext"/>.
		/// </summary>
		public EvaluationContext()
		{
			this.Settings = new ExpressionSettings();
			this.Identifiers = [];
			this.BranchStack = new Stack<string>();
			this.Functions = new Dictionary<string, EvalFunction>(StringComparer.OrdinalIgnoreCase);
			this.Listener = null;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="EvaluationContext"/>.
		/// </summary>
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
			this.BranchStack = new Stack<string>();
		}


		private string? GetDebuggerDisplay() => this.ToString();
	}
}
