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

                foreach (IBaseCommand command in CommandFinder.FindAllCommands())
                {
                    SingletonCommandRunner.Instance.Library.Add(command);
                }

                SingletonCommandRunner.Instance.Run("yo");
                SingletonCommandRunner.Instance.Run("help");
                SingletonCommandRunner.Instance.Run("search hel");
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