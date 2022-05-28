using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    [SerializeField, HideInInspector] private InventoryScript _invControl;
    [SerializeField, HideInInspector] private SoundMaster _soundControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField] private int _matIndex;
    [SerializeField] private TextMeshProUGUI _matNameText;
    [SerializeField] private TextMeshProUGUI _matCountText;

    [SerializeField] private MaterialData _matRef;
    [SerializeField] private Image _buttonImage;


    private void Awake()
    {
        _invControl = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
        _soundControl = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
    }

    public void playMouseOverSound() { _soundControl.playButtonHoverSound(); }
    public void playButtonClickSound() { _soundControl.playButtonClickSound(); }

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

    public void setAlpha(float alpha)
    {
        var tempColor = _buttonImage.color;
        tempColor.a = alpha;
        _buttonImage.color = tempColor;
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
