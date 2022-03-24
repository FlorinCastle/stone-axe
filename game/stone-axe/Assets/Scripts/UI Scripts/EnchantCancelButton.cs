using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantCancelButton : MonoBehaviour
{
    [SerializeField] private InventoryScript _invControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField] private int _enchIndex;
    
    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }

    public void setEncInfoText() { _invControl.setEnchantDetailText(_enchIndex); }

    public void setSelectedEnchant() { _invControl.setSelectedEnchant(_enchIndex); }

    public void setMyIndex(int i) { _myButtonIndex = i; }
    public int MyIndex { get => _myButtonIndex; }

    public void setEnchantIndex(int i) { _enchIndex = i; }
    public int EnchantIndex { get => _enchIndex; }
}
