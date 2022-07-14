using System;
using UnityEngine;

namespace Popcron.CommandRunner
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class AutoRegisterCommands
    {
        private static bool doneIt;

        private static void DoIt()
        {
            if (!doneIt)
            {
                doneIt = true;

                foreach (ICommand command in CommandFinder.FindAllCommands())
                {
                    SingletonCommandRunner.Instance.Library.Add(command);
                }

                SingletonCommandRunner.Instance.Run("time");
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnPlay()
        {
            DoIt();
        }

#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnRecompile()
        {
            DoIt();
        }

        static AutoRegisterCommands()
        {
            DoIt();
        }
#endif
    }
}