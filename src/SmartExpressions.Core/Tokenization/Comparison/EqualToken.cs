using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartExpressions.Core.Tokenization;

namespace SmartExpressions.Core.Tokenization.Comparison
{
	public readonly struct EqualToken(int position) : IToken
	{
		public TokenType Type => TokenType.Equal;

		public string Lexeme => "==";

		public int Position => position;

		public object Value => "==";
	}
}
