using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChordDiagram : MonoBehaviour
{
    public GameObject SetupPanel;
    public GameObject DiagramPanel;
    public UIHoverListener HoverListener;

    // Setup Panel
    public InputField ChordNameInput;
    public List<InputField> StringValueInputs;
    public List<Button> ToggleOpenButtons;
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
    public Button InsertButton;
    public Button MoveLeftButton;
    public Button MoveRightButton;
    public Chord CurrentChord;

    private List<GameObject> dots;
    private RectTransform rectTransform;
    private Action<ChordDiagram> onChordRemoved;
    private Action<ChordDiagram> onChordDuplicated;
    private Action<int> onInsert;
    private Action<Transform, MoveDirection> onElementMoved;

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
        InsertButton.onClick.AddListener(OnInsertPressed);
        FinishButton.onClick.AddListener(FinishAndDisplayChord);
        HoverListener.AddHoverListener(OnHoverStateChanged);
        MoveLeftButton.onClick.AddListener(TriggerMoveLeft);
        MoveRightButton.onClick.AddListener(TriggerMoveRight);

        for (int i = 0, count = ToggleOpenButtons.Count; i < count; ++i)
        {
            SetupToggleOnOffButton(ToggleOpenButtons[i], i);
        }

        DisplaySetupPanel();
    }

    public void Initialize(
        Action<ChordDiagram> onChordRemoved, 
        Action<ChordDiagram> onChordDuplicated, 
        Action<int> onInsert,
        Action<Transform, MoveDirection> onElementMoved)
    {
        this.onChordRemoved = onChordRemoved;
        this.onChordDuplicated = onChordDuplicated;
        this.onInsert = onInsert;
        this.onElementMoved = onElementMoved;
    }

    private void OnHoverStateChanged(bool isMouseHovering)
    {
        DeleteButton.gameObject.SetActive(isMouseHovering);
        bool isInDisplayMode = DiagramPanel.activeSelf;
        EditButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
        InsertButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
        DuplicateButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
        MoveLeftButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
        MoveRightButton.gameObject.SetActive(isMouseHovering && isInDisplayMode);
    }

    public void DisplaySetupPanel()
    {
        if (!CurrentChord.isInitialized)
        {
            SetupPanel.SetActive(true);
            DiagramPanel.SetActive(false);
            OnHoverStateChanged(false);

            EventSystem.current.SetSelectedGameObject(StringValueInputs[5].gameObject, null);
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
        OnHoverStateChanged(false);

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
        for (int i = 0, count = OpenStringIcons.Count; i < count; ++i)
        {
            OpenStringIcons[i].SetActive(false);
        }

        for (int i = 0, count = BlockedStringIcons.Count; i < count; ++i)
        {
            BlockedStringIcons[i].SetActive(false);
        }

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
        OnHoverStateChanged(false);
    }

    private void SetupToggleOnOffButton(Button button, int index)
    {
        button.onClick.AddListener(() =>
        {
            string currentVal = StringValueInputs[index].text;
            if (currentVal == "-1")
            {
                StringValueInputs[index].text = "0";
            }
            else
            {
                StringValueInputs[index].text = "-1";
            }
        });
    }

    private void DuplicateChord()
    {
        onChordDuplicated(this);
    }

    private void OnInsertPressed()
    {
        onInsert(transform.GetSiblingIndex() + 1);
    }

    public void TriggerMoveLeft()
    {
        onElementMoved(transform, MoveDirection.Left);
    }

    public void TriggerMoveRight()
    {
        onElementMoved(transform, MoveDirection.Right);
    }

    private void RemoveFromList()
    {
        onChordRemoved(this);
        transform.SetParent(null);
        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        HoverListener.RemoveHoverListener(OnHoverStateChanged);
    }
}
