using System.Collections.Concurrent;

using SmartExpressions.Core.Evaluation;
using SmartExpressions.Core.Lexing;
using SmartExpressions.Core.Nodes;
using SmartExpressions.Core.Parsing;
using SmartExpressions.Core.Tokens;
using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Expressions
{
	/// <summary> Represents a configurable and evaluatable expression based on a textual formula. </summary>
	public class Expression
	{
		/// <summary> Gets a value indicating whether the expression has been successfully tokenized and parsed. </summary>
		public bool IsAssembled => this._isTokenized && this._isParsed;

		/// <summary> Gets a value indicating whether the expression should be automatically tokenized and parsed before evaluation. </summary>
		public bool AssembleOnEvaluation { get; private set; }

		/// <summary> Gets the textual formula of the expression. </summary>
		public string Formula { get; private set; }

		/// <summary> Gets the tokens produced from the formula after tokenization. </summary>
		private List<IToken> _tokens;

		/// <summary> Gets the root node obtaining after parsing the tokens. </summary>
		private ExpressionNode _root;

		/// <summary> Gets the parameter dictionary used during expression evaluation. </summary>
		public ConcurrentDictionary<string, object> Bindings { get; private set; }

		/// <summary> Gets a value indicating whether the expression has been successfully tokenized . </summary>
		private bool _isTokenized = false;

		/// <summary> Gets a value indicating whether the expression has been successfully parsed. </summary>
		private bool _isParsed = false;


		#region Constructors

		/// <summary> Initializes a new instance of the <see cref="Expression"/> class with an empty formula. </summary>
		public Expression()
		{
			this.Formula = string.Empty;
			this._tokens = [];
			this.Bindings = [];
			this.AssembleOnEvaluation = true;
			this._isTokenized = false;
			this._isParsed = false;
		}

		/// <summary> Initializes a new instance of the <see cref="Expression"/> class with the specified formula. </summary>
		/// <param name="formula">The textual formula representing the expression.</param>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="formula"/> is <see langword="null"/>.  </exception>
		public Expression(string formula) : this()
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));
			this.Formula = formula;
		}

		#endregion


		/// <summary> Sets the formula of the expression and resets tokenization and parsing state. </summary>
		/// <param name="formula">The new formula of the expression.</param>
		/// <returns>The current <see cref="Expression"/> instance for fluent configuration.</returns>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="formula"/> is <see langword="null"/>. </exception>
		public Expression SetFormula(string formula)
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));

			this._isTokenized = false;
			this._isParsed = false;
			this.Formula = formula;
			return this;
		}


		/// <summary> Binds multiple parameters to the expression. Existing keys are overwritten. </summary>
		/// <param name="parameters">A collection of parameter key-value pairs.</param>
		/// <returns>The current <see cref="Expression"/> instance for fluent configuration.</returns>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="parameters"/> is <see langword="null"/>. </exception>
		public Expression BindParameters(IDictionary<string, object> parameters)
		{
			ArgumentNullException.ThrowIfNull(parameters, nameof(parameters));

			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				_ = this.Bindings.AddOrUpdate(parameter.Key, parameter.Value, (_, _) => parameter.Value);
			}
			return this;
		}


		/// <summary> Binds a single parameter to the expression. Existing keys are overwritten. </summary>
		/// <param name="key">The parameter name.</param>
		/// <param name="value">The parameter value.</param>
		/// <returns>The current <see cref="Expression"/> instance for fluent configuration.</returns>
		/// <exception cref="ArgumentException"> Thrown if <paramref name="key"/> is null, empty, or whitespace. </exception>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="value"/> is <see langword="null"/>. </exception>
		public Expression BindParameter(string key, object value)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
			ArgumentNullException.ThrowIfNull(value, nameof(value));

			_ = this.Bindings.AddOrUpdate(key, value, (_, _) => value);
			return this;
		}


		/// <summary> Tokenizes the current formula into a sequence of <see cref="Token"/> instances. </summary>
		/// <returns> A <see cref="Operation"/> indicating whether tokenization succeeded. </returns>
		public Operation Tokenize()
		{
			if (this._isTokenized)
			{
				return Operation.Success();
			}

			Lexer lexer = new Lexer(this.Formula);
			Operation<List<IToken>> tokenization = lexer.Run();
			if (tokenization.Status != Status.Success)
			{
				return Operation.Failure(tokenization.Message);
			}

			this._tokens = tokenization.Value;
			this._isTokenized = true;
			return Operation.Success();
		}


		/// <summary> Parses the previously generated tokens into an internal expression representation. </summary>
		/// <returns> A <see cref="Operation"/> indicating whether parsing succeeded. </returns>
		public Operation Parse()
		{
			if (!this._isTokenized)
			{
				return Operation.Failure("Expression wasn't tokenized. Remember to call tokenization before parsing expression.");
			}

			if (this._isParsed)
			{
				return Operation.Success();
			}

			Parser parser = new Parser(this._tokens);
			Operation<ExpressionNode> parsing = parser.Run();
			if (parsing.Status != Status.Success)
			{
				return Operation.Failure(parsing.Message);
			}

			// Parse
			this._root = parsing.Value;
			this._isParsed = true;
			return Operation.Success();
		}


		/// <summary> Executes tokenization and parsing in sequence if necessary. </summary>
		/// <returns> A <see cref="Operation"/> indicating whether the expression  was successfully assembled. </returns>
		public Operation Assemble()
		{
			if (!this._isTokenized)
			{
				Operation smartResult = this.Tokenize();
				if (smartResult.Status != Status.Success)
				{
					return smartResult;
				}
			}

			if (!this._isParsed)
			{
				Operation smartResult = this.Parse();
				if (smartResult.Status != Status.Success)
				{
					return smartResult;
				}
			}

			return Operation.Success();
		}

		/// <inheritdoc/>
		/// <remarks> If <see cref="AssembleOnEvaluation"/> is enabled, the expression is tokenized and parsed automatically before evaluation. </remarks>
		public Operation<object> Evaluate(EvaluatorOptions options = default)
		{
			if (this.AssembleOnEvaluation)
			{
				Operation preconfiguration = this.Assemble();
				if (preconfiguration.Status != Status.Success)
				{
					return Operation<object>.Failure(preconfiguration.Message);
				}
			}

			if (!this.IsAssembled)
			{
				return Operation<object>.Failure(
					$"Expression was never assembled. " +
					$"Remember to tokenize and parse the expression if {nameof(this.AssembleOnEvaluation)} is disabled.");
			}
			else
			{
				Evaluator evaluator = new Evaluator(options, this.Bindings);
				return evaluator.Run(this._root);
			}
		}
	}
}