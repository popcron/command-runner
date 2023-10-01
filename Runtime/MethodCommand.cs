#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Popcron.CommandRunner
{
    public readonly struct MethodCommand : ICommand
    {
        private readonly Action? action;
        private readonly MethodBase? staticMethod;
        private readonly Func<object?>? methodWithResult;
        private readonly string path;

        public ReadOnlySpan<char> Path => path.AsSpan();
        public IEnumerable<Type> Parameters => Array.Empty<Type>();

        public MethodCommand(string path, Action action)
        {
            this.path = path;
            this.action = action;
            this.staticMethod = null;
            this.methodWithResult = null;
        }

        public MethodCommand(string path, MethodBase staticMethod)
        {
            this.path = path;
            this.action = null;
            this.staticMethod = staticMethod;
            this.methodWithResult = null;
        }

        public MethodCommand(string path, Func<object?> methodWithResult)
        {
            this.path = path;
            this.action = null;
            this.staticMethod = null;
            this.methodWithResult = methodWithResult;
        }

        object? ICommand.Run(ExecutionInput input)
        {
            if (action != null)
            {
                action.Invoke();
                return null;
            }
            else if (staticMethod != null)
            {
                return staticMethod.Invoke(null, null);
            }
            else if (methodWithResult != null)
            {
                return methodWithResult.Invoke();
            }
            else
            {
                throw new Exception("No method was found to run.");
            }
        }
    }
}