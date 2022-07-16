using System;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct TimeCommand : ICommand
    {
        public string Path => "time";

        public Result Run(Context parameters)
        {
            Debug.Log(DateTime.Now.ToString());
            return null;
        }
    }
}