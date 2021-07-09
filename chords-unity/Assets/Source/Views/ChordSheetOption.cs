using System;
using UnityEngine;
using UnityEngine.UI;

public class ChordSheetOption : MonoBehaviour
{
    public Text ChordSheetName;
    public Button LoadButton;

    private string sheetName;
    private Action<string> onLoad;

    private void Start()
    {
        LoadButton.onClick.AddListener(LoadSheet);
    }

    public void Initialize(string sheetName, Action<string> onLoad)
    {
        this.sheetName = sheetName;
        this.onLoad = onLoad;
        ChordSheetName.text = sheetName;
    }

    private void LoadSheet()
    {
        onLoad(sheetName);
    }
}
