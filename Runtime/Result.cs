using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public class Result
    {
        public StringBuilder Log { get; }
        public object Value { get; private set; }

        public Result()
        {
            Application.logMessageReceived += LogMessage;
            Log = new StringBuilder();
        }

        public Result(object value)
        {
            this.Value = value;
        }

        public Result(object value, StringBuilder log)
        {
            Value = value;
            Log = log;
        }

        private void LogMessage(string message, string stackTrace, LogType type)
        {
            Log.AppendLine(message);
        }

        public StringBuilder Set(object value)
        {
            Value = value;
            Application.logMessageReceived -= LogMessage;
            return Log;
        }
    }
}