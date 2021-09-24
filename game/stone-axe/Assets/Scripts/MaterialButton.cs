using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    [SerializeField] private InventoryScript _invControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField] private int _matIndex;
    //[SerializeField] private int _itemIndex;
    //[SerializeField] private int _partIndex;
    [SerializeField] private Text _matNameText;
    [SerializeField] private Text _matCountText;


    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }
    /*
    public void setItemInfoText() { _invControl.setItemDetailText(_myButtonIndex); }
    public void setPartInfoText() { _invControl.setPartDetailText(_myButtonIndex); }
    public void setSelectedItem() { _invControl.setSelectedItem(_itemIndex); }
    public void setSelectedPart() { _invControl.setSelectedPart(_partIndex); }*/
    public void setMatInfoText(MaterialData matInfo)
    {
        _matNameText.text = matInfo.Material;
        _matCountText.text = matInfo.MaterialCount.ToString();
    }

    public void setSelectedMat()
    {
        _invControl.setSelectedMat(_matIndex);
    }

    public void setMyIndex(int i) { _myButtonIndex = i; }
    public int MyIndex { get => _myButtonIndex; }

    /*
    public void setItemIndex(int i) { _itemIndex = i; }
    public int ItemIndex { get => _itemIndex; }

    public void setPartIndex(int i) { _partIndex = i; }
    public int PartIndex { get => _partIndex; }
    */
}
