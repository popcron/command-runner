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
                    Singletons.Runner.Library.Add(command);
                }
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