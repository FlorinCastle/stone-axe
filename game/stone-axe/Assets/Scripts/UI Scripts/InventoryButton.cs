using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField] private InventoryScript _invControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField] private int _itemIndex;
    [SerializeField] private int _partIndex;
    [SerializeField] private int _enchIndex;
    [Header("References")]
    [SerializeField] private Image _buttonImage;
    [SerializeField] private GameObject _isNewImage;
    [SerializeField] private GameObject _forQuestImage;
    [SerializeField] private GameObject _isEnchantedImage;
    
    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }

    public void setItemInfoText() { _invControl.setItemDetailText(_itemIndex); }
    public void setPartInfoText() { _invControl.setPartDetailText(_partIndex); }
    public void setEncInfoText() { _invControl.setEnchantDetailText(_enchIndex); }

    public void setSelectedItem() { _invControl.setSelectedItem(_itemIndex); }
    public void setSelectedPart() { _invControl.setSelectedPart(_partIndex); }
    public void setSelectedEnchant() { _invControl.setSelectedEnchant(_enchIndex); }

    public void setIsNew(bool value) { _isNewImage.SetActive(value); }
    public void setIsForQuest(bool value) { _forQuestImage.SetActive(value); }
    public void setIsEnchanted(bool value) { _isEnchantedImage.SetActive(value); }

    public void setAlpha(float alpha)
    {
        var tempColor = _buttonImage.color;
        tempColor.a = alpha;
        _buttonImage.color = tempColor;
    }

    public void setMyIndex(int i) { _myButtonIndex = i; }
    public int MyIndex { get => _myButtonIndex; }

    public void setItemIndex(int i) { _itemIndex = i; }
    public int ItemIndex { get => _itemIndex; }

    public void setPartIndex(int i) { _partIndex = i; }
    public int PartIndex { get => _partIndex; }

    public void setEnchantIndex(int i) { _enchIndex = i; }
    public int EnchantIndex { get => _enchIndex; }
}
