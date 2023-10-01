#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public readonly struct ExecutionResult
    {
        public readonly List<(string text, string stackTrace, LogType type)> logs;
        public readonly object? commandInput;

        public ExecutionResult(List<(string, string, LogType)> logs, object? commandInput)
        {
            this.logs = logs;
            this.commandInput = commandInput;
        }
    }
}