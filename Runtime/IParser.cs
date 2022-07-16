namespace Popcron.CommandRunner
{
    public interface IParser
    {
        bool TryParse(string text, out CommandInput result);
    }
}