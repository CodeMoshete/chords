using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    public Button AddNewButton;
    public GameObject ChordDiagramTemplate;
    public Transform ScrollContainer;

    public Button ClearButton;

    public Button LoadButton;
    public Button SaveButton;
    public LoadPanel LoadPanel;
    public SavePanel SavePanel;

    public WebRequestManager WebRequestManager;

    private List<ChordDiagram> chords;
    private string currentSheetName;

    private void Start()
    {
        chords = new List<ChordDiagram>();
        AddNewButton.onClick.AddListener(AddNewChord);
        SavePanel.Initialize(SaveCurrentChords);
        LoadPanel.Initialize(LoadChords);
        LoadButton.onClick.AddListener(ShowLoadPanel);
        SaveButton.onClick.AddListener(ShowSavePanel);
        ClearButton.onClick.AddListener(ClearChords);
    }

    private void AddNewChord(Chord chord)
    {
        GameObject newDiagram = Instantiate<GameObject>(ChordDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(ScrollContainer.childCount - 2);
        ChordDiagram diagram = newDiagram.GetComponent<ChordDiagram>();
        diagram.Initialize(OnChordRemoved);
        diagram.DisplayChord(chord);
        chords.Add(diagram);
    }

    private void AddNewChord()
    {
        GameObject newDiagram = Instantiate<GameObject>(ChordDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(ScrollContainer.childCount - 2);
        ChordDiagram diagram = newDiagram.GetComponent<ChordDiagram>();
        diagram.Initialize(OnChordRemoved);
        chords.Add(diagram);
    }

    private void OnChordRemoved(ChordDiagram diagram)
    {
        chords.Remove(diagram);
    }

    private void ClearChords()
    {
        for (int i = 0, count = chords.Count; i < count; ++i)
        {
            Destroy(chords[i].gameObject);
        }
        chords = new List<ChordDiagram>();
    }

    private void ShowLoadPanel()
    {
        WebRequestManager.GetData<ChordSheetCollection>(LoadPanel.ShowWindow);
    }

    private void ShowSavePanel()
    {
        SavePanel.ShowWindow();
    }

    private void SaveCurrentChords(string sheetName)
    {
        currentSheetName = sheetName;
        WebRequestManager.GetData<ChordSheetCollection>(OnSheetsRetrievedForSave);
    }

    private void OnSheetsRetrievedForSave(ChordSheetCollection sheetCollection)
    {
        ChordSheet newSheet = new ChordSheet();
        newSheet.Chords = new List<Chord>();
        newSheet.ElementTypes = new List<string>();
        for (int i = 0, count = chords.Count; i < count; ++i)
        {
            newSheet.Chords.Add(chords[i].CurrentChord);
        }

        if (sheetCollection == null)
        {
            sheetCollection = ChordSheetCollection.Create();
        }

        int sheetIndex = sheetCollection.SheetNames.IndexOf(currentSheetName);
        if (sheetIndex < 0)
        {
            sheetCollection.SheetNames.Add(currentSheetName);
            sheetCollection.Sheets.Add(newSheet);
        }
        else
        {
            sheetCollection.Sheets[sheetIndex] = newSheet;
        }

        string sheetJson = JsonUtility.ToJson(sheetCollection);
        WebRequestManager.PostData(sheetJson, OnSaveComplete);
    }

    private void OnSaveComplete()
    {
        SavePanel.CloseWindow();
    }

    private void LoadChords(ChordSheet sheet)
    {
        ClearChords();
        for (int i = 0, count = sheet.Chords.Count; i < count; ++i)
        {
            AddNewChord(sheet.Chords[i]);
        }
        LoadPanel.CloseWindow();
    }
}
