#nullable enable
using System;

namespace Popcron.CommandRunner
{
    public interface IParser
    {
        bool TryFindCommand(ReadOnlySpan<char> text, ILibrary library, out IBaseCommand command, out InputParameters parameters);
    }

    public static class IParserFunctions
    {
        public static bool TryFindCommand<T>(this T parser, string? text, ILibrary library, out IBaseCommand command, out InputParameters parameters) where T : IParser
        {
            if (text != null)
            {
                return parser.TryFindCommand(text.AsSpan(), library, out command, out parameters);
            }
            else
            {
                command = default!;
                parameters = default;
                return false;
            }
        }

        public static bool TryFindCommand<T>(this T parser, ReadOnlySpan<char> text, ILibrary library, out IBaseCommand command, out InputParameters parameters) where T : IParser
        {
            return parser.TryFindCommand(text, library, out command, out parameters);
        }
    }
}