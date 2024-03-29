using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveTracker : MonoBehaviour
{
    [SerializeField] private string _saveRef;
    [SerializeField] private int _indexRef;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _shopNameText;
    [SerializeField] private TextMeshProUGUI _saveNumText;
    [SerializeField] private TextMeshProUGUI _dayTimeText;
    [SerializeField] private GameObject _selectedHighlight;
    //[SerializeField] private Button _loadGameButton;
    //[SerializeField] private Button _deleteGameButton;
    private string shopName;
    private string playerName;
    [SerializeField, HideInInspector] private SoundMaster _soundControl;

    private void Awake()
    {
        _soundControl = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
    }

    public void playMouseOverSound() { _soundControl.playButtonHoverSound(); }
    public void playButtonClickSound() { _soundControl.playButtonClickSound(); }

    public void toggleSection()
    {
        if (_saveRef != "")
        {
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().selectSave(_saveRef);
            Debug.Log(this.gameObject.name + " selected");
            showHighlight();
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().saveGameSelected(true);
//            _loadGameButton.interactable = true;
//            _deleteGameButton.interactable = true;
        }
    }
    public void setupTexts()
    {
        _shopNameText.text = playerName + "\n" + shopName;
        _saveNumText.text = "Save " + Index;
    }
    public void showHighlight()
    {
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().hideHighlights();
        _selectedHighlight.SetActive(true);
    }
    public void hideHighlight() { _selectedHighlight.SetActive(false); }
    public void disableButtons() { GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().saveGameSelected(false); }

    public string SaveReference { get => _saveRef; set => _saveRef = value; }
    public int Index { get => _indexRef; set => _indexRef = value; }

    public string PlayerName { set => playerName = value; }
    public string ShopName { set => shopName = value; }
}
