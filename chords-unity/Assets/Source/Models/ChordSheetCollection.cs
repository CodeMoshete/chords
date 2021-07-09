using System;
using System.Collections.Generic;

[Serializable]
public class ChordSheetCollection
{
    public List<string> SheetNames;
    public List<ChordSheet> Sheets;

    public static ChordSheetCollection Create()
    {
        ChordSheetCollection newCollection = new ChordSheetCollection();
        newCollection.SheetNames = new List<string>();
        newCollection.Sheets = new List<ChordSheet>();
        return newCollection;
    }
}
