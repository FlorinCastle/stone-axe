using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemControl : MonoBehaviour
{
    [SerializeField] private InventoryScript _invScriptRef;
    [SerializeField] private UIControl _uIControlRef;
    [SerializeField] private GameObject _selectedItem;
    [Header("UI")]
    [SerializeField] private Text _itemText;
    [SerializeField] private Button _sellItemButton;
    [SerializeField] private Button _refuseButton;
    [Header("Modifying Skills")]
    [SerializeField] private ECO_IncSellPrice _sellPriceSkill;

    private void Awake()
    {
        if (_invScriptRef == null)
            _invScriptRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();

        if (_uIControlRef == null)
            _uIControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>();

        _sellItemButton.interactable = false;
        _refuseButton.interactable = false;
    }

    public void suggestAlt()
    {
        _invScriptRef.setupItemInventory(true,1);
    }

    public void selectItem()
    {
        _selectedItem = _invScriptRef.getSelectedItem();
        if (_selectedItem != null)
        {
            //Debug.Log(_selectedItem.GetComponent<ItemDataStorage>().ItemName);
            setupDiscription();
            _sellItemButton.interactable = true;
            _refuseButton.interactable = true;
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

        _sellItemButton.GetComponentInChildren<Text>().text = "sell: " + Mathf.RoundToInt(_itemData.TotalValue * _sellPriceSkill.getModifiedSellPrice());

    }

    public void sellItem()
    {
        _itemData = _selectedItem.GetComponent<ItemDataStorage>();
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().addCurrency(Mathf.RoundToInt(_itemData.TotalValue * _sellPriceSkill.getModifiedSellPrice()));
        _invScriptRef.RemoveItem(_itemData.InventoryIndex);

        this.gameObject.GetComponent<ExperienceManager>().addExperience(3);
        clearSellMenu();
    }
    
    public void clearSellMenu()
    {
        _itemData = null;
        _itemText.text = "item text";
        _sellItemButton.GetComponentInChildren<Text>().text = "sell: [price]";
        _sellItemButton.interactable = false;
        _refuseButton.interactable = false;
    }
}
