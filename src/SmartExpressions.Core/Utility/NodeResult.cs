using SmartExpressions.Core.Nodes;

namespace SmartExpressions.Core.Utility
{
	public readonly record struct NodeResult
	{
		private readonly bool _isOk;
		private readonly ExpressionNode? _value;
		private readonly string? _message;

		internal NodeResult(bool isOk, string? message, ExpressionNode? value)
		{
			this._isOk = isOk;
			this._value = value;
			this._message = message;
		}

		internal static NodeResult Ok(ExpressionNode value)
			=> new NodeResult(true, "", value);


		internal static NodeResult Fail(string message)
			=> new NodeResult(false, message, null);


		public bool IsOk()
			=> this._isOk;

		public bool IsFail()
			=> !this._isOk;

		public string GetMessage()
		{
			ArgumentNullException.ThrowIfNull(this._message, nameof(this._message));
			return this._message;
		}

		public ExpressionNode GetValue()
		{
			ArgumentNullException.ThrowIfNull(this._value, nameof(this._value));
			return this._value;
		}
	}
}
