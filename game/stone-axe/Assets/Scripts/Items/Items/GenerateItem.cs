using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button haggleButton;
    [SerializeField] private Button generateButton;
    private TextMeshProUGUI buyButtonText;
    private TextMeshProUGUI haggleButtonText;

    [SerializeField] private GameMaster _gameMasterRef;

    private bool haggleSucceded = false;

    //[SerializeField] InventoryScriptableObject inventoryStorage;

    private void Awake()
    {
        //_gameMaster = GameObject.FindGameObjectWithTag("GameMaster");
        buyButtonText = buyButton.GetComponentInChildren<TextMeshProUGUI>();
        haggleButtonText = haggleButton.GetComponentInChildren<TextMeshProUGUI>();
        haggleButtonText.text = "haggle\n(success chance: n/a)";
        generateButton.interactable = false;
        itemText.text = "wait for an adventurer to arrive that offers an item to buy";
    }

    [SerializeField] private ItemData _generatedItem;
    [SerializeField] private EnchantData _generatedEnchant;
    // setup for level restrictions yet
    public void GenerateRandomItem()
    {
        //Debug.LogWarning("Implement level locking here!");
        _generatedItem = itemScript.chooseItem();
        do
        {
            _generatedItem = itemScript.chooseItem();
        } while (_generatedItem.ItemLevel > _gameMasterRef.GetLevel);

        int ranEnchChance = Random.Range(0, 1000);
        if (ranEnchChance <= 100)
        {
            _generatedEnchant = enchantScript.chooseEnchant();
            _generatedItem.setIsEnchanted(true);
        }
        else
            _generatedItem.setIsEnchanted(false);

        generateItemText(true);
        itemText.text = _generatedText;
        buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _buyPriceSkill.getModifiedBuyPrice());
        buyButton.interactable = true;
        haggleButtonText.text = "haggle\n(success chance: " + (_haggleSkill.getHaggleChance()).ToString() + "%)";
        haggleButton.interactable = true;
    }
    public void GeneratePresetItem(ItemData item, MaterialData part1Mat, MaterialData part2Mat, MaterialData part3Mat, bool forceInsert)
    {
        _generatedItem = item;
        _generatedItem.Part1.Material = part1Mat;
        _generatedItem.Part2.Material = part2Mat;
        _generatedItem.Part3.Material = part3Mat;
        if (forceInsert == false)
        {
            int ranEnchChance = Random.Range(0, 1000);
            if (ranEnchChance >= 100)
            {
                _generatedEnchant = enchantScript.chooseEnchant();
                _generatedItem.setIsEnchanted(true);
            }
            else
                _generatedItem.setIsEnchanted(false);

            generateItemText(false);
            itemText.text = _generatedText;
            buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _buyPriceSkill.getModifiedBuyPrice());
            buyButton.interactable = true;
            haggleButtonText.text = "haggle\n(success chance: " + (_haggleSkill.getHaggleChance()).ToString() + "%)";
            haggleButton.interactable = true;

        }
        else if (forceInsert == true)
            _inventoryRef.InsertItem(_generatedItem);
        //_generatedItem = null;
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
        clearBuyMenu();

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
        clearBuyMenu();
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
        clearBuyMenu();
        this.gameObject.GetComponent<SellItemControl>().clearSellMenu();
    }
    public void clearBuyMenu()
    {
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
            GenerateRandomItem();
        }
        else if (gameObject.GetComponent<GameMaster>().AdventurerAtCounter == false)
        {
            generateButton.interactable = false;
            clearBuyMenu();
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

    private void generateItemText(bool randomMats)
    {
        _itemName = "Item - " + _generatedItem.ItemName;
        if (randomMats == true) _materials = "\n\nMaterials\n" + _generatedItem.RandomMaterials;
        else if (randomMats == false) _materials = "\n\nMaterials\n" +
                _generatedItem.Part1.Material.Material + "\n" +
                _generatedItem.Part2.Material.Material + "\n" +
                _generatedItem.Part3.Material.Material;

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
