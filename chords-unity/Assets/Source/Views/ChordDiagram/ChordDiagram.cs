using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChordDiagram : MonoBehaviour
{
    public GameObject SetupPanel;
    public GameObject DiagramPanel;

    // Setup Panel
    public InputField ChordNameInput;
    public List<InputField> StringValueInputs;
    public Button FinishButton;

    // Diagram Panel
    public Text ChordNameField;
    public GameObject BaseBoard;
    public Transform DotsContainer;
    public List<GameObject> OpenStringIcons;
    public List<GameObject> BlockedStringIcons;
    public GameObject ChordDotTemplate;
    public Text FretNumLabelTop;
    public Text FretNumLabelBottom;
    public Button DeleteButton;
    public Button EditButton;
    public Button DuplicateButton;
    public Chord CurrentChord;

    private List<GameObject> dots;
    private RectTransform rectTransform;
    private Action<ChordDiagram> onChordRemoved;
    private Action<ChordDiagram> onChordDuplicated;

    public ChordDiagram()
    {
        dots = new List<GameObject>();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        DeleteButton.onClick.AddListener(RemoveFromList);
        EditButton.onClick.AddListener(EditChord);
        DuplicateButton.onClick.AddListener(DuplicateChord);
        FinishButton.onClick.AddListener(FinishAndDisplayChord);
        DisplaySetupPanel();
    }

    public void Initialize(Action<ChordDiagram> onChordRemoved, Action<ChordDiagram> onChordDuplicated)
    {
        this.onChordRemoved = onChordRemoved;
        this.onChordDuplicated = onChordDuplicated;
    }

    private void Update()
    {
        Vector3 testPos = rectTransform.TransformPoint(new Vector3(rectTransform.rect.x, rectTransform.rect.y, 0f));
        Vector3 testPos2 = rectTransform.TransformPoint(new Vector3(rectTransform.rect.x + rectTransform.rect.width, 
            rectTransform.rect.y + rectTransform.rect.height, 0f));
        Vector3 mousePos = Input.mousePosition;
        bool isMouseHovering = mousePos.x > testPos.x && mousePos.x < testPos2.x && 
            mousePos.y > testPos.y && mousePos.y < testPos2.y;
        //Debug.Log(testPos.ToString() + " " + testPos2.ToString() + " : " + Input.mousePosition.ToString() + " " + isMouseHovering);
        DeleteButton.gameObject.SetActive(isMouseHovering);
        bool isInDisplayMode = DiagramPanel.activeSelf;
        EditButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
        DuplicateButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
    }

    public void DisplaySetupPanel()
    {
        if (!CurrentChord.isInitialized)
        {
            SetupPanel.SetActive(true);
            DiagramPanel.SetActive(false);

            //EventSystem.current.SetSelectedGameObject(StringValueInputs[0].gameObject, null);
            //StringValueInputs[0].OnPointerClick(null);
        }
    }

    private void FinishAndDisplayChord()
    {
        Chord chord = new Chord(
            ChordNameInput.text,
            GetStringVal(StringValueInputs[0].text),
            GetStringVal(StringValueInputs[1].text),
            GetStringVal(StringValueInputs[2].text),
            GetStringVal(StringValueInputs[3].text),
            GetStringVal(StringValueInputs[4].text),
            GetStringVal(StringValueInputs[5].text));

        DisplayChord(chord);
    }

    private int GetStringVal(string input)
    {
        int returnVal;
        int.TryParse(input, out returnVal);
        return returnVal;
    }

    public void DisplayChord(Chord chord)
    {
        CurrentChord = chord;
        SetupPanel.SetActive(false);
        DiagramPanel.SetActive(true);

        BaseBoard.SetActive(chord.FirstFretNum == 1);
        ChordNameField.text = chord.ChordName;

        for (int i = 0, count = chord.NormalizedStringValues.Count; i < count; ++i)
        {
            int fretNum = chord.NormalizedStringValues[i];
            if (fretNum < 0)
            {
                SetStringBlocked(i);
            }
            else if (fretNum == 0)
            {
                SetStringOpen(i);
            }
            else
            {
                SetDotOnFret(i, fretNum);
            }
        }

        if (chord.LowestFretIndex == 1)
        {
            FretNumLabelTop.text = chord.LowestFretNum.ToString();
        }
        else
        {
            FretNumLabelBottom.text = chord.LowestFretNum.ToString();
        }
    }

    private void SetStringOpen(int stringNum)
    {
        OpenStringIcons[stringNum].SetActive(true);
    }

    private void SetStringBlocked(int stringNum)
    {
        BlockedStringIcons[stringNum].SetActive(true);
    }

    private void SetDotOnFret(int stringNum, int fretNum)
    {
        Vector2 spawnPos = new Vector2(
            Constants.STRING_POSITIONS_X[stringNum],
            Constants.FRET_POSITIONS_Y[fretNum - 1]);
        GameObject dot = Instantiate(ChordDotTemplate, transform);
        dot.transform.localPosition = spawnPos;
        dots.Add(dot);
    }

    private void EditChord()
    {
        for (int i = 0, count = dots.Count; i < count; ++i)
        {
            Destroy(dots[i]);
        }

        for (int i = 0, count = CurrentChord.StringValues.Count; i < count; ++i)
        {
            StringValueInputs[i].text = CurrentChord.StringValues[i].ToString();
        }

        ChordNameInput.text = CurrentChord.ChordName;
        SetupPanel.SetActive(true);
        DiagramPanel.SetActive(false);
    }

    private void DuplicateChord()
    {
        onChordDuplicated(this);
    }

    private void RemoveFromList()
    {
        onChordRemoved(this);
        transform.SetParent(null);
        GameObject.Destroy(gameObject);
    }
}
