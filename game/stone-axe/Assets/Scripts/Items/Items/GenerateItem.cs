using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] private Item itemScript;
    [SerializeField] private Enchant enchantScript;
    [SerializeField] private InventoryScript _inventoryRef;
    [SerializeField] private GameObject _inventoryControl;
    [SerializeField] private GameObject _gameMaster;
    [Header("UI")]
    [SerializeField] private Text itemText;
    [SerializeField] private Button buyButton;
    Text buyButtonText;

    //[SerializeField] InventoryScriptableObject inventoryStorage;

    private void Awake()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        buyButtonText = buyButton.GetComponentInChildren<Text>();
    }

    [SerializeField] private ItemData _generatedItem;
    [SerializeField] private EnchantData _generatedEnchant;
    public void GenerateRandomItem()
    {
        _generatedItem = itemScript.chooseItem();
        int ranEnchChance = Random.Range(0, 1000);
        if (ranEnchChance >= 100)
        {
            _generatedEnchant = enchantScript.chooseEnchant();
            _generatedItem.setIsEnchanted(true);
        }
        else
            _generatedItem.setIsEnchanted(false);

        generateItemText();
        itemText.text = _generatedText;
        buyButtonText.text = "buy: " + _generatedItem.TotalValue;
        buyButton.interactable = true;
    }

    public void buyGeneratedItem()
    {
        if (_generatedItem != null)
        {
            if (this.gameObject.GetComponent<GameMaster>().removeCurrency(_generatedItem.TotalValue))
            {
                _inventoryRef.InsertItem(_generatedItem);
            }
        }
        _generatedItem = null;
        buyButton.interactable = false;
        buyButtonText.text = "buy: [price]";
        itemText.text = "item text";
    }

    public void forceInsertItem()
    {
        if (_generatedItem != null)
        {
            _inventoryRef.InsertItem(_generatedItem);
        }
        _generatedItem = null;
        buyButton.interactable = false;
        buyButtonText.text = "buy: [price]";
        itemText.text = "item text";
    }

    public void forceDisassembleItem()
    {
        if (_generatedItem != null)
        {
            int index = _inventoryRef.InsertItem(_generatedItem);
            //Debug.Log("inserted item to disassemble at index: " + index);
            _inventoryRef.setSelectedItem(index);
            _gameMaster.GetComponent<DisassembleItemControl>().selectItem();
            _gameMaster.GetComponent<DisassembleItemControl>().disassembleItem();
        }
        _generatedItem = null;
        buyButton.interactable = false;
        buyButtonText.text = "buy: [price]";
        itemText.text = "item text";
    }

    private string _generatedText;

    private string _itemName;
    private string _materials;
    private string _totalStrenght;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;
    private bool _isEnchanted;
    private string _enchant;

    private void generateItemText()
    {
        _itemName = "Item - " + _generatedItem.ItemName;
        _materials = "\n\nMaterials\n" + _generatedItem.RandomMaterials;

        _totalStrenght = "\nStrenght: " + _generatedItem.TotalStrength;
        _totalDex = "\nDextarity: " + _generatedItem.TotalDextarity;
        _totalInt = "\nIntelegence: " + _generatedItem.TotalIntelegence;
        _totalValue = "\n\nValue: " + _generatedItem.TotalValue;
        _isEnchanted = _generatedItem.IsEnchanted;

        if (_isEnchanted) 
            _enchant = "\nitem is enchanted"; 
        else
            _enchant = "\nitem is not enchanted";

        _generatedText = "";
        _generatedText += _itemName
            + "\nStats" + _totalStrenght
            + _totalDex
            + _totalInt
            + _materials
            + _enchant
            + _totalValue;
    }

    public EnchantData getEnchantment
    {
        get => _generatedEnchant;
    }
}
