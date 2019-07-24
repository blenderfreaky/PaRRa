using System;
using System.Collections.Generic;
using System.Text;

namespace Parra.Common.Parser
{
    public class Token : IToken
    {
        public ITokenType Type { get; }
        public TokenIndex Index { get; }
    }
}
