using System;
using UnityEngine;
using UnityEngine.UI;

public class LabelElement : MonoBehaviour
{
    public Text LabelField;
    public InputField LabelInputField;
    public Button AcceptButton;
    public Button CancelButton;
    public GameObject SetupPanel;
    public GameObject DisplayPanel;
    public SharedElementControlElements SharedElements;

    private SharedElementControls sharedElementControls;
    private Action<LabelElement> onLabelRemoved;
    private Action<int> onInsert;
    private Action<Transform, MoveDirection> onElementMoved;

    public void Initialize(
        Action<LabelElement> onLabelRemoved,
        Action<int> onInsert,
        Action<Transform, MoveDirection> onElementMoved)
    {
        this.onLabelRemoved = onLabelRemoved;
        this.onInsert = onInsert;
        this.onElementMoved = onElementMoved;

        SetupPanel.SetActive(true);
        DisplayPanel.SetActive(false);

        AcceptButton.onClick.AddListener(FinishAndDisplayLabel);
        CancelButton.onClick.AddListener(CancelButtonPressed);

        sharedElementControls = new SharedElementControls(SharedElements, OnEditPressed, OnInsertPressed,
            RemoveFromList, TriggerMoveLeft, TriggerMoveRight, null);
    }

    private void FinishAndDisplayLabel()
    {
        DisplayLabel(LabelInputField.text);
    }

    public void DisplayLabel(string labelContent)
    {
        SetupPanel.SetActive(false);
        LabelField.text = labelContent;
        DisplayPanel.SetActive(true);
    }

    private void OnEditPressed()
    {
        LabelInputField.text = LabelField.text;
        SetupPanel.SetActive(true);
        DisplayPanel.SetActive(false);
    }

    public void TriggerMoveLeft()
    {
        onElementMoved(transform, MoveDirection.Left);
    }

    public void TriggerMoveRight()
    {
        onElementMoved(transform, MoveDirection.Right);
    }

    private void OnInsertPressed()
    {
        onInsert(transform.GetSiblingIndex() + 1);
    }

    private void CancelButtonPressed()
    {
        if (string.IsNullOrEmpty(LabelField.text))
        {
            RemoveFromList();
            return;
        }

        SetupPanel.SetActive(false);
        DisplayPanel.SetActive(true);
    }

    private void RemoveFromList()
    {
        onLabelRemoved(this);
        transform.SetParent(null);
        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        sharedElementControls.Destroy();
    }
}
