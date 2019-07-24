using System.Collections.Generic;

namespace Parra.Common
{
    public interface ITokenType
    {
        IEnumerable<IRange<char>> First { get; }
        IEnumerable<ITokenType> Next { get; }
    }
}