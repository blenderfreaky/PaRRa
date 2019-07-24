namespace Parra.Common
{
    public interface INonTerminal : INodeType
    {
        IRule Rule { get; }
    }
}