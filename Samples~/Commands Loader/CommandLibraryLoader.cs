using UnityEngine.Scripting;
using UnityEngine;
using System;
using System.Reflection;
using Popcron.CommandRunner;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

/// <summary>
/// Makes sure that the singleton library has commands loaded.
/// </summary>
[Preserve]
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class CommandLibraryLoader
{
    private static bool hasLoaded;

    static CommandLibraryLoader()
    {
        TryToLoad();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#if UNITY_EDITOR
    [DidReloadScripts]
#endif
    private static void TryToLoad()
    {
        if (hasLoaded) return;
        hasLoaded = true;

        LoadCommandsIn(Assembly.GetExecutingAssembly());
        LoadCommandsIn(typeof(Library).Assembly);
    }

    private static void LoadCommandsIn(Assembly assembly)
    {
        Library library = Library.Singleton;
        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsAbstract)
            {
                continue;
            }

            if (typeof(IBaseCommand).IsAssignableFrom(type))
            {
                try
                {
                    IBaseCommand prefab = (IBaseCommand)Activator.CreateInstance(type);
                    library.Add(prefab);
                    Debug.LogFormat("Added command {0}", prefab.Path);
                }
                catch (Exception e)
                {
                    if (e is not MissingMethodException)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }
    }
}
