using System;

namespace Popcron.CommandRunner
{
    public readonly struct CommandInput : IEquatable<CommandInput>
    {
        public readonly string[] pieces;

        public bool IsEmpty => pieces is null || pieces.Length == 0;
        public int Count => pieces.Length;

        public string this[int index] => pieces[index];

        public CommandInput(string[] pieces)
        {
            this.pieces = pieces;
        }

        public CommandInput(string text)
        {
            this.pieces = ClassicParser.Parse(text).pieces;
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return null;
            }
            else 
            {
                return string.Join(' ', pieces);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is CommandInput other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(CommandInput other)
        {
            if (pieces.Length != other.pieces.Length)
            {
                return false;
            }

            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i] != other.pieces[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(string text)
        {
            CommandInput other = text;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(pieces);
        }

        public static implicit operator CommandInput (string text)
        {
            return ClassicParser.Parse(text);
        }
    }
}