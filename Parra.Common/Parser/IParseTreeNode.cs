using System.Collections.Generic;

namespace Parra.Common
{
    public interface IParseTreeNode
    {
        IParseTreeNode Parent { get; }
        ICollection<IParseTreeNode> Children { get; }

        INodeType NodeType { get; }
        IRange<TokenIndex> TokenRange { get; }
    }
}
