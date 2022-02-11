using System;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelector : MonoBehaviour
{
    public Button NewChordButton;
    public Button NewMelodyButton;
    public Button CancelButton;

    private Action<int> onNewChord;
    private Action<int> onNewMelody;
    private int targetIndex;

    public void Initialize(Action<int> onNewChord, Action<int> onNewMelody)
    {
        this.onNewChord = onNewChord;
        this.onNewMelody = onNewMelody;
        targetIndex = -1;

        NewChordButton.onClick.AddListener(OnNewChordPressed);
        NewMelodyButton.onClick.AddListener(OnNewMelodyPressed);
        CancelButton.onClick.AddListener(OnCancel);
    }

    private void OnNewChordPressed()
    {
        onNewChord(targetIndex);
        OnCancel();
    }

    private void OnNewMelodyPressed()
    {
        onNewMelody(targetIndex);
        OnCancel();
    }

    public void ShowElementSelector(int targetIndex = -1)
    {
        this.targetIndex = targetIndex;
        if (targetIndex >= 0)
        {
            transform.SetSiblingIndex(targetIndex);
        }
        gameObject.SetActive(true);
    }

    private void OnCancel()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 2);
        gameObject.SetActive(false);
    }
}
