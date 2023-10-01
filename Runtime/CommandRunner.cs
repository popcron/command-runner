#nullable enable
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public class CommandRunner : ICommandRunner
    {
        public static CommandRunner Singleton { get; } = new CommandRunner(Popcron.CommandRunner.Library.Singleton, new ClassicParser());

        private readonly ILibrary library;
        private readonly IParser parser;

        public ILibrary Library => library;

        public CommandRunner(ILibrary library, IParser parser)
        {
            this.library = library;
            this.parser = parser;
        }

        UniTask<ExecutionResult> ICommandRunner.FindAndRunAsync(string text)
        {
            if (parser.TryFindCommand(text, library, out IBaseCommand foundCommand, out InputParameters parameters))
            {
                return ((ICommandRunner)this).RunAsync(foundCommand, parameters);
            }
            else return default;
        }

        async UniTask<ExecutionResult> ICommandRunner.RunAsync<T>(T command, InputParameters parameters)
        {
            if (command is IAsyncCommand)
            {
                List<(string, string, LogType)> logs = new List<(string, string, LogType)>();
                ExecutionFrame commandToCall = new ExecutionFrame(command, library, parameters, logs);
                ExecutionResult result = await commandToCall.CallAsyncCommand();
                commandToCall.Dispose();
                return result;
            }
            else if (command is ICommand)
            {
                List<(string, string, LogType)> logs = new List<(string, string, LogType)>();
                ExecutionFrame commandToCall = new ExecutionFrame(command, library, parameters, logs);
                ExecutionResult result = commandToCall.CallCommand();
                commandToCall.Dispose();
                return result;
            }
            else return default;
        }

        public readonly struct ExecutionFrame : IDisposable
        {
            private readonly IBaseCommand command;
            private readonly ILibrary library;
            private readonly InputParameters parameters;
            private readonly List<(string, string, LogType)> logs;

            public ExecutionFrame(IBaseCommand command, ILibrary library, InputParameters parameters, List<(string, string, LogType)> logs)
            {
                this.command = command;
                this.library = library;
                this.parameters = parameters;
                this.logs = logs;
                Application.logMessageReceivedThreaded += OnLog;
            }

            private void OnLog(string text, string stackTrace, LogType type)
            {
                logs.Add((text, stackTrace, type));
            }

            public ExecutionResult CallCommand()
            {
                if (command is ICommand c)
                {
                    ExecutionInput input = new ExecutionInput(library, parameters);
                    object? output = c.Run(input);
                    return new ExecutionResult(logs, output);
                }
                else throw new Exception("Command is not an ICommand");
            }

            public async UniTask<ExecutionResult> CallAsyncCommand()
            {
                if (command is IAsyncCommand c)
                {
                    ExecutionInput input = new ExecutionInput(library, parameters);
                    object? output = await c.RunAsync(input);
                    return new ExecutionResult(logs, output);
                }
                else throw new Exception("Command is not an IAsyncCommand");
            }

            public void Dispose()
            {
                Application.logMessageReceivedThreaded -= OnLog;
            }
        }
    }
}