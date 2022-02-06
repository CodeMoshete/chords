using System;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelector : MonoBehaviour
{
    public Button NewChordButton;
    public Button NewMelodyButton;
    public Button CancelButton;

    private Action onNewChord;
    private Action onNewMelody;

    public void Initialize(Action onNewChord, Action onNewMelody)
    {
        this.onNewChord = onNewChord;
        this.onNewMelody = onNewMelody;

        NewChordButton.onClick.AddListener(OnNewChordPressed);
        NewMelodyButton.onClick.AddListener(OnNewMelodyPressed);
        CancelButton.onClick.AddListener(OnCancel);
    }

    private void OnNewChordPressed()
    {
        onNewChord();
        OnCancel();
    }

    private void OnNewMelodyPressed()
    {
        onNewMelody();
        OnCancel();
    }

    public void ShowElementSelector()
    {
        gameObject.SetActive(true);
    }

    private void OnCancel()
    {
        gameObject.SetActive(false);
    }
}
