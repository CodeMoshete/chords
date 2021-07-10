using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Chord
{
    public List<int> StringValues;

    private List<int> normalizedStringValues;
    public List<int> NormalizedStringValues
    {
        get
        {
            if (normalizedStringValues == null)
            {
                normalizedStringValues = new List<int>();
                for (int i = 0, count = StringValues.Count; i < count; ++i)
                {
                    int newVal = StringValues[i] > 0 ?
                        StringValues[i] - (FirstFretNum - 1) :
                        StringValues[i];

                    normalizedStringValues.Add(newVal);
                }
                normalizedStringValues.Reverse();
            }
            return normalizedStringValues;
        }
    }

    public string ChordName;
    public int FirstFretNum;
    public int LowestFretIndex;
    public int LowestFretNum;

    public bool isInitialized;

    public Chord(string chordName, int firstString, int secondString, int thirdString,
        int fourthString, int fifthString, int sixthString)
    {
        ChordName = chordName;

        StringValues = new List<int>(new int[] {
            firstString,
            secondString,
            thirdString,
            fourthString,
            fifthString,
            sixthString
        });

        int highestFret = 0;
        LowestFretNum = 100;

        for (int i = 0, count = StringValues.Count; i < count; ++i)
        {
            int stringVal = StringValues[i];
            if (stringVal > highestFret)
            {
                highestFret = stringVal;
            }

            if (stringVal > 0 && StringValues[i] < LowestFretNum)
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
        
        Debug.Log("done");

        isInitialized = true;
    }
}
