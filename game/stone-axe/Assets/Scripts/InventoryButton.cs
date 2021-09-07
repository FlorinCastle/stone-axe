using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private InventoryScript _invControl;
    private int _myButtonIndex;
    
    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }

    public void setInfoText()
    {
        _invControl.setItemDetailText(_myButtonIndex);
    }

    public void setMyIndex(int i)
    {
        _myButtonIndex = i;
    }

    public int getMyIndex()
    {
        return _myButtonIndex;
    }
}
