#nullable enable
using System;

namespace Popcron.CommandRunner
{
    /// <summary>
    /// Annotating a type declaration with this attribute will make it available in <see cref="Library.Singleton"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class RegisterIntoSingletonAttribute : Attribute
    {

    }
}