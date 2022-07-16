using UnityEngine;
using UnityEngine.UI;

public class ConsoleEntry : Button
{
    [SerializeField]
    private Text label;

    public string Text
    {
        get => label.text;
        set => label.text = value;
    }
}