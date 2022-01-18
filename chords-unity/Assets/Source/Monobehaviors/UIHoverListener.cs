using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverListener : MonoBehaviour
{
    private RectTransform rectTransform;
    private List<Action<bool>> listeners = new List<Action<bool>>();
    private bool isHovering;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 testPos = rectTransform.TransformPoint(
            new Vector3(rectTransform.rect.x, rectTransform.rect.y, 0f));
        Vector3 testPos2 = rectTransform.TransformPoint(
            new Vector3(rectTransform.rect.x + rectTransform.rect.width,
            rectTransform.rect.y + rectTransform.rect.height, 0f));
        Vector3 mousePos = Input.mousePosition;
        bool isMouseHovering = mousePos.x > testPos.x && mousePos.x < testPos2.x &&
            mousePos.y > testPos.y && mousePos.y < testPos2.y;

        if (isMouseHovering && !isHovering || !isMouseHovering && isHovering)
        {
            for (int i = 0, count = listeners.Count; i < count; ++i)
            {
                listeners[i](isMouseHovering);
            }
            isHovering = isMouseHovering;
        }
        //Debug.Log(testPos.ToString() + " " + testPos2.ToString() + " : " + Input.mousePosition.ToString() + " " + isMouseHovering);
    }

    public void AddHoverListener(Action<bool> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveHoverListener(Action<bool> listener)
    {
        listeners.Remove(listener);
    }

    public void OnDestroy()
    {
        listeners.Clear();
        listeners = null;
    }
}
