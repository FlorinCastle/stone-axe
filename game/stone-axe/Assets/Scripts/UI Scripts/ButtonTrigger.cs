using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTrigger : MonoBehaviour, IPointerEnterHandler
{
    public Button _button = null;
    public Button button { get { if (_button == null) { _button = Get(); } return _button; } }

    private Button Get() { return GetComponent<Button>(); }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        OnPointerEnter(eventData);
    }
}
