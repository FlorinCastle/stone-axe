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
    public MonoBehaviour passToMonoBehavior;

    private void Start()
    {
        scrollRect = gameObject.GetComponentInParent<ScrollRect>();
    }
    private Button Get() { return GetComponent<Button>(); }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        base.OnPointerEnter(eventData);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnEndDrag(eventData);
    }
    public override void OnScroll(PointerEventData eventData)
    {
        if (scrollRect != null)
            scrollRect.OnScroll(eventData);
    }
}
