using System;

namespace Popcron.CommandRunner
{
    public interface IParser
    {
        CommandInput Parse(ReadOnlySpan<char> text);
    }
}