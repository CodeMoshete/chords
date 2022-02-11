using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    public Button AddNewButton;
    public GameObject ChordDiagramTemplate;
    public GameObject MelodyDiagramTemplate;
    public Transform ScrollContainer;

    public Button ClearButton;

    public Button LoadButton;
    public Button SaveButton;
    public LoadPanel LoadPanel;
    public SavePanel SavePanel;

    public ElementSelector ElementSelector;

    public WebRequestManager WebRequestManager;

    private List<ChordDiagram> chords;
    private List<MelodyDiagram> melodies;
    private string currentSheetName;

    private void Start()
    {
        ElementSelector.Initialize(AddNewChord, AddNewMelody);
        chords = new List<ChordDiagram>();
        melodies = new List<MelodyDiagram>();
        AddNewButton.onClick.AddListener(AddNewElement);
        SavePanel.Initialize(SaveCurrentChords);
        LoadPanel.Initialize(LoadElements);
        LoadButton.onClick.AddListener(ShowLoadPanel);
        SaveButton.onClick.AddListener(ShowSavePanel);
        ClearButton.onClick.AddListener(ClearAll);
    }

    private void AddNewElement()
    {
        ElementSelector.ShowElementSelector();
    }

    public void AddNewElement(int targetIndex = -1)
    {
        ElementSelector.ShowElementSelector(targetIndex);
    }

    private void AddNewChord(Chord chord)
    {
        GameObject newDiagram = Instantiate<GameObject>(ChordDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(ScrollContainer.childCount - 3);
        ChordDiagram diagram = newDiagram.GetComponent<ChordDiagram>();
        diagram.Initialize(OnChordRemoved, OnChordDuplicated, AddNewElement, MoveElement);
        diagram.DisplayChord(chord);
        chords.Add(diagram);
    }

    private void AddNewChord(int targetIndex = -1)
    {
        GameObject newDiagram = Instantiate<GameObject>(ChordDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(targetIndex >= 0 ? targetIndex : ScrollContainer.childCount - 3);
        ChordDiagram diagram = newDiagram.GetComponent<ChordDiagram>();
        diagram.Initialize(OnChordRemoved, OnChordDuplicated, AddNewElement, MoveElement);
        chords.Add(diagram);
    }

    private void OnChordRemoved(ChordDiagram diagram)
    {
        chords.Remove(diagram);
    }

    private void OnChordDuplicated(ChordDiagram diagram)
    {
        AddNewChord(diagram.CurrentChord);
    }

    private void AddNewMelody(MelodyDiagramModel melody)
    {
        GameObject newDiagram = Instantiate<GameObject>(MelodyDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(ScrollContainer.childCount - 3);
        MelodyDiagram diagram = newDiagram.GetComponent<MelodyDiagram>();
        diagram.Initialize(OnMelodyRemoved, OnMelodyDuplicated, AddNewElement, MoveElement);
        diagram.DisplayMelody(melody);
        melodies.Add(diagram);
    }

    private void AddNewMelody(int targetIndex = -1)
    {
        GameObject newDiagram = Instantiate<GameObject>(MelodyDiagramTemplate, ScrollContainer);
        newDiagram.transform.SetSiblingIndex(targetIndex >= 0 ? targetIndex : ScrollContainer.childCount - 3);
        MelodyDiagram diagram = newDiagram.GetComponent<MelodyDiagram>();
        diagram.Initialize(OnMelodyRemoved, OnMelodyDuplicated, AddNewElement, MoveElement);
        melodies.Add(diagram);
    }

    private void OnMelodyRemoved(MelodyDiagram diagram)
    {
        melodies.Remove(diagram);
    }

    private void OnMelodyDuplicated(MelodyDiagram diagram)
    {
        AddNewMelody(diagram.CurrentMelody);
    }

    private void ClearAll()
    {
        ClearMelodies();
        ClearChords();
    }

    private void ClearChords()
    {
        for (int i = 0, count = chords.Count; i < count; ++i)
        {
            Destroy(chords[i].gameObject);
        }
        chords = new List<ChordDiagram>();
    }

    private void ClearMelodies()
    {
        for (int i = 0, count = melodies.Count; i < count; ++i)
        {
            Destroy(melodies[i].gameObject);
        }
        melodies = new List<MelodyDiagram>();
    }

    private void MoveElement(Transform element, MoveDirection direction)
    {
        int numSiblings = ScrollContainer.childCount;
        int currentSiblingIndex = element.GetSiblingIndex();
        if (direction == MoveDirection.Left && currentSiblingIndex > 0)
        {
            element.SetSiblingIndex(currentSiblingIndex - 1);
        }
        else if (direction == MoveDirection.Right && currentSiblingIndex < numSiblings - 3)
        {
            element.SetSiblingIndex(currentSiblingIndex + 1);
        }
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
        newSheet.MelodyDiagrams = new List<MelodyDiagramModel>();
        newSheet.ElementTypesInt = new List<int>();
        for (int i = 0, count = chords.Count; i < count; ++i)
        {
            newSheet.Chords.Add(chords[i].CurrentChord);
        }

        for (int i = 0, count = melodies.Count; i < count; ++i)
        {
            newSheet.MelodyDiagrams.Add(melodies[i].GetModel());
        }

        int numChildren = ScrollContainer.childCount;
        for (int i = 0; i < numChildren; ++i)
        {
            Transform child = ScrollContainer.GetChild(i);
            if (child.GetComponent<ChordDiagram>() != null)
            {
                newSheet.ElementTypesInt.Add((int)ElementType.Chord);
            }
            else if (child.GetComponent<MelodyDiagram>() != null)
            {
                newSheet.ElementTypesInt.Add((int)ElementType.Melody);
            }
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

    private void LoadLegacyElement(ChordSheet sheet)
    {
        for (int i = 0, count = sheet.Chords.Count; i < count; ++i)
        {
            AddNewChord(sheet.Chords[i]);
        }
    }

    private void LoadElements(ChordSheet sheet)
    {
        ClearChords();
        ClearMelodies();
        if ((sheet.ElementTypes == null || sheet.ElementTypes.Count == 0) && sheet.Chords.Count > 0)
        {
            LoadLegacyElement(sheet);
            LoadPanel.CloseWindow();
            return;
        }

        int chordIndex = 0;
        int melodyIndex = 0;
        for (int i = 0, count = sheet.ElementTypes.Count; i < count; ++i)
        {
            switch (sheet.ElementTypes[i])
            {
                case ElementType.Chord:
                    AddNewChord(sheet.Chords[chordIndex]);
                    chordIndex++;
                    break;
                case ElementType.Melody:
                    AddNewMelody(sheet.MelodyDiagrams[melodyIndex]);
                    melodyIndex++;
                    break;
            }

        }

        LoadPanel.CloseWindow();
    }
}
