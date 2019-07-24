using System;
using System.Collections.Generic;
using System.Text;

namespace Parra.Common
{
    public interface IRule
    {
        IEnumerable<INodeType> First { get; }
        IEnumerable<IRule> Next { get; }
    }
}
