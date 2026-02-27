using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartExpressions.Core.Tokens.Comparison
{
	public readonly struct LessEqualToken(int position) : IToken
	{
		public TokenType Type => TokenType.LessEqual;

		public string Lexeme => "<=";

		public int Position => position;

		public object Value => "<=";
	}
}
