using System;
using UnityEngine;
using UnityEngine.UI;

public class MelodyDot : MonoBehaviour
{
    public UIHoverListener HoverListener;
    public GameObject VisContainer;
    public Button SetButton;
    public Button DeleteButton;
    public Action<MelodyDot> OnDotPressed;
    public Action<MelodyDot> OnDotDeleted;
    public Text FretNumText;

    private int fretNum;
    public int FretNum 
    {
        get
        {
            return IsActive ? fretNum : -1;
        }
    }
    public int SequenceIndex { get; private set; }

    public bool IsActive { get; private set; }

    private void Start()
    {
        VisContainer.SetActive(IsActive);
        HoverListener.AddHoverListener(OnHoverStateChanged);
        SetButton.onClick.AddListener(DotPressed);
        DeleteButton.onClick.AddListener(DotDeleted);
    }

    private void DotPressed()
    {
        OnDotPressed(this);
    }

    private void DotDeleted()
    {
        DeleteButton.gameObject.SetActive(false);
        OnDotDeleted(this);
    }

    public void SetActive(bool active, int fretNum = -1)
    {
        FretNumText.text = fretNum >= 0 ? fretNum.ToString() : FretNumText.text;
        IsActive = active;
        if (IsActive)
        {
            this.fretNum = fretNum;
            VisContainer.SetActive(true);
        }
    }

    private void OnHoverStateChanged(bool isHovering)
    {
        VisContainer.SetActive(isHovering || IsActive);

        if (IsActive)
        {
            DeleteButton.gameObject.SetActive(isHovering);
        }
    }

    private void OnDestroy()
    {
        HoverListener.RemoveHoverListener(OnHoverStateChanged);
    }
}
