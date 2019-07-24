using System.Collections.Generic;

namespace Parra.Common
{
    public interface ITerminal : INodeType
    {
        ITokenType Type { get; }
    }
}