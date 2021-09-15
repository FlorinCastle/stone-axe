using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemControl : MonoBehaviour
{
    [SerializeField] private InventoryScript _invScriptRef;
    [SerializeField] private UIControl _uIControlRef;
    [SerializeField] private GameObject _selectedItem;
    [SerializeField] private Text _itemText;
    [Header("UI")]
    [SerializeField] private Button _sellItemButton;

    private void Awake()
    {
        if (_invScriptRef == null)
            _invScriptRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();

        if (_uIControlRef == null)
            _uIControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>();
    }

    public void suggestAlt()
    {
        _invScriptRef.setupItemInventory(true);
    }

    public void selectItem()
    {
        _selectedItem = _invScriptRef.getSelectedItem();
        if (_selectedItem != null)
        {
            //Debug.Log(_selectedItem.GetComponent<ItemDataStorage>().ItemName);
            setupDiscription();
            _sellItemButton.interactable = true;
        }
        else
        {
            Debug.Log("No Item selected!");
            _sellItemButton.interactable = false;
        }

    }

    private ItemDataStorage _itemData;
    private string _itemName;
    private string _materials;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;
    private void setupDiscription()
    {
        // get reference to ItemDataStorage script
        _itemData = _selectedItem.GetComponent<ItemDataStorage>();

        // set up text strings
        _itemName = "Item - " + _itemData.ItemName;
        _materials = "\n\nMaterials\n" + _itemData.Part1.Material.Material
            + "\n" + _itemData.Part2.Material.Material
            + "\n" + _itemData.Part3.Material.Material;
        _totalStrength = "\nStrenght: " + _itemData.TotalStrength;
        _totalDex = "\nDextarity: " + _itemData.TotalDextarity;
        _totalInt = "\nIntelegence: " + _itemData.TotalIntelegence;
        _totalValue = "\n\nValue: " + _itemData.TotalValue;

        // organize the texts
        _itemText.text = _itemName +
            "\nStats" + _totalStrength + _totalDex + _totalInt
            + _materials + _totalValue;

    }

    public void sellItem()
    {
        _itemData = _selectedItem.GetComponent<ItemDataStorage>();
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().addCurrency(_itemData.TotalValue);
        _invScriptRef.RemoveItem(_itemData.InventoryIndex);
    }
}
