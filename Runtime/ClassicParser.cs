using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Popcron.CommandRunner
{
    public readonly struct ClassicParser : IParser
    {
        private readonly static Regex parseRegex = new Regex(@"[\""].+?[\""]|[^ ]+");

        public bool TryParse(string text, out CommandInput result)
        {
            MatchCollection matches = parseRegex.Matches(text);
            List<string> pieces = new List<string>();
            foreach (Match match in matches)
            {
                pieces.Add(match.Value);
            }

            result = new CommandInput(pieces.ToArray());
            return true;
        }
    }
}