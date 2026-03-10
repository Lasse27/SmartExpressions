using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Expressions
{


	/// <summary> 
	/// Represents a configurable and evaluatable expression based on a textual formula. 
	/// </summary>
	public class Expression
	{
		/// <summary> 
		/// Gets the textual formula of the expression. 
		/// </summary>
		public string Formula { get; private set; }

		/// <summary> 
		/// Gets the settings used when evaluating the expression.
		/// </summary>
		public ExpressionSettings Settings { get; private set; }

		/// <summary> 
		/// Gets the parameter dictionary used during expression evaluation. 
		/// </summary>
		public Dictionary<string, object> Bindings { get; private set; }

		/// <summary> 
		/// Gets or sets the functions registered for the evaluation. 
		/// </summary>
		public Dictionary<string, EvalFunction> Functions { get; set; }



		#region PRIVATE | INTERNAL

		/// <summary> 
		/// Gets a value indicating whether the expression has been successfully tokenized and parsed. 
		/// </summary>
		private bool IsAssembled => this._isTokenized && this._isParsed;

		/// <summary> 
		/// Gets the tokens produced from the formula after tokenization. 
		/// </summary>
		private List<Token> _tokens;

		/// <summary> 
		/// Gets the root node obtaining after parsing the tokens. 
		/// </summary>
		private ExpressionNode? _root;

		/// <summary> 
		/// Gets a value indicating whether the expression has been successfully tokenized . 
		/// </summary>
		private bool _isTokenized;

		/// <summary> 
		/// Gets a value indicating whether the expression has been successfully parsed. 
		/// </summary>
		private bool _isParsed;

		#endregion


		#region Constructors

		/// <summary> 
		/// Initializes a new instance of the <see cref="Expression"/> class. 
		/// </summary>
		public Expression()
		{
			this.Formula = string.Empty;
			this.Settings = new ExpressionSettings();
			this.Bindings = new Dictionary<string, object>(5, StringComparer.OrdinalIgnoreCase); // assume 5 items
			this.Functions = new Dictionary<string, EvalFunction>(5, StringComparer.OrdinalIgnoreCase); // assume 5 items

			this._tokens = [];
			this._root = null;
			this._isTokenized = false;
			this._isParsed = false;
		}

		/// <summary> 
		/// Initializes a new instance of the <see cref="Expression"/> class. 
		/// </summary>
		/// <exception cref="ArgumentNullException"/>
		public Expression(string formula) : this()
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));
			this.Formula = formula;
		}

		/// <summary> 
		/// Initializes a new instance of the <see cref="Expression"/> class. 
		/// </summary>
		/// <exception cref="ArgumentNullException"/>
		public Expression(ExpressionSettings settings) : this()
			=> this.Settings = settings;

		/// <summary> 
		/// Initializes a new instance of the <see cref="Expression"/> class. 
		/// </summary>
		/// <exception cref="ArgumentNullException"/>
		public Expression(string formula, ExpressionSettings settings) : this()
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));

			this.Formula = formula;
			this.Settings = settings;
		}

		#endregion


		/// <summary> 
		/// Sets the formula of the expression and resets tokenization and parsing state. 
		/// </summary>
		/// <returns>
		/// The current <see cref="Expression"/> instance for fluent configuration.
		/// </returns>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="formula"/> is <see langword="null"/>. </exception>
		public Expression SetFormula(string formula)
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));

			this.Formula = formula;
			this._isTokenized = false;
			this._isParsed = false;
			return this;
		}


		#region [ Registration ]

		/// <summary> 
		/// Registers a single parameter to the expression. Existing keys are overwritten. 
		/// </summary>
		/// <returns>
		/// The current <see cref="Expression"/> instance for fluent configuration.
		/// </returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="ArgumentNullException"/>
		public Expression RegisterBinding(string key, object value)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
			ArgumentNullException.ThrowIfNull(value, nameof(value));

			if (this.Bindings.TryGetValue(key, out object? _))
			{
				this.Bindings[key] = value;
			}
			else
			{
				this.Bindings.Add(key, value);
			}
			return this;
		}

		/// <summary> 
		/// Registers multiple parameters to the expression. Existing keys are overwritten. 
		/// </summary>
		/// <returns>
		/// The current <see cref="Expression"/> instance for fluent configuration.
		/// </returns>
		/// <exception cref="ArgumentNullException"/>
		public Expression RegisterBindings(IDictionary<string, object> bindings)
		{
			ArgumentNullException.ThrowIfNull(bindings, nameof(bindings));
			foreach (KeyValuePair<string, object> parameter in bindings)
			{
				if (this.Bindings.TryGetValue(parameter.Key, out object? _))
				{
					this.Bindings[parameter.Key] = parameter.Value;
				}
				else
				{
					this.Bindings.Add(parameter.Key, parameter.Value);
				}
			}
			return this;
		}

		public Expression RegisterFunction(string key, EvalFunction function)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
			ArgumentNullException.ThrowIfNull(function, nameof(function));
			if (this.Functions.TryGetValue(key, out EvalFunction? _))
			{
				this.Functions[key] = function;
			}
			else
			{
				this.Functions.Add(key, function);
			}
			return this;
		}

		public Expression RegisterFunctions(IDictionary<string, EvalFunction> functions)
		{
			ArgumentNullException.ThrowIfNull(functions, nameof(functions));
			foreach (KeyValuePair<string, EvalFunction> func in functions)
			{
				if (this.Functions.TryGetValue(func.Key, out EvalFunction? _))
				{
					this.Functions[func.Key] = func.Value;
				}
				else
				{
					this.Functions.Add(func.Key, func.Value);
				}
			}
			return this;
		}

		#endregion


		#region [ Initialization and Creation ]

		/// <summary> Tokenizes the current formula into a sequence of <see cref="Token"/> instances. </summary>
		/// <returns> A <see cref="Result"/> indicating whether tokenization succeeded. </returns>
		public Result<List<Token>> Tokenize()
		{
			if (this._isTokenized)
			{
				return Result<List<Token>>.Success(this._tokens);
			}

			Lexer lexer = new Lexer(this.Formula);
			Result<List<Token>> tokenization = lexer.Run();
			if (tokenization.Status != Status.Success)
			{
				return tokenization;
			}

			this._tokens = tokenization.Value;
			this._isTokenized = true;
			return tokenization;
		}


		/// <summary> Parses the previously generated tokens into an internal expression representation. </summary>
		/// <returns> A <see cref="Result"/> indicating whether parsing succeeded. </returns>
		public Result<ExpressionNode> Parse()
		{
			if (!this._isTokenized)
			{
				return Result<ExpressionNode>.Failure("Expression wasn't tokenized. Remember to call tokenization before parsing expression.");
			}

			if (this._isParsed)
			{
				return Result<ExpressionNode>.Success(this._root!);
			}

			Parser parser = new Parser(this._tokens);
			Result<ExpressionNode> parsing = parser.Run();
			if (parsing.Status != Status.Success)
			{
				return parsing;
			}

			// Parse
			this._root = parsing.Value;
			this._isParsed = true;
			return parsing;
		}


		/// <summary> Executes tokenization and parsing in sequence if necessary. </summary>
		/// <returns> A <see cref="Result"/> indicating whether the expression  was successfully assembled. </returns>
		public Result Assemble()
		{
			if (!this._isTokenized)
			{
				Result<List<Token>> tokens = this.Tokenize();
				if (tokens.Status != Status.Success)
				{
					return Result.Failure(tokens.Message);
				}
			}

			if (!this._isParsed)
			{
				Result<ExpressionNode> parses = this.Parse();
				if (parses.Status != Status.Success)
				{
					return Result.Failure(parses.Message);
				}
			}

			return Result.Success();
		}

		#endregion


		/// <inheritdoc/>
		public Result<object> Evaluate(IProgress<string>? listener = default)
		{
			if (this.Settings.AssembleOnEvaluation)
			{
				Result preconfiguration = this.Assemble();
				if (preconfiguration.Status != Status.Success)
				{
					return Result<object>.Failure(preconfiguration.Message);
				}
			}

			if (!this.IsAssembled)
			{
				return Result<object>.Failure(
					"Expression was not assembled. " +
					"Remember to tokenize and parse the expression " +
					"if AssembleOnEvaluation is disabled.");
			}
			else
			{
				EvaluationContext ctx = new EvaluationContext(
					settings: this.Settings,
					identifiers: this.Bindings,
					functions: this.Functions,
					listener: listener
				);
				return this._root!.Evaluate(ctx);
			}
		}
	}
}