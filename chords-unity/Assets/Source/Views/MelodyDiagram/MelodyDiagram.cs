using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MelodyDiagram : MonoBehaviour
{
    private Action<MelodyDiagram> onMelodyRemoved;
    private Action<MelodyDiagram> onMelodyDuplicated;
    private List<MelodyDot> melodyDots;
    private MelodyDot selectedDot;

    public MelodyDiagramModel CurrentMelody { get; private set; }

    public UIHoverListener hoverListener;
    public GameObject FretEnterPanel;
    public InputField FretInput;
    public Button SetButton;
    public Button CancelButton;
    public Button DeleteButton;

    private void Update()
    {
        if (FretEnterPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SetFretButtonPressed();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelFretButtonPressed();
            }
        }
    }

    public void Initialize(Action<MelodyDiagram> onMelodyRemoved, Action<MelodyDiagram> onMelodyDuplicated)
    {
        melodyDots = new List<MelodyDot>(gameObject.GetComponentsInChildren<MelodyDot>());
        for (int i = 0, count = melodyDots.Count; i < count; ++i)
        {
            melodyDots[i].OnDotPressed = OnDotPressed;
            melodyDots[i].OnDotDeleted = OnDotDeleted;
        }

        SetButton.onClick.AddListener(SetFretButtonPressed);
        CancelButton.onClick.AddListener(CancelFretButtonPressed);
        DeleteButton.onClick.AddListener(DeleteButtonPressed);

        hoverListener.AddHoverListener(showDeleteButton);
        showDeleteButton(false);

        this.onMelodyRemoved = onMelodyRemoved;
        this.onMelodyDuplicated = onMelodyDuplicated;
    }

    private void showDeleteButton(bool show)
    {
        DeleteButton.gameObject.SetActive(show);
    }

    public void DisplayMelody(MelodyDiagramModel melody)
    {
        for (int i = 0, count = melody.MelodyDots.Count; i < count; ++i)
        {
            MelodyDotModel dotModel = melody.MelodyDots[i];
            melodyDots[dotModel.SequenceIndex].SetActive(true, dotModel.FretNum);
        }
    }

    public MelodyDiagramModel GetModel()
    {
        MelodyDiagramModel retModel = new MelodyDiagramModel();
        retModel.MelodyDots = new List<MelodyDotModel>();
        for (int i = 0, count = melodyDots.Count; i < count; ++i)
        {
            if (melodyDots[i].IsActive)
            {
                MelodyDotModel newDot = new MelodyDotModel();
                newDot.SequenceIndex = i;
                newDot.FretNum = melodyDots[i].FretNum;
                retModel.MelodyDots.Add(newDot);
            }
        }
        return retModel;
    }

    private void SetFretButtonPressed()
    {
        int fretNum = -1;
        bool didParse = int.TryParse(FretInput.text, out fretNum);
        if (didParse)
        {
            selectedDot.SetActive(true, fretNum);
            selectedDot = null;
            FretEnterPanel.SetActive(false);
        }
    }

    private void CancelFretButtonPressed()
    {
        selectedDot = null;
        FretEnterPanel.SetActive(false);
    }

    private void OnDotPressed(MelodyDot dot)
    {
        selectedDot = dot;
        Debug.Log("Dot Pressed: " + dot.name);
        FretEnterPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(FretInput.gameObject, null);
    }

    private void OnDotDeleted(MelodyDot dot)
    {
        Debug.Log("Dot Deleted: " + dot.name);
        dot.SetActive(false);
    }

    private void DeleteButtonPressed()
    {
        onMelodyRemoved(this);
        Destroy(gameObject);
    }
}
