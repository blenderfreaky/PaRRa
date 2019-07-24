using System;
using System.Collections.Generic;
using System.Text;

namespace Parra.Common
{
    public interface IToken
    {
        ITokenType Type { get; }
        TokenIndex Index { get; }
    }
}
