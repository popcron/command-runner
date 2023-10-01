#nullable enable
using System;

namespace Popcron.CommandRunner
{
    /// <summary>
    /// Annotating a method with this attribute will make it automatically registered into <see cref="Library.Singleton"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public class CommandAttribute : Attribute
    {
        private readonly string path;

        public string Path => path;

        public CommandAttribute(string path)
        {
            this.path = path;
        }
    }
}