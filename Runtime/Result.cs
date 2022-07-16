using System.Collections.Generic;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public class Result
    {
        public List<string> Logs { get; }
        public bool HasLogs { get; }
        public object Value { get; private set; }

        public Result()
        {
            Application.logMessageReceived += LogMessage;
            Logs = new List<string>();
            HasLogs = true;
        }

        public Result(object value)
        {
            this.Value = value;
            HasLogs = false;
        }

        public Result(object value, List<string> log)
        {
            Value = value;
            Logs = log;
            HasLogs = true;
        }

        private void LogMessage(string message, string stackTrace, LogType type)
        {
            Logs.Add(message);
        }

        public void Set(object value)
        {
            Value = value;
            Application.logMessageReceived -= LogMessage;
        }
    }
}