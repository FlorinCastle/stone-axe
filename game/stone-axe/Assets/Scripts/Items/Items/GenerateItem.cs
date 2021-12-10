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
    [SerializeField] private ECO_DecBuyPrice _buyPriceSkill;
    [SerializeField] private ECO_HaggleSuccess _haggleSkill;
    [Header("UI")]
    [SerializeField] private Text itemText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button haggleButton;
    [SerializeField] private Button generateButton;
    private Text buyButtonText;
    private Text haggleButtonText;

    private bool haggleSucceded = false;

    //[SerializeField] InventoryScriptableObject inventoryStorage;

    private void Awake()
    {
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        buyButtonText = buyButton.GetComponentInChildren<Text>();
        haggleButtonText = haggleButton.GetComponentInChildren<Text>();
        haggleButtonText.text = "haggle\n(success chance: n/a)";
        generateButton.interactable = false;
    }

    [SerializeField] private ItemData _generatedItem;
    [SerializeField] private EnchantData _generatedEnchant;
    public void GenerateRandomItem()
    {
        _generatedItem = itemScript.chooseItem();
        //Debug.Log("generated item: " + _generatedItem.ItemName);
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
        buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _buyPriceSkill.getModifiedBuyPrice());
        buyButton.interactable = true;
        haggleButtonText.text = "haggle\n(success chance: " + (_haggleSkill.getHaggleChance()).ToString() + "%)";
        haggleButton.interactable = true;
    }
    public void GenerateRandomEnchant()
    {
        _generatedEnchant = enchantScript.chooseEnchant();
    }
    public void buyGeneratedItem()
    {
        if (_generatedItem != null)
        {
            if (haggleSucceded == false)
            {
                if (this.gameObject.GetComponent<GameMaster>().removeCurrency(Mathf.RoundToInt(_generatedItem.TotalValue * _buyPriceSkill.getModifiedBuyPrice())))
                {
                    _inventoryRef.InsertItem(_generatedItem);
                    this.gameObject.GetComponent<ExperienceManager>().addExperience(3);
                }
            }
            else if (haggleSucceded == true)
            {
                if (this.gameObject.GetComponent<GameMaster>().removeCurrency(Mathf.RoundToInt(_generatedItem.TotalValue * (_buyPriceSkill.getModifiedBuyPrice() + _haggleSkill.getModifiedPrice()))))
                {
                    _inventoryRef.InsertItem(_generatedItem);
                    this.gameObject.GetComponent<ExperienceManager>().addExperience(3);
                }
            }
        }
        _generatedItem = null;
        buyButton.interactable = false;
        buyButtonText.text = "buy: [price]";
        itemText.text = "item text";
        haggleButton.interactable = false;
        haggleButtonText.text = "haggle\n(success chance: n/a)";
        this.gameObject.GetComponent<AdventurerMaster>().dismissAdventurers();

        if (_gameMaster.gameObject.GetComponent<QuestControl>().CurrentQuest != null &&
            (_gameMaster.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial" ||
            _gameMaster.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Story"))
        {
            if (_gameMaster.gameObject.GetComponent<QuestControl>().CurrentStage.StageType == "Buy_Item")
            {
                Debug.LogWarning("Quest Notif - Bought item from Adventurer!");
                _gameMaster.gameObject.GetComponent<QuestControl>().nextStage();
            }
        }
    }
    public void haggleGeneratedPrice()
    {
        int ran = Random.Range(0, 100);
        Debug.Log("ran is " + ran.ToString());
        if (ran >= Mathf.RoundToInt(_haggleSkill.getHaggleChance()))
        {
            Debug.Log("haggle fail");
            haggleSucceded = false;
        }
        else if (ran < Mathf.RoundToInt(_haggleSkill.getHaggleChance()))
        {
            Debug.Log("haggle success");
            haggleSucceded = true;
            buyButton.GetComponentInChildren<Text>().text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * (_buyPriceSkill.getModifiedBuyPrice() + _haggleSkill.getModifiedPrice()));
        }
        haggleButton.GetComponentInChildren<Text>().text = "haggle\ncomplete";
        haggleButton.interactable = false;
    }
    public void forceInsertItem()
    {
        if (_generatedItem != null)
            _inventoryRef.InsertItem(_generatedItem);
        _generatedItem = null;
        buyButton.interactable = false;
        buyButtonText.text = "buy: [price]";
        itemText.text = "item text";
        haggleButton.interactable = false;
        haggleButtonText.text = "haggle\n(success chance: n/a)";
    }
    public void forceInsertEnchant()
    {
        if (_generatedEnchant != null)
            _inventoryRef.InsertEnchatment(_inventoryRef.convertEnchantData(_generatedEnchant));
        _generatedEnchant = null;
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
        haggleButton.interactable = false;
        haggleButtonText.text = "haggle\n(success chance: n/a)";
    }
    public void adventurerAtCounter()
    {
        if (gameObject.GetComponent<GameMaster>().AdventurerAtCounter == true)
        {
            generateButton.interactable = true;
        }
        else if (gameObject.GetComponent<GameMaster>().AdventurerAtCounter == false)
        {
            generateButton.interactable = false;
        }
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
