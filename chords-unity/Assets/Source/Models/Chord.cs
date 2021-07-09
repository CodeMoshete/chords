using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chord
{
    private List<int> stringValues;
    public List<int> NormalizedStringValues;

    public string ChordName;
    public int FirstFretNum;
    public int LowestFretIndex;
    public int LowestFretNum;

    public bool isInitialized;

    public Chord(string chordName, int firstString, int secondString, int thirdString,
        int fourthString, int fifthString, int sixthString)
    {
        ChordName = chordName;

        stringValues = new List<int>(new int[] {
            firstString,
            secondString,
            thirdString,
            fourthString,
            fifthString,
            sixthString
        });

        NormalizedStringValues = new List<int>();

        int highestFret = 0;
        LowestFretNum = 100;

        for (int i = 0, count = stringValues.Count; i < count; ++i)
        {
            int stringVal = stringValues[i];
            if (stringVal > highestFret)
            {
                highestFret = stringVal;
            }

            if (stringVal > 0 && stringValues[i] < LowestFretNum)
            {
                LowestFretNum = stringVal;
            }
        }

        if (LowestFretNum == 1)
        {
            FirstFretNum = 1;
            LowestFretIndex = 1;
        }
        else
        {
            int difference = highestFret - LowestFretNum;
            FirstFretNum = difference < 3 ? LowestFretNum - 1 : LowestFretNum;
            LowestFretIndex = difference < 3 ? 2 : 1;
        }

        for (int i = 0, count = stringValues.Count; i < count; ++i)
        {
            int newVal = stringValues[i] > 0 ? stringValues[i] - (FirstFretNum - 1) : stringValues[i];
            NormalizedStringValues.Add(newVal);
        }
        NormalizedStringValues.Reverse();
        Debug.Log("done");

        isInitialized = true;
    }
}
