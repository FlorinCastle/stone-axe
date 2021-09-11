using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private InventoryScript _invControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField] private int _itemIndex;
    
    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }

    public void setItemInfoText()
    {
        _invControl.setItemDetailText(_myButtonIndex);
    }

    public void setPartInfoText()
    {
        _invControl.setPartDetailText(_myButtonIndex);
    }

    public void setMyIndex(int i) { _myButtonIndex = i; }
    public int MyIndex { get => _myButtonIndex; }

    public void setItemIndex(int i) { _itemIndex = i; }
    public int ItemIndex { get => _itemIndex; }
}
