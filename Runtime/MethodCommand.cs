#nullable enable
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Popcron.CommandRunner
{
    public class MethodCommand : IAsyncCommand
    {
        private Action? method;
        private MethodBase? methodBase;
        private Func<Result>? methodWithResult;
        private Func<Task<Result>>? methodWithTaskResult;

        public string Path { get; }

        public MethodCommand(string path, Action method)
        {
            Path = path;
            this.method = method;
        }

        public MethodCommand(string path, MethodBase methodBase)
        {
            Path = path;
            this.methodBase = methodBase;
        }

        public MethodCommand(string path, Func<Result> methodWithResult)
        {
            Path = path;
            this.methodWithResult = methodWithResult;
        }

        public MethodCommand(string path, Func<Task<Result>> methodWithTaskResult)
        {
            Path = path;
            this.methodWithTaskResult = methodWithTaskResult;
        }

        public Result? Run(Context context)
        {
            if (methodWithResult is not null)
            {
                return methodWithResult();
            }
            else if (method is not null)
            {
                method();
                return null;
            }
            else if (methodBase is not null)
            {
                object result = methodBase.Invoke(null, null);
                if (result is Result)
                {
                    return (Result)result;
                }
                else if (result is not null)
                {
                    return new Result(result);
                }
                else
                {
                    return null;
                }
            }
            else if (methodWithTaskResult is not null)
            {
                throw new Exception("Cannot run a method with a task result synchronously. Use RunAsync()");
            }
            else
            {
                throw new Exception("No method was found to run.");
            }
        }

        public async Task<Result?> RunAsync(Context context)
        {
            if (methodWithTaskResult is not null)
            {
                return await methodWithTaskResult();
            }
            else
            {
                return Run(context);
            }
        }
    }
}