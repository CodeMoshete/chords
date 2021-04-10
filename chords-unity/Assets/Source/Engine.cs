using UnityEngine;

public class Engine : MonoBehaviour
{
    public ChordDiagram Chord;

    private void Start()
    {
        Chord chord = new Chord("Asus2add13", 5, -1, 0, 4, 5, 0);
        Chord.DisplayChord(chord);
    }
}
