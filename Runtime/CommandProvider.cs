#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using Object = UnityEngine.Object;

namespace Popcron.CommandRunner
{
    public static class CommandProvider
    {
        private const string type = "command";
        private const string displayName = "Commands";

        [SearchItemProvider]
        private static SearchProvider CreateProvider()
        {
            return new SearchProvider(type, displayName)
            {
                priority = 22,
                filterId = "c:",
                showDetails = true,
                toObject = (item, type) => GetObject(item),
                fetchItems = (context, items, provider) => SearchAssets(context, items, provider),
                fetchLabel = (item, context) => FetchLabel(item),
                fetchDescription = (item, context) => FetchDescription(item),
                trackSelection = (item, context) => TrackSelection(item, context)
            };
        }

        [SearchActionsProvider]
        internal static IEnumerable<SearchAction> CreateActionHandlers()
        {
            return new[]
            {
                new SearchAction(type, "run", null, "Run", Execute),
                new SearchAction(type, "edit", null, "Edit", OpenItem)
            };
        }

        private static void Execute(SearchItem[] items)
        {
            foreach (SearchItem item in items)
            {
                CommandInfo info = (CommandInfo)item.data;
                CommandRunner.Singleton.Run(info.path);
            }
        }

        private static void OpenItem(SearchItem item)
        {
            Object asset = GetObject(item);
            if (!asset || !AssetDatabase.OpenAsset(asset))
            {
                string path = AssetDatabase.GetAssetPath(asset);
                if (!string.IsNullOrEmpty(path))
                {
                    EditorUtility.OpenWithDefaultApp(path);
                }
            }
        }

        private static void TrackSelection(SearchItem item, SearchContext context)
        {

        }

        private static string FetchDescription(SearchItem item)
        {
            CommandInfo info = (CommandInfo)item.data;
            if (info.command is IDescription description)
            {
                return description.Description;
            }
            else
            {
                return null;
            }
        }

        private static string FetchLabel(SearchItem item)
        {
            CommandInfo info = (CommandInfo)item.data;
            return info.command.Path;
        }

        private static object SearchAssets(SearchContext context, List<SearchItem> items, SearchProvider provider)
        {
            items.AddRange(SearchAssets(context, provider));
            return null;
        }

        private static IEnumerable<SearchItem> SearchAssets(SearchContext context, SearchProvider provider)
        {
            if (string.IsNullOrEmpty(context.searchText))
            {
                yield break;
            }

            foreach (IBaseCommand prefab in Library.Singleton.Search(context.searchText))
            {
                CommandInfo info = new CommandInfo(prefab);
                yield return provider.CreateItem(context, prefab.Path, -1, null, null, null, info);
            }
        }

        private static Object GetObject(SearchItem item)
        {
            CommandInfo info = (CommandInfo)item.data;
            Type commandType = info.command.GetType();
            string searchInput = commandType.FullName;
            string[] search = AssetDatabase.FindAssets($"t:script {searchInput}");
            if (search.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(search[0]);
                return AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            }

            return null;
        }

        public readonly struct CommandInfo
        {
            public readonly IBaseCommand command;
            public readonly string path;

            public CommandInfo(IBaseCommand command)
            {
                this.command = command;
                this.path = command.Path;
            }
        }
    }
}
#endif