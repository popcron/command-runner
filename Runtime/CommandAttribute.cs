#nullable enable
using System;

namespace Popcron.CommandRunner
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandAttribute : Attribute
    {
        public string Path { get; }

        public CommandAttribute(string path)
        {
            Path = path;
        }
    }
}