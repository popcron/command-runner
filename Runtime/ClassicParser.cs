using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Popcron.CommandRunner
{
    /// <summary>
    /// Separates text input into pieces by spaces, takes quotes into account.
    /// </summary>
    public readonly struct ClassicParser : IParser
    {
        private readonly static Regex spaceRegex = new Regex(@"[\""].+?[\""]|[^ ]+");
        private readonly static Dictionary<ILibrary, Dictionary<IBaseCommand, int>> commandToParameterCount = new Dictionary<ILibrary, Dictionary<IBaseCommand, int>>();

        private static Dictionary<IBaseCommand, int> GetParameterCounts(ILibrary library)
        {
            if (!commandToParameterCount.TryGetValue(library, out Dictionary<IBaseCommand, int> parameterCounts))
            {
                parameterCounts = new Dictionary<IBaseCommand, int>();
                foreach (IBaseCommand command in library.Commands)
                {
                    List<Type> parameterTypes = new List<Type>();
                    foreach (Type parameterType in command.Parameters)
                    {
                        parameterTypes.Add(parameterType);
                    }

                    parameterCounts.Add(command, parameterTypes.Count);
                }

                commandToParameterCount.Add(library, parameterCounts);
            }

            return parameterCounts;
        }

        bool IParser.TryFindCommand(ReadOnlySpan<char> text, ILibrary library, out IBaseCommand foundCommand, out InputParameters parameters)
        {
            Dictionary<IBaseCommand, int> parameterCounts = GetParameterCounts(library);
            MatchCollection matches = spaceRegex.Matches(text.ToString());
            List<string> textPieces = new List<string>();
            foreach (Match match in matches)
            {
                textPieces.Add(match.Value);
            }

            ReadOnlySpan<char> possiblePathMinusOne;
            ReadOnlySpan<char> possiblePathMinusTwo;
            ReadOnlySpan<char> possiblePathMinusThree;
            ReadOnlySpan<char> possiblePathMinusFour;
            if (textPieces.Count > 0)
            {
                int totalLengthMinusOne = 0;
                int totalLengthMinusTwo = 0;
                int totalLengthMinusThree = 0;
                int totalLengthMinusFour = 0;
                for (int i = 0; i < textPieces.Count - 1; i++)
                {
                    totalLengthMinusOne += textPieces[i].Length;
                }

                for (int i = 0; i < textPieces.Count - 2; i++)
                {
                    totalLengthMinusTwo += textPieces[i].Length;
                }

                for (int i = 0; i < textPieces.Count - 3; i++)
                {
                    totalLengthMinusThree += textPieces[i].Length;
                }

                for (int i = 0; i < textPieces.Count - 4; i++)
                {
                    totalLengthMinusFour += textPieces[i].Length;
                }

                possiblePathMinusOne = text.Slice(0, totalLengthMinusOne);
                possiblePathMinusTwo = text.Slice(0, totalLengthMinusTwo);
                possiblePathMinusThree = text.Slice(0, totalLengthMinusThree);
                possiblePathMinusFour = text.Slice(0, totalLengthMinusFour);
            }
            else
            {
                possiblePathMinusOne = ReadOnlySpan<char>.Empty;
                possiblePathMinusTwo = ReadOnlySpan<char>.Empty;
                possiblePathMinusThree = ReadOnlySpan<char>.Empty;
                possiblePathMinusFour = ReadOnlySpan<char>.Empty;
            }

            //find a command that has a path that starts with the pieces
            foreach (IBaseCommand command in library.Commands)
            {
                int parameterCount = parameterCounts[command];
                if (parameterCount == 0)
                {
                    if (command.Path.SequenceEqual(text))
                    {
                        foundCommand = command;
                        parameters = default;
                        return true;
                    }
                }
                else if (parameterCount == 1)
                {
                    if (command.Path.SequenceEqual(possiblePathMinusOne))
                    {
                        foundCommand = command;
                        parameters = new InputParameters(textPieces.ToArray());
                        return true;
                    }
                }
                else if (parameterCount == 2)
                {
                    if (command.Path.SequenceEqual(possiblePathMinusTwo))
                    {
                        foundCommand = command;
                        parameters = new InputParameters(textPieces.ToArray());
                        return true;
                    }
                }
                else if (parameterCount == 3)
                {
                    if (command.Path.SequenceEqual(possiblePathMinusThree))
                    {
                        foundCommand = command;
                        parameters = new InputParameters(textPieces.ToArray());
                        return true;
                    }
                }
                else if (parameterCount == 4)
                {
                    if (command.Path.SequenceEqual(possiblePathMinusFour))
                    {
                        foundCommand = command;
                        parameters = new InputParameters(textPieces.ToArray());
                        return true;
                    }
                }
            }

            foundCommand = default!;
            parameters = default;
            return false;
        }
    }
}