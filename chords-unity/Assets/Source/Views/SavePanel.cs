using System;
using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour
{
    public InputField ChordSheetInput;
    public Button SaveButton;
    public Button CancelButton;

    private Action<string> OnSaveInitiated;

    public void Start()
    {
        SaveButton.onClick.AddListener(TriggerSave);
        CancelButton.onClick.AddListener(CloseWindow);
    }

    public void Initialize(Action<string> onSave)
    {
        OnSaveInitiated = onSave;
    }

    private void TriggerSave()
    {
        if (!string.IsNullOrEmpty(ChordSheetInput.text))
        {
            OnSaveInitiated(ChordSheetInput.text);
            CloseWindow();
        }
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
