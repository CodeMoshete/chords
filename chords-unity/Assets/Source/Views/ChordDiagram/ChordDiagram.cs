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
    public Chord CurrentChord;

    private RectTransform rectTransform;
    private Action<ChordDiagram> onChordRemoved;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        DeleteButton.onClick.AddListener(RemoveFromList);
        DisplaySetupPanel();
    }

    public void Initialize(Action<ChordDiagram> onChordRemoved)
    {
        this.onChordRemoved = onChordRemoved;
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
    }

    public void DisplaySetupPanel()
    {
        if (!CurrentChord.isInitialized)
        {
            SetupPanel.SetActive(true);
            DiagramPanel.SetActive(false);
            FinishButton.onClick.AddListener(FinishAndDisplayChord);
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
    }

    private void RemoveFromList()
    {
        onChordRemoved(this);
        transform.SetParent(null);
        GameObject.Destroy(gameObject);
    }
}
