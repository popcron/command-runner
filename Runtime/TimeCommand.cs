using System;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct TimeCommand : ICommand
    {
        public string Path => "time";

        public void Run(Context parameters)
        {
            Debug.Log(DateTime.Now.ToString());
        }
    }
}