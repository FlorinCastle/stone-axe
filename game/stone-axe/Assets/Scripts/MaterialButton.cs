using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    [SerializeField] private InventoryScript _invControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField] private int _matIndex;
    [SerializeField] private Text _matNameText;
    [SerializeField] private Text _matCountText;

    [SerializeField] private MaterialData _matRef;


    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }

    public void setMatText()
    {
        _invControl.setMatDetailText(_myButtonIndex);
    }

    public void setMatInfoText(MaterialDataStorage matInfo)
    {
        _matNameText.text = matInfo.Material;
        _matCountText.text = matInfo.MaterialCount.ToString();
    }

    public void setSelectedMat()
    {
        _invControl.setSelectedMat(_matRef);
    }

    public void setMyIndex(int i) { _myButtonIndex = i; }
    public int MyIndex { get => _myButtonIndex; }

    public void setMatIndex(int i) { _matIndex = i; }

    public MaterialData MaterialData
    {
        get => _matRef;
        set => _matRef = value;
    }
}
