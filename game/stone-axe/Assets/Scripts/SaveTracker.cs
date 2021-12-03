using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTracker : MonoBehaviour
{
    [SerializeField] private string _saveRef;
    [SerializeField] private int _indexRef;
    [Header("UI Elements")]
    [SerializeField] private Text _shopNameText;
    [SerializeField] private Text _dayTimeText;
    [SerializeField] private GameObject _selectedHighlight;
    private string shopName;
    private string playerName;

    public void toggleSection()
    {
        if (_saveRef != "")
        {
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().SelectedSave = _saveRef;
            Debug.Log(this.gameObject.name + " selected");
            showHighlight();
        }
    }
    public void setupTexts()
    {
        _shopNameText.text = playerName + "\n" + shopName;
    }
    public void showHighlight()
    {
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().hideHighlights();
        _selectedHighlight.SetActive(true);
    }
    public void hideHighlight() { _selectedHighlight.SetActive(false); }

    public string SaveReference { get => _saveRef; set => _saveRef = value; }
    public int Index { get => _indexRef; }

    public string PlayerName { set => playerName = value; }
    public string ShopName { set => shopName = value; }
}
