using System;
using System.Collections.Generic;

public enum ElementType
{
    Chord,
    Melody,
    Label
}

[Serializable]
public class ChordSheet
{
    private List<ElementType> elementTypes;
    public List<ElementType> ElementTypes
    {
        get
        {
            if (elementTypes == null)
            {
                elementTypes = new List<ElementType>();
                if (ElementTypesInt != null)
                {
                    for(int i = 0, count = ElementTypesInt.Count; i < count; ++i)
                    {
                        elementTypes.Add((ElementType)ElementTypesInt[i]);
                    }
                }
            }
            return elementTypes;
        }
    }

    public List<int> ElementTypesInt;
    public List<Chord> Chords;
    public List<MelodyDiagramModel> MelodyDiagrams;
    public List<string> Labels;
}
