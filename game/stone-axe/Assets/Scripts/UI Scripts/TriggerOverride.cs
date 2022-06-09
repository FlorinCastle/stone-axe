using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TriggerOverride : EventTrigger
{
    public Button _button = null;
    public Button button { get { if (_button == null) { _button = Get(); } return _button; } }
    private ScrollRect scrollRect;
    private bool dragOrScroll;

    private void Start()
    {
        scrollRect = gameObject.GetComponentInParent<ScrollRect>();
        dragOrScroll = false;
    }
    private Button Get() { return GetComponent<Button>(); }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        base.OnPointerEnter(eventData);
        dragOrScroll = false;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (gameObject.GetComponent<Button>() != null && dragOrScroll == true)
            EventSystem.current.SetSelectedGameObject(null);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnBeginDrag(eventData);
            dragOrScroll = true;
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnDrag(eventData);
            dragOrScroll = true;
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnEndDrag(eventData);
            dragOrScroll = true;
        }
    }
    public override void OnScroll(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnScroll(eventData);
            dragOrScroll = true;
        }
    }
}
