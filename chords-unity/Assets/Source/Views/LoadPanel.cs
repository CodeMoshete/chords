using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour
{
    public Button CancelButton;
    public Transform ScrollContainer;
    public GameObject ChordSheetOptionTemplate;

    private List<GameObject> options;
    private Action<ChordSheet> onLoad;

    public LoadPanel()
    {
        options = new List<GameObject>();
    }

    private void Start()
    {
        CancelButton.onClick.AddListener(CloseWindow);
    }

    public void Initialize(Action<ChordSheet> onLoad)
    {
        this.onLoad = onLoad;
    }

    public void ShowWindow(ChordSheetCollection chordSheets)
    {
        gameObject.SetActive(true);

        for (int i = 0, count = options.Count; i < count; ++i)
        {
            Destroy(options[i]);
        }
        options = new List<GameObject>();

        for (int i = 0, count = chordSheets.SheetNames.Count; i < count; ++i)
        {
            GameObject newOption = Instantiate<GameObject>(ChordSheetOptionTemplate, ScrollContainer);
            ChordSheetOption option = newOption.GetComponent<ChordSheetOption>();
            option.Initialize(chordSheets.SheetNames[i], (sheetName) => 
            {
                onLoad(chordSheets.Sheets[chordSheets.SheetNames.IndexOf(sheetName)]);
            });
            options.Add(newOption);
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
