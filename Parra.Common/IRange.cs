namespace Parra.Common
{
    public interface IRange<T>
    {
        T First { get; }
        T Last { get; }
    }
}
