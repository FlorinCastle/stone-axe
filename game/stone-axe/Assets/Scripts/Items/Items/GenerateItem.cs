using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GenerateItem : MonoBehaviour
{
    [SerializeField] private Item itemScript;
    [SerializeField] private Part partScript;
    [SerializeField] private Enchant enchantScript;
    [SerializeField] private InventoryScript _inventoryRef;
    [SerializeField] private GameObject _inventoryControl;
    [SerializeField] private GameObject _gameMaster;
    [SerializeField] private SkillManager _skillManager;
    //[SerializeField] private ECO_DecBuyPrice _buyPriceSkill;
    //[SerializeField] private ECO_HaggleSuccess _haggleSkill;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI advText;
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
        advText.text = "Awaiting Adventurer Arrival";
    }

    [SerializeField] private ItemData _generatedItem;
    [SerializeField] private ItemJsonData _generatedItemJson;
    [SerializeField] private EnchantData _generatedEnchant;
    // setup for level restrictions yet
    public void GenerateRandomItem()
    {
        // get a random item that is level complient
        _generatedItem = itemScript.chooseItem();
        do
        {
            _generatedItem = itemScript.chooseItem();
        } while (_generatedItem.ItemLevel > _gameMasterRef.GetLevel);

        // get materials
        int ranMatP1Int = 0;
        do
        {
            ranMatP1Int = Random.Range(0, _generatedItem.Part1.ValidMaterialData.Count);
            _generatedItem.Part1.Material = _generatedItem.Part1.ValidMaterialData[ranMatP1Int];
        } while (_generatedItem.Part1.ValidMaterialData[ranMatP1Int].LevelRequirement > _gameMasterRef.GetLevel);
        int ranMatP2Int = 0;
        do
        {
            ranMatP2Int = Random.Range(0, _generatedItem.Part2.ValidMaterialData.Count);
            _generatedItem.Part2.Material = _generatedItem.Part2.ValidMaterialData[ranMatP2Int];
        } while (_generatedItem.Part2.ValidMaterialData[ranMatP2Int].LevelRequirement > _gameMasterRef.GetLevel);
        int ranMatP3Int = 0;
        do
        {
            ranMatP3Int = Random.Range(0, _generatedItem.Part3.ValidMaterialData.Count);
            _generatedItem.Part3.Material = _generatedItem.Part3.ValidMaterialData[ranMatP3Int];
        } while (_generatedItem.Part3.ValidMaterialData[ranMatP3Int].LevelRequirement > _gameMasterRef.GetLevel);


        _generatedItem.Part1.Material = _generatedItem.Part1.ValidMaterialData[ranMatP1Int];
        _generatedItem.Part2.Material = _generatedItem.Part2.ValidMaterialData[ranMatP2Int];
        _generatedItem.Part3.Material = _generatedItem.Part3.ValidMaterialData[ranMatP3Int];


        // get chance of item being enchanted
        int ranEnchChance = Random.Range(0, 1000);
        //Debug.LogWarning("TODO - GenerateItem.GenerateRandomItem(): ADD CODE FOR BOOSTED ENCHANT CHANCE AROUND ABOUT HERE!");
        if (ranEnchChance <= (100 + _skillManager.EnchantChanceRef.getAddedEnchChance()))
        {
            _generatedEnchant = enchantScript.chooseEnchant();
            _generatedItem.setIsEnchanted(true);
        }
        else
            _generatedItem.setIsEnchanted(false);

        generateItemText();
        itemText.text = _generatedText;
        buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice());
        buyButton.interactable = true;
        haggleButtonText.text = "haggle\n(success chance: " + (_skillManager.HagglePriceRef.getHaggleChance()).ToString() + "%)";
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
            //Debug.LogWarning("TODO - GenerateItem.GeneratePresetItem(): ADD CODE FOR BOOSTED ENCHANT CHANCE AROUND ABOUT HERE!");
            if (ranEnchChance <= (100 + _skillManager.EnchantChanceRef.getAddedEnchChance()))
            {
                _generatedEnchant = enchantScript.chooseEnchant();
                _generatedItem.setIsEnchanted(true);
            }
            else
                _generatedItem.setIsEnchanted(false);

            generateItemText();
            itemText.text = _generatedText;
            buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice());
            buyButton.interactable = true;
            haggleButtonText.text = "haggle\n(success chance: " + (_skillManager.HagglePriceRef.getHaggleChance()).ToString() + "%)";

            advText.text = "Awaiting Adventurer Arrival";

            haggleButton.interactable = true;
        }
        else if (forceInsert == true)
            _inventoryRef.InsertItem(_generatedItem);
        //_generatedItem = null;
    }
    public void GeneratePresetItem(ItemJsonData item, MaterialData part1Mat, MaterialData part2Mat, MaterialData part3Mat, bool forceInsert)
    {
        _generatedItemJson = item;
        _generatedItemJson.parts.Add(partScript.getPartJsonData(item.requiredParts[0]));
        _generatedItemJson.parts[0].materialData = part1Mat;
        _generatedItemJson.parts.Add(partScript.getPartJsonData(item.requiredParts[1]));
        _generatedItemJson.parts[1].materialData = part2Mat;
        _generatedItemJson.parts.Add(partScript.getPartJsonData(item.requiredParts[2]));
        _generatedItemJson.parts[2].materialData = part3Mat;
        if (forceInsert == false)
        {
            int ranEnchChance = Random.Range(0, 100);
            if (ranEnchChance <= (100 + _skillManager.EnchantChanceRef.getAddedEnchChance()))
            {
                _generatedEnchant = enchantScript.chooseEnchant();
                _generatedItemJson.ench = _generatedEnchant;
                _generatedItemJson.isEnchanted = true;
            }
            else
                _generatedItemJson.isEnchanted = false;

            generateItemTextJson();
            itemText.text = _generatedText;

            buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice());
            buyButton.interactable = true;
            haggleButtonText.text = "haggle\n(success chance: " + (_skillManager.HagglePriceRef.getHaggleChance()).ToString() + "%)";

            advText.text = "Awaiting Adventurer Arrival";

            haggleButton.interactable = true;
        }
        else if (forceInsert == true)
            _inventoryRef.InsertItem(_generatedItemJson);
    }
    public void GeneratePresetItem(ItemJsonData item, List<MaterialData> mats, bool forceInsert)
    {
        if (item.requiredParts.Count == mats.Count)
        {
            int i = 0;
            _generatedItemJson = item;
            foreach (string part in item.requiredParts)
            {
                if (partScript.getPartJsonData(part).validMaterials.Contains(mats[i].Material))
                {
                    PartJsonData partJson = partScript.getPartJsonData(part);
                    _generatedItemJson.parts.Add(partJson);
                    partJson.materialData = mats[i];
                }
                else
                {
                    Debug.LogWarning("GenerateItem.GeneratePresetItem(ItemJsonData, List<MaterialData>, bool): Cannot generate item with input data!");
                    break;
                }
                i++;
            }
            if (forceInsert == false)
            {
                int ranEnchChance = Random.Range(0, 100);
                if (ranEnchChance <= (100 + _skillManager.EnchantChanceRef.getAddedEnchChance()))
                {
                    _generatedEnchant = enchantScript.chooseEnchant();
                    _generatedItemJson.ench = _generatedEnchant;
                    _generatedItemJson.isEnchanted = true;
                }
                else
                    _generatedItemJson.isEnchanted = false;

                generateItemTextJson();
                itemText.text = _generatedText;

                buyButtonText.text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * _skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice());
                buyButton.interactable = true;
                haggleButtonText.text = "haggle\n(success chance: " + (_skillManager.HagglePriceRef.getHaggleChance()).ToString() + "%)";

                advText.text = "Awaiting Adventurer Arrival";

                haggleButton.interactable = true;
            }
            else if (forceInsert == true)
                _inventoryRef.InsertItem(_generatedItemJson);
        }
        else
            Debug.LogWarning("GenerateItem.GeneratePresetItem(ItemJsonData, List<MaterialData>, bool): Cannot generate item with input data!");
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
                if (gameObject.GetComponent<GameMaster>().removeCurrency(Mathf.RoundToInt(_generatedItem.TotalValue * _skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice())))
                {
                    _inventoryRef.InsertItem(_generatedItem);
                    gameObject.GetComponent<ExperienceManager>().addExperience(3);
                }
            }
            else if (haggleSucceded == true)
            {
                if (gameObject.GetComponent<GameMaster>().removeCurrency(Mathf.RoundToInt(_generatedItem.TotalValue * (_skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice() + _skillManager.HagglePriceRef.getModifiedPrice()))))
                {
                    _inventoryRef.InsertItem(_generatedItem);
                    gameObject.GetComponent<ExperienceManager>().addExperience(3);
                }
            }
        }
        clearBuyMenu();

        gameObject.GetComponent<AdventurerMaster>().dismissAdventurers();

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
        if (ran >= Mathf.RoundToInt(_skillManager.HagglePriceRef.getHaggleChance()))
        {
            Debug.Log("haggle fail");
            haggleSucceded = false;
        }
        else if (ran < Mathf.RoundToInt(_skillManager.HagglePriceRef.getHaggleChance()))
        {
            Debug.Log("haggle success");
            haggleSucceded = true;
            buyButton.GetComponentInChildren<Text>().text = "buy: " + Mathf.RoundToInt(_generatedItem.TotalValue * (_skillManager.DecreaseBuyPriceRef.getModifiedBuyPrice() + _skillManager.HagglePriceRef.getModifiedPrice()));
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
        gameObject.GetComponent<SellItemControl>().clearSellMenu();
    }
    public void clearBuyMenu()
    {
        _generatedItem = null;
        buyButton.interactable = false;
        buyButtonText.text = "buy: [price]";
        itemText.text = "wait for an adventurer to arrive that offers an item to buy";
        haggleButton.interactable = false;
        haggleButtonText.text = "haggle\n(success chance: n/a)";
        advText.text = "Awaiting Adventurer Arrival";
    }
    public void adventurerAtCounter(AdventurerAI aiRef)
    {
        if (gameObject.GetComponent<GameMaster>().AdventurerAtCounter == true)
        {
            generateButton.interactable = true;
            advText.text = "Adventurer [name]\n" + aiRef.AdventurerType;

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

    private void generateItemText()
    {
        _itemName = "Item - " + _generatedItem.ItemName;

        _materials = "\n\nMaterials\n" +
            _generatedItem.Part1.Material.Material + "\n" +
            _generatedItem.Part2.Material.Material + "\n" +
            _generatedItem.Part3.Material.Material;

        _totalStrenght = "\nStrength: " + _generatedItem.TotalStrength;
        _totalDex = "\nDexterity: " + _generatedItem.TotalDextarity;
        _totalInt = "\nIntelligence: " + _generatedItem.TotalIntelegence;
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
    private void generateItemTextJson()
    {
        _itemName = "Item - " + _generatedItemJson.itemName;

        _materials = "\n\nMaterials\n" +
            _generatedItemJson.parts[0].materialData.Material + "\n" +
            _generatedItemJson.parts[1].materialData.Material + "\n" +
            _generatedItemJson.parts[2].materialData.Material;

        _totalStrenght = "\nStrength: " + calculateTotalStr();
        _totalDex = "\nDexterity: " + calculateTotalDex();
        _totalInt = "\nIntelligence: " + calculateTotalInt();
        _totalValue = "\n\nValue: " + calculateTotalVal();
        _isEnchanted = _generatedItemJson.isEnchanted;

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

    private int calculateTotalStr()
    {
        return _generatedItemJson.baseStrength + _generatedItemJson.parts[0].baseStrength + _generatedItemJson.parts[1].baseStrength + _generatedItemJson.parts[2].baseStrength;
    }
    private int calculateTotalDex()
    {
        return _generatedItemJson.baseDextarity + _generatedItemJson.parts[0].baseDextarity + _generatedItemJson.parts[1].baseDextarity + _generatedItemJson.parts[2].baseDextarity;
    }
    private int calculateTotalInt()
    {
        return _generatedItemJson.baseIntelligence + _generatedItemJson.parts[0].baseIntelligence + _generatedItemJson.parts[1].baseIntelligence + _generatedItemJson.parts[2].baseIntelligence;
    }
    private int calculateTotalVal()
    {
        return _generatedItemJson.baseCost +
            (_generatedItemJson.parts[0].baseCost + (_generatedItemJson.parts[0].materialData.BaseCostPerUnit * _generatedItemJson.parts[0].unitsOfMaterialNeeded)) +
            (_generatedItemJson.parts[1].baseCost + (_generatedItemJson.parts[1].materialData.BaseCostPerUnit * _generatedItemJson.parts[1].unitsOfMaterialNeeded)) +
            (_generatedItemJson.parts[2].baseCost + (_generatedItemJson.parts[2].materialData.BaseCostPerUnit * _generatedItemJson.parts[2].unitsOfMaterialNeeded));
    }
}
