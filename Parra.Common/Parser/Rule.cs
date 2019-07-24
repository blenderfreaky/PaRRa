using Parra;
using Parra.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parra
{
    public class Rule : IRule
    {
        public IEnumerable<INodeType> First { get; }
        public IEnumerable<IRule> Next { get; }
    }
}
