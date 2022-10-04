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
            Value = value;
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

    public class Result<T> : Result
    {
        public new T Value { get; private set; }

        public Result(T value) : base(value)
        {
            Value = value;
        }

        public Result(T value, List<string> log) : base(value, log)
        {
            Value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }
    }
}