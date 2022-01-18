using System;
using UnityEngine;
using UnityEngine.UI;

public class MelodyDot : MonoBehaviour
{
    public UIHoverListener HoverListener;
    public Button SetButton;
    public Action<MelodyDot> OnDotPressed;
    public Text FretNumText;
    public int StringNum { get; private set; }
    public int SequenceIndex { get; private set; }

    private bool isActive;

    private void Start()
    {
        HoverListener.AddHoverListener(OnHoverStateChanged);
        SetButton.onClick.AddListener(DotPressed);
    }

    private void DotPressed()
    {
        OnDotPressed(this);
    }

    public void SetActive(bool active, int fretNum)
    {
        FretNumText.text = fretNum.ToString();
        isActive = active;
    }

    private void OnHoverStateChanged(bool isHovering)
    {
        gameObject.SetActive(isHovering || isActive);
    }

    private void OnDestroy()
    {
        HoverListener.RemoveHoverListener(OnHoverStateChanged);
    }
}
