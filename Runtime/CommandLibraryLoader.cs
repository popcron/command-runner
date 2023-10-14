#nullable enable
using UnityEngine;
using System;
using Popcron.CommandRunner;
using System.Runtime.Serialization;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif

namespace Popcron.CommandLoader
{
    /// <summary>
    /// Makes sure that the singleton library has commands loaded.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class CommandLibraryLoader
    {
        private static bool hasLoaded;

        static CommandLibraryLoader()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += () =>
            {
                TryToLoad();
            };
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#if UNITY_EDITOR
        [DidReloadScripts]
#endif
        private static void TryToLoad()
        {
            if (hasLoaded) return;
            hasLoaded = true;

            Library library = Library.Singleton;
            foreach ((Type type, RegisterIntoSingletonAttribute attribute) in TypeCache.GetTypesWithAttribute<RegisterIntoSingletonAttribute>())
            {
                if (!type.IsInterface && !type.IsAbstract)
                {
                    bool hasDefaultConstructor = type.GetConstructor(Type.EmptyTypes) != null;
                    if (hasDefaultConstructor)
                    {
                        IBaseCommand prefab = (IBaseCommand)Activator.CreateInstance(type);
                        library.Add(prefab);
                    }
                    else
                    {
                        IBaseCommand prefab = (IBaseCommand)FormatterServices.GetUninitializedObject(type);
                        library.Add(prefab);
                    }
                }
            }

            //register static methods with the command attribute
            foreach ((MethodInfo method, CommandAttribute attribute) found in TypeCache.GetMembersWithAttribute<CommandAttribute>())
            {
                if (found.method.IsStatic)
                {
                    IBaseCommand prefab = new MethodCommand(found.attribute.Path, found.method);
                    library.Add(prefab);
                }
            }
        }
    }
}