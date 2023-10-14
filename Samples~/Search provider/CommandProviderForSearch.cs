#nullable enable
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.QuickSearch;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Popcron.CommandRunner
{
    public static class CommandProviderForSearch
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();

        public const string SearchServiceID = "command";
        public const string DisplayName = "Commands";

        [SearchItemProvider]
        private static SearchProvider CreateProvider()
        {
            return new SearchProvider(SearchServiceID, DisplayName)
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
                new SearchAction(SearchServiceID, "run", null, "Run", Execute),
                new SearchAction(SearchServiceID, "edit", null, "Edit", OpenItem)
            };
        }

        private static void Execute(SearchItem[] items)
        {
            foreach (SearchItem item in items)
            {
                IBaseCommand command = (IBaseCommand)item.data;
                UniTask.Create(async () =>
                {
                    ExecutionResult result = await CommandRunner.Singleton.Run(command);
                    Debug.Log(result.commandInput?.ToString());
                });
            }
        }

        private static void OpenItem(SearchItem item)
        {
            Object? asset = GetObject(item);
            if (asset != null || !AssetDatabase.OpenAsset(asset))
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
            IBaseCommand command = (IBaseCommand)item.data;
            if (command is ICommandInformation description)
            {
                stringBuilder.Clear();
                description.Append(stringBuilder);
                return stringBuilder.ToString();
            }
            else
            {
                return command.GetType().Name;
            }
        }

        private static string FetchLabel(SearchItem item)
        {
            IBaseCommand command = (IBaseCommand)item.data;
            return command.Path.ToString();
        }

        private static object SearchAssets(SearchContext context, List<SearchItem> items, SearchProvider provider)
        {
            return SearchCommands(context, provider);
        }

        private static IEnumerable<SearchItem> SearchCommands(SearchContext context, SearchProvider provider)
        {
            if (string.IsNullOrEmpty(context.searchText))
            {
                return Array.Empty<SearchItem>();
            }

            ReadOnlySpan<char> searchText = context.searchText.AsSpan();
            if (!string.IsNullOrEmpty(context.filterId))
            {
                searchText = searchText.Slice(context.filterId.Length);
            }

            HashSet<SearchItem> foundItems = new HashSet<SearchItem>();
            SearchCommand search = new SearchCommand();
            foreach (IBaseCommand command in search.Search(searchText, Library.Singleton))
            {
                SearchItem item = provider.CreateItem(context, command.Path.ToString(), -1, null, null, null, command);
                if (command is ICommandInformation commandInfo)
                {
                    stringBuilder.Clear();
                    commandInfo.Append(stringBuilder);
                    item.description = stringBuilder.ToString();
                }

                foundItems.Add(item);
            }

            return foundItems;
        }

        private static Object? GetObject(SearchItem item)
        {
            IBaseCommand command = (IBaseCommand)item.data;
            Type commandType = command.GetType();
            string searchInput = commandType.FullName;
            string[] search = AssetDatabase.FindAssets($"t:script {searchInput}");
            if (search.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(search[0]);
                return AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            }

            return null;
        }
    }

    public class EditorCommandRunner : EditorWindow
    {
        private const float Height = 24f;
        private const float RunButtonWidth = 50f;

        private string search;
        private bool focused;

        private void OnEnable()
        {
            minSize = new Vector2(100f, Height);
            maxSize = new Vector2(300f, Height);
            search = EditorPrefs.GetString("editorSearchCommand", "");
        }

        private void OnDisable()
        {
            EditorPrefs.SetString("editorSearchCommand", search);
            focused = false;
        }

        private void OnGUI()
        {
            Event e = Event.current;
            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("SearchField");
            EditorGUILayout.BeginHorizontal();
            string newSearch = EditorGUILayout.TextField(search);
            if (EditorGUI.EndChangeCheck())
            {
                search = newSearch;
            }

            if (GUILayout.Button("Run", GUILayout.Width(RunButtonWidth)))
            {
                Run();
                Close();
            }

            EditorGUILayout.EndHorizontal();
            if (e.isKey)
            {
                if (e.keyCode == KeyCode.Return)
                {
                    Run();
                    Close();
                }
                else if (e.keyCode == KeyCode.Escape)
                {
                    Close();
                }
            }
            else if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.BackQuote)
                {
                    Close();
                }
            }

            if (!focused)
            {
                GUI.FocusControl("SearchField");
                focused = true;
            }
        }

        private void Run()
        {
            if (!string.IsNullOrEmpty(search))
            {
                UniTask.Create(async () =>
                {
                    ExecutionResult result = await CommandRunner.Singleton.FindAndRunAsync(search);
                    Debug.Log(result.commandInput?.ToString());
                });
            }
        }
    }

    [InitializeOnLoad]
    public static class EditorCommandRunnerManager
    {
        private static EditorCommandRunner? runner;

        static EditorCommandRunnerManager()
        {
            SceneView.duringSceneGui += DuringSceneGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            OnGUI();
        }

        private static void ProjectWindowItemInstanceOnGUI(int instanceID, Rect selectionRect)
        {
            OnGUI();
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            OnGUI();
        }

        private static void DuringSceneGUI(SceneView view)
        {
            OnGUI();
        }

        private static void OnGUI()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.BackQuote)
            {
                if (runner == null)
                {
                    runner = EditorWindow.GetWindow<EditorCommandRunner>(true, nameof(CommandRunner), true);
                }
                else
                {
                    runner.Close();
                    runner = null;
                }
            }
        }
    }
}