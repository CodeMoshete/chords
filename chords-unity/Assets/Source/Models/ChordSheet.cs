using System;
using System.Collections.Generic;

[Serializable]
public class ChordSheet
{
    public List<string> ElementTypes;
    public List<Chord> Chords;
    public List<MelodyDiagramModel> MelodyDiagrams;
}
