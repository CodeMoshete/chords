using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChordDiagram : MonoBehaviour
{
    public Text ChordNameField;
    public GameObject BaseBoard;
    public Transform DotsContainer;
    public List<GameObject> OpenStringIcons;
    public List<GameObject> BlockedStringIcons;
    public GameObject ChordDotTemplate;
    public Text FretNumLabelTop;
    public Text FretNumLabelBottom;

    public void DisplayChord(Chord chord)
    {
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
}
