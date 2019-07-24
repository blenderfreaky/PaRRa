using Parra;
using Parra.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parra
{
    public class TokenType : ITokenType
    {
        public IEnumerable<IRange<char>> First { get; }
        public IEnumerable<ITokenType> Next { get; }
    }
}
