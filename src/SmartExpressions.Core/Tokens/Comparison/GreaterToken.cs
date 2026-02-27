using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartExpressions.Core.Tokens.Comparison
{
	public readonly struct GreaterToken(int position) : IToken
	{
		public TokenType Type => TokenType.Greater;

		public string Lexeme => ">";

		public int Position => position;

		public object Value => ">";
	}
}
