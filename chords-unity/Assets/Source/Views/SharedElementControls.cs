using System;

public class SharedElementControls
{
    public bool IsEnabled;

    private SharedElementControlElements elements;
    private Action onEdit;
    private Action onInsert;
    private Action onDelete;
    private Action onMoveLeft;
    private Action onMoveRight;
    private Action onDuplicate;

    public SharedElementControls(
        SharedElementControlElements elements,
        Action onEdit,
        Action onInsert,
        Action onDelete,
        Action onMoveLeft,
        Action onMoveRight,
        Action onDuplicate)
    {
        this.onEdit = onEdit;
        this.onInsert = onInsert;
        this.onDelete = onDelete;
        this.onMoveLeft = onMoveLeft;
        this.onMoveRight = onMoveRight;
        this.onDuplicate = onDuplicate;

        this.elements = elements;

        IsEnabled = true;

        elements.InsertButton.onClick.AddListener(InsertButtonPressed);
        elements.DeleteButton.onClick.AddListener(DeleteButtonPressed);

        if (elements.EditButton != null)
        {
            elements.EditButton.onClick.AddListener(EditButtonPressed);
        }

        if (elements.DuplicateButton != null)
        {
            elements.DuplicateButton.onClick.AddListener(DuplicateButtonPressed);
        }

        elements.MoveLeftButton.onClick.AddListener(MoveLeftButtonPress);
        elements.MoveRightButton.onClick.AddListener(MoveRightButtonPressed);

        elements.HoverListener.AddHoverListener(ShowControls);
        ShowControls(false);
    }

    public void ShowControls(bool show)
    {
        elements.DeleteButton.gameObject.SetActive(show);
        elements.MoveLeftButton.gameObject.SetActive(show && IsEnabled);
        elements.MoveRightButton.gameObject.SetActive(show && IsEnabled);
        elements.InsertButton.gameObject.SetActive(show && IsEnabled);

        if (elements.DuplicateButton != null)
        {
            elements.DuplicateButton.gameObject.SetActive(show && IsEnabled);
        }

        if (elements.EditButton != null)
        {
            elements.EditButton.gameObject.SetActive(show && IsEnabled);
        }
    }

    private void InsertButtonPressed()
    {
        onInsert();
    }

    private void DeleteButtonPressed()
    {
        onDelete();
    }

    private void DuplicateButtonPressed()
    {
        onDuplicate();
    }

    private void EditButtonPressed()
    {
        onEdit();
    }

    private void MoveLeftButtonPress()
    {
        onMoveLeft();
    }

    private void MoveRightButtonPressed()
    {
        onMoveRight();
    }

    public void Destroy()
    {
        elements.HoverListener.RemoveHoverListener(ShowControls);
    }
}
