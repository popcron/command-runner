namespace Popcron.CommandRunner
{
    public interface ICommandRunner
    {
        Result Run(string text);
    }
}