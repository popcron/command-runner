#nullable enable
using System;
using System.Text;

namespace Popcron.CommandRunner
{
    public readonly struct InputParameters : IEquatable<InputParameters>
    {
        private static readonly StringBuilder builder = new StringBuilder();

        public readonly string?[]? pieces;

        public bool IsEmpty => pieces is null || pieces.Length == 0;
        public int Count => pieces?.Length ?? 0;

        public string? this[int index] => pieces != null ? pieces[index] : null;

        public InputParameters(string?[] pieces)
        {
            this.pieces = pieces;
        }

        public override string? ToString()
        {
            if (IsEmpty)
            {
                return null;
            }
            else 
            {
                builder.Clear();
                if (pieces != null)
                {
                    int length = pieces.Length;
                    for (int i = 0; i < length; i++)
                    {
                        string? p = pieces[i];
                        if (p != null)
                        {
                            builder.Append(p);
                        }
                        else
                        {
                            builder.Append("null");
                        }

                        if (i < length - 1)
                        {
                            builder.Append(" ");
                        }
                    }
                }

                return builder.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is InputParameters other)
            {
                return Equals(other);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(InputParameters other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                if (pieces != null)
                {
                    for (int i = 0; i < pieces.Length; i++)
                    {
                        if (pieces[i] is string s)
                        {
                            hash = hash * 23 + GetTextHash(s);
                        }
                        else
                        {
                            hash = hash * 23;
                        }
                    }

                    int GetTextHash(string text)
                    {
                        int hash = 17;
                        for (int i = 0; i < text.Length; i++)
                        {
                            hash = hash * 23 + text[i].GetHashCode();
                        }

                        return hash;
                    }
                }

                return hash;
            }
        }

        public static InputParameters Create<P1>(P1 p1)
        {
            return new InputParameters(new string?[] { p1?.ToString() });
        }

        public static InputParameters Create<P1, P2>(P1 p1, P2 p2)
        {
            return new InputParameters(new string?[] { p1?.ToString(), p2?.ToString() });
        }

        public static InputParameters Create<P1, P2, P3>(P1 p1, P2 p2, P3 p3)
        {
            return new InputParameters(new string?[] { p1?.ToString(), p2?.ToString(), p3?.ToString() });
        }

        public static InputParameters Create<P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
        {
            return new InputParameters(new string?[] { p1?.ToString(), p2?.ToString(), p3?.ToString(), p4?.ToString() });
        }
    }
}