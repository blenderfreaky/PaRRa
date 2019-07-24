using System;
using System.Collections.Generic;
using System.Text;

namespace Parra.Common.Parser
{
    public class ParseTreeNode : IParseTreeNode
    {
        public IParseTreeNode Parent { get; }
        public ICollection<IParseTreeNode> Children { get; }

        public INodeType NodeType { get; }
        public IRange<TokenIndex> TokenRange { get; }
    }
}
