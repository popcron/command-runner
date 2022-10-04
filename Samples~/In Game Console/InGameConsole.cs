using System;
using System.Collections.Generic;
using Popcron.CommandRunner;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameConsole : MonoBehaviour
{
    private static InGameConsole instance;

    /// <summary>
    /// The potential singleton in game console component in the scene.
    /// </summary>
    public static InGameConsole Singleton
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<InGameConsole>();
                if (!instance)
                {
                    Debug.LogErrorFormat("The {0} component is missing from the scene", typeof(InGameConsole));
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private Transform window;

    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Text previewCommand;

    [SerializeField]
    private ScrollRect suggestions;

    [SerializeField]
    private ConsoleEntry suggestionPrefab;

    private List<ConsoleEntry> entries = new List<ConsoleEntry>();
    private List<string> history = new List<string>();
    private bool isOpen;
    private int selectedEntry;
    private int selectedHistoricEntry;

    public bool IsOpen
    {
        get => window.gameObject.activeSelf;
        set
        {
            bool isOpen = window.gameObject.activeSelf;
            if (isOpen != value)
            {
                previewCommand.enabled = false;
                window.gameObject.SetActive(value);
                if (value)
                {
                    ClearAllEntries();
                    inputField.text = "";
                    SelectAndFocus();
                }
            }
        }
    }

    private void SelectAndFocus()
    {
        inputField.Select();
        inputField.ActivateInputField();

        EventSystem es = EventSystem.current;
        if (es)
        {
            es.SetSelectedGameObject(inputField.gameObject, null);
        }
    }

    private void Awake()
    {
        IsOpen = false;
    }

    private void OnEnable()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
        inputField.onSubmit.AddListener(OnSubmit);
    }

    private void OnDisable()
    {
        inputField.onValueChanged.RemoveListener(OnValueChanged);
        inputField.onSubmit.RemoveListener(OnSubmit);
    }

    public void Submit() //invoked from the ui
    {
        OnSubmit(inputField.text);
    }

    private void OnSubmit(string text)
    {
        history.Add(text);

        bool wroteAnything = false;
        Result result = CommandRunner.Singleton.Run(text);
        if (result?.HasLogs == true)
        {
            ClearAllEntries();
            foreach (string log in result.Logs)
            {
                if (log.IndexOf('\n') != -1)
                {
                    string[] lines = log.Split('\n');
                    foreach (string line in lines)
                    {
                        CreateEntry(line, null);
                        wroteAnything = true;
                    }
                }
                else
                {
                    CreateEntry(log, null);
                    wroteAnything = true;
                }
            }
        }

        if (wroteAnything)
        {
            inputField.MoveTextStart(false);
            inputField.MoveTextEnd(true);
        }
        else
        {
            IsOpen = false;
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            KeyCode pressedKey = e.keyCode;
            bool wantsToClose = pressedKey == KeyCode.Escape;
            if (wantsToClose && IsOpen)
            {
                IsOpen = false;
            }

            bool wantsToToggle = pressedKey == KeyCode.BackQuote || pressedKey == KeyCode.F2;
            if (wantsToToggle)
            {
                IsOpen = !IsOpen;
            }

            if (inputField.isFocused)
            {
                bool next = pressedKey == KeyCode.Tab || pressedKey == KeyCode.DownArrow;
                if (next && entries.Count > 0)
                {
                    selectedEntry = selectedEntry + 1 % entries.Count;
                    previewCommand.enabled = true;
                    previewCommand.text = entries[selectedEntry].Text;

                    AutocompleteSuggestion();
                }

                bool previous = pressedKey == KeyCode.UpArrow;
                if (previous && history.Count > 0)
                {
                    int index = history.Count - selectedHistoricEntry - 1;
                    if (history.Count > index && index >= 0)
                    {
                        Autocomplete(history[index]);
                        selectedHistoricEntry++;
                    }
                }
            }
            else
            {
                bool wantsToRefocus = pressedKey == KeyCode.A && e.modifiers.HasFlag(EventModifiers.Control);
                if (wantsToRefocus)
                {
                    SelectAndFocus();
                    inputField.MoveTextStart(false);
                    inputField.MoveTextEnd(true);
                }
            }
        }
    }

    private void OnValueChanged(string newValue)
    {
        ClearAllEntries();
        if (!string.IsNullOrEmpty(newValue))
        {
            foreach (IBaseCommand prefab in Library.Singleton.Prefabs)
            {
                string text = prefab.Path;
                if (text.StartsWith(newValue))
                {
                    CreateEntry(text, () => Autocomplete(text));
                }
            }
        }

        if (entries.Count > 0)
        {
            previewCommand.enabled = true;
            previewCommand.text = entries[0].Text;
            selectedEntry = 0;
        }
        else
        {
            previewCommand.enabled = false;
            selectedEntry = -1;
        }
    }

    private void Autocomplete(string text)
    {
        inputField.text = text;
        inputField.MoveTextEnd(true);
    }

    private void AutocompleteSuggestion()
    {
        string suggestion = entries[selectedEntry].Text;
        Autocomplete(suggestion);
    }

    private void ClearAllEntries()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            Destroy(entries[i].gameObject);
        }

        entries.Clear();
    }

    private void CreateEntry(string text, Action execute)
    {
        ConsoleEntry entry = Instantiate(suggestionPrefab, suggestions.content);
        entry.Text = text;
        entry.onClick.AddListener(() => execute?.Invoke());
        entries.Add(entry);
    }
}
