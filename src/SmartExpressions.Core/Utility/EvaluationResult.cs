using System.Security.Cryptography;
using System.Text;

namespace SmartExpressions.Core.Utility
{
	public readonly record struct EvaluationResult
	{
		private readonly bool _isOk;
		private readonly string _path;
		internal readonly object? _value;
		private readonly string? _message;

		internal EvaluationResult(bool isOk, string? message, string path, object? value)
		{
			this._isOk = isOk;
			this._path = path;
			this._value = value;
			this._message = message;
		}

		public static EvaluationResult Ok(string path, object value)
			=> new EvaluationResult(true, "", path, value);


		public static EvaluationResult Fail(string message)
			=> new EvaluationResult(false, message, "", null);


		public bool IsOk() 
			=> this._isOk;

		public bool IsFail() 
			=> !this._isOk;

		public string GetBranchId()
		{
			byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(this._path));
			return Convert.ToHexString(bytes).ToLowerInvariant();
		}

		public string GetMessage()
		{
			ArgumentNullException.ThrowIfNull(this._message, nameof(this._message));
			return this._message;
		}

		public object GetValue()
		{
			ArgumentNullException.ThrowIfNull(this._value, nameof(this._value));
			return this._value;
		}
	}
}
