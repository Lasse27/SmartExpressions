using System.Collections.Concurrent;

using SmartExpressions.Core.Utility;

namespace SmartExpressions.Core.Expressions
{
	/// <summary> Represents a configurable and evaluatable expression based on a textual formula. </summary>
	public class SmartExpression
	{
		/// <summary> Gets a value indicating whether the expression has been successfully tokenized and parsed. </summary>
		public bool IsAssembled => _isTokenized && _isParsed;

		/// <summary> Gets a value indicating whether the expression should be automatically tokenized and parsed before evaluation. </summary>
		public bool AssembleOnEvaluation { get; private set; }

		/// <summary> Gets the textual formula of the expression. </summary>
		public string Formula { get; private set; }

		/// <summary> Gets the tokens produced from the formula after tokenization. </summary>
		public SmartToken[] Tokens { get; private set; }

		/// <summary> Gets the parameter dictionary used during expression evaluation. </summary>
		public ConcurrentDictionary<string, object> Parameters { get; private set; }

		/// <summary> Gets a value indicating whether the expression has been successfully tokenized . </summary>
		private bool _isTokenized = false;

		/// <summary> Gets a value indicating whether the expression has been successfully parsed. </summary>
		private bool _isParsed = false;


		#region Constructors

		/// <summary> Initializes a new instance of the <see cref="SmartExpression"/> class with an empty formula. </summary>
		public SmartExpression()
		{
			this.Formula = string.Empty;
			this.Tokens = [];
			this.Parameters = [];
			this.AssembleOnEvaluation = true;
			this._isTokenized = false;
			this._isParsed = false;
		}

		/// <summary> Initializes a new instance of the <see cref="SmartExpression"/> class with the specified formula. </summary>
		/// <param name="formula">The textual formula representing the expression.</param>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="formula"/> is <see langword="null"/>.  </exception>
		public SmartExpression(string formula) : this()
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));
			this.Formula = formula;
		}

		#endregion


		/// <summary> Sets the formula of the expression and resets tokenization and parsing state. </summary>
		/// <param name="formula">The new formula of the expression.</param>
		/// <returns>The current <see cref="SmartExpression"/> instance for fluent configuration.</returns>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="formula"/> is <see langword="null"/>. </exception>
		public SmartExpression SetFormula(string formula)
		{
			ArgumentNullException.ThrowIfNull(formula, nameof(formula));

			this._isTokenized = false;
			this._isParsed = false;
			this.Formula = formula;
			return this;
		}


		/// <summary> Binds multiple parameters to the expression. Existing keys are overwritten. </summary>
		/// <param name="parameters">A collection of parameter key-value pairs.</param>
		/// <returns>The current <see cref="SmartExpression"/> instance for fluent configuration.</returns>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="parameters"/> is <see langword="null"/>. </exception>
		public SmartExpression BindParameters(IDictionary<string, object> parameters)
		{
			ArgumentNullException.ThrowIfNull(parameters, nameof(parameters));

			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				_ = this.Parameters.AddOrUpdate(parameter.Key, parameter.Value, (_, _) => parameter.Value);
			}
			return this;
		}


		/// <summary> Binds a single parameter to the expression. Existing keys are overwritten. </summary>
		/// <param name="key">The parameter name.</param>
		/// <param name="value">The parameter value.</param>
		/// <returns>The current <see cref="SmartExpression"/> instance for fluent configuration.</returns>
		/// <exception cref="ArgumentException"> Thrown if <paramref name="key"/> is null, empty, or whitespace. </exception>
		/// <exception cref="ArgumentNullException"> Thrown if <paramref name="value"/> is <see langword="null"/>. </exception>
		public SmartExpression BindParameter(string key, object value)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));
			ArgumentNullException.ThrowIfNull(value, nameof(value));

			_ = this.Parameters.AddOrUpdate(key, value, (_, _) => value);
			return this;
		}


		/// <summary> Tokenizes the current formula into a sequence of <see cref="SmartToken"/> instances. </summary>
		/// <returns> A <see cref="SmartResult"/> indicating whether tokenization succeeded. </returns>
		public SmartResult Tokenize()
		{
			if (this._isTokenized)
				return SmartResult.Ok();

			// Tokenize
			return SmartResult.Ok();
		}


		/// <summary> Parses the previously generated tokens into an internal expression representation. </summary>
		/// <returns> A <see cref="SmartResult"/> indicating whether parsing succeeded. </returns>
		public SmartResult Parse()
		{
			if (!this._isTokenized)
				return SmartResult.Fail("Expression wasn't tokenized. Remember to call tokenization before parsing expression.");

			if (!this._isParsed)
				return SmartResult.Ok();

			// Parse
			return SmartResult.Ok();
		}


		/// <summary> Executes tokenization and parsing in sequence if necessary. </summary>
		/// <returns> A <see cref="SmartResult"/> indicating whether the expression  was successfully assembled. </returns>
		public SmartResult Assemble()
		{
			if (!this._isTokenized)
			{
				SmartResult smartResult = this.Tokenize();
				if (!smartResult.Success)
				{
					return smartResult;
				}
			}
				
			if (!this._isParsed)
			{
				SmartResult smartResult = this.Parse();
				if (!smartResult.Success)
				{
					return smartResult;
				}
			}

			return SmartResult.Ok();
		}


		/// <summary> Evaluates the assembled expression and returns the computed result. </summary>
		/// <returns> A <see cref="SmartResult{T}"/> containing the evaluation result if successful; otherwise, failure information. </returns>
		/// <remarks> If <see cref="AssembleOnEvaluation"/> is enabled, the expression is tokenized and parsed automatically before evaluation. </remarks>
		public SmartResult<object> Evaluate()
		{
			if (this.AssembleOnEvaluation)
			{
				SmartResult preconfiguration = this.Assemble();
				if (!preconfiguration.Success)
				{
					return SmartResult<object>.Fail(preconfiguration.Message, preconfiguration.Exception);
				}
			}

			if (this.IsAssembled == false)
			{
				return SmartResult<object>.Fail(
					$"Expression was never assembled. " +
					$"Remember to tokenize and parse the expression if {nameof(this.AssembleOnEvaluation)} is disabled.");
			}

			return SmartResult<object>.Ok(null);
		}
	}
}