using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Popcron.CommandRunner
{
    public readonly struct ClassicParser : IParser
    {
        private readonly static Regex spaceRegex = new Regex(@"[\""].+?[\""]|[^ ]+");

        CommandInput IParser.Parse(ReadOnlySpan<char> text)
        {
            return Parse(text);
        }

        public static CommandInput Parse(string text)
        {
            MatchCollection matches = spaceRegex.Matches(text);
            List<string> pieces = new List<string>();
            foreach (Match match in matches)
            {
                pieces.Add(match.Value);
            }

            return new CommandInput(pieces.ToArray());
        }

        public static CommandInput Parse(ReadOnlySpan<char> text)
        {
            return Parse(text.ToString());
        }
    }
}