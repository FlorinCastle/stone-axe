using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellItemControl : MonoBehaviour
{
    [SerializeField] private InventoryScript _invScriptRef;
    [SerializeField] private UIControl _uIControlRef;
    [SerializeField] private GameObject _selectedItem;
    private GameMaster _gameMasterRef;
    [Header("Shop UI")]
    [SerializeField] private TextMeshProUGUI _advText;
    [SerializeField] private TextMeshProUGUI _itemText;
    [SerializeField] private Button _sellItemButton;
    [SerializeField] private Button _refuseButton;
    [SerializeField] private Button _haggleButton;
    [SerializeField] private Button _suggestButton;
    [Header("Market UI")]
    [SerializeField] private TextMeshProUGUI _marketAdvText;
    [SerializeField] private TextMeshProUGUI _marketItemText;
    [SerializeField] private Button _marketSellItemButton;
    [SerializeField] private Button _marketRefuseButton;
    [SerializeField] private Button _marketHaggleButton;
    [SerializeField] private Button _marketSuggestButton;
    [Header("Modifying Skills")]
    [SerializeField] private ECO_IncSellPrice _sellPriceSkill;
    [SerializeField] private ECO_HaggleSuccess _haggleSuccessSkill;
    [SerializeField] private float _marketModifier = 0.1f;

    private bool haggleSucceded = false;
    private int sellingState = 0;

    private void Awake()
    {
        if (_invScriptRef == null)
            _invScriptRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();

        if (_uIControlRef == null)
            _uIControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>();
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        _sellItemButton.interactable = false;
        _refuseButton.interactable = false;
        _haggleButton.interactable = false;
        _suggestButton.interactable = false;
        _haggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\n(success chance: n/a)";
        _itemText.text = "choose item to sell";

        _marketSellItemButton.interactable = false;
        _marketRefuseButton.interactable = false;
        _marketHaggleButton.interactable = false;
        _marketSuggestButton.interactable = false;
        _marketHaggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\n(success chance: n/a)";
        _itemText.text = "choose item to sell";

        _advText.text = "Awaiting Adventurer Arrival";
        _marketAdvText.text = "Awaiting Adventurer Arrival";
    }

    public void suggestAlt()
    {
        _invScriptRef.setupItemInventory();
    }

    public void selectItem()
    {
        //Debug.Log("selling state " + sellingState);
        _selectedItem = _invScriptRef.getSelectedItem();
        if (_selectedItem != null)
        {
            setupDiscription();
        }
        else
        {
            Debug.Log("No Item selected!");
            _sellItemButton.interactable = false;
        }
    }
    public void selectItem(GameObject value)
    {
        if (value.GetComponent<ItemDataStorage>())
            _selectedItem = value;

        if (_selectedItem != null)
            setupDiscription();
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
        if (_selectedItem != null)
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
            if (sellingState == 0)
            {
                _itemText.text = _itemName +
                    "\nStats" + _totalStrength + _totalDex + _totalInt
                    + _materials + _totalValue;

                _sellItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "sell: " + Mathf.RoundToInt(_itemData.TotalValue * _sellPriceSkill.getModifiedSellPrice());

                _haggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\n(success chance: " + (_haggleSuccessSkill.getHaggleChance()).ToString() + "%)";
            }
            else if (sellingState == 1)
            {
                _marketItemText.text = _itemName +
                    "\nStats" + _totalStrength + _totalDex + _totalInt
                    + _materials + _totalValue;

                _marketSellItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "sell: " + Mathf.RoundToInt(_itemData.TotalValue * (_sellPriceSkill.getModifiedSellPrice() + _marketModifier));

                _marketHaggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\n(success chance: " + (_haggleSuccessSkill.getHaggleChance()).ToString() + "%)";
            }

        }
        
    }

    public void sellItem()
    {
        _itemData = _selectedItem.GetComponent<ItemDataStorage>();

        if (haggleSucceded == false)
        {
            if (sellingState == 0)
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().addCurrency(Mathf.RoundToInt(_itemData.TotalValue * _sellPriceSkill.getModifiedSellPrice()));
            else if (sellingState == 1)
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().addCurrency(Mathf.RoundToInt(_itemData.TotalValue * _sellPriceSkill.getModifiedSellPrice() + _marketModifier));
        }
        else if (haggleSucceded == true)
        {
            if (sellingState == 0)
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().addCurrency(Mathf.RoundToInt(_itemData.TotalValue * (_sellPriceSkill.getModifiedSellPrice() + _haggleSuccessSkill.getModifiedPrice())));
            else if (sellingState == 1)
                GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().addCurrency(Mathf.RoundToInt(_itemData.TotalValue * (_sellPriceSkill.getModifiedSellPrice() + _haggleSuccessSkill.getModifiedPrice()) + _marketModifier));
        }


        //_invScriptRef.RemoveItem(_itemData.InventoryIndex);
        _invScriptRef.RemoveItem(_selectedItem);

        gameObject.GetComponent<ExperienceManager>().addExperience(3);
        clearSellMenu();
        gameObject.GetComponent<DisassembleItemControl>().clearDisassembleMenu();
        gameObject.GetComponent<AdventurerMaster>().dismissAdventurers();

        if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest != null &&
            (this.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial" ||
            this.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Story"))
        {
            if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.StageType == "Sell_Item")
            {
                Debug.LogWarning("Quest Notif - Sell Done");
                _gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();

            }
        }
    }

    public void hagglePrice()
    {
        int ran = Random.Range(0, 100);
        if (ran >= Mathf.RoundToInt(_haggleSuccessSkill.getHaggleChance()))
        {
            Debug.Log("haggle fail");
            haggleSucceded = false;
        }
        else if (ran < Mathf.RoundToInt(_haggleSuccessSkill.getHaggleChance()))
        {
            Debug.Log("haggle success");
            haggleSucceded = true;
            if (sellingState == 0)
                _sellItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "sell: " + Mathf.RoundToInt(_itemData.TotalValue * (_sellPriceSkill.getModifiedSellPrice() + _haggleSuccessSkill.getModifiedPrice()));
            else if (sellingState == 1)
                _marketSellItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "sell: " + Mathf.RoundToInt(_itemData.TotalValue * (_sellPriceSkill.getModifiedSellPrice() + _haggleSuccessSkill.getModifiedPrice() + _marketModifier));
        }
        _haggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\ncomplete";
        _haggleButton.interactable = false;
        _marketHaggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\ncomplete";
        _marketHaggleButton.interactable = false;
    }
    
    public void clearSellMenu()
    {
        _itemData = null;
        _itemText.text = "choose item";
        _sellItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "sell: [price]";
        _haggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\n(success chance: n/a)";

        if (_gameMasterRef.AdventurerAtCounter == false)
            _advText.text = "Awaiting Adventurer Arrival";

        _sellItemButton.interactable = false;
        _refuseButton.interactable = false;
        _haggleButton.interactable = false;
        haggleSucceded = false;

        _marketItemText.text = "choose item";
        _marketSellItemButton.GetComponentInChildren<TextMeshProUGUI>().text = "sell: [price]";
        _marketHaggleButton.GetComponentInChildren<TextMeshProUGUI>().text = "haggle\n(success chance: n/a)";

        _marketAdvText.text = "Awaiting Adventurer Arrival";

        _marketSellItemButton.interactable = false;
        _marketRefuseButton.interactable = false;
        _marketHaggleButton.interactable = false;
        haggleSucceded = false;
    }
    //private bool itemSelected = false;
    public void adventurerAtCounter(AdventurerAI aiRef)
    {
        if (gameObject.GetComponent<GameMaster>().AdventurerAtCounter == true)
        {
            _suggestButton.interactable = true;
            _marketSuggestButton.interactable = true;

            if (aiRef != null)
            {
                _advText.text = "Adventurer [name]\n"+ aiRef.AdventurerType;
                _marketAdvText.text = "Adventurer [name]\n" + aiRef.AdventurerType;
            }
            else
            {
                _advText.text = "Awaiting Adventurer Arrival";
                _marketAdvText.text = "Awaiting Adventurer Arrival";
            }

            // get random item
            if (GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().ItemInvSize > 0)
            {
                GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().selectRandomItem();
            }
            else
            {
                if (gameObject.GetComponent<GameMaster>().MarketActive == true)
                    this.gameObject.GetComponent<AdventurerMaster>().dismissAdventurers();
            }
        }
        else if (gameObject.GetComponent<GameMaster>().AdventurerAtCounter == false)
        {
            _suggestButton.interactable = false;
            _marketSuggestButton.interactable = false;

            _advText.text = "Awaiting Adventurer Arrival";
            // clear selected item
            clearSellMenu();
        }
        canSell(gameObject.GetComponent<GameMaster>().AdventurerAtCounter);
    }
    public void canSell(bool input)
    {
        if (sellingState == 0) // shop level
        {
            //Debug.Log(_selectedItem.GetComponent<ItemDataStorage>().ItemName);
            setupDiscription();
            if (GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().ItemInvSize > 0)
            {
                _sellItemButton.interactable = input;
                _refuseButton.interactable = input;
                _haggleButton.interactable = input;
            }
            else
            {
                _sellItemButton.interactable = false;
                _refuseButton.interactable = false;
                _haggleButton.interactable = false;
            }
        }
        else if (sellingState == 1) // market level
        {
            //Debug.Log(_selectedItem.GetComponent<ItemDataStorage>().ItemName);
            setupDiscription();

            if (GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().ItemInvSize > 0)
            {
                _marketSellItemButton.interactable = input;
                _marketRefuseButton.interactable = input;
                _marketHaggleButton.interactable = input;
            }
            else
            {
                _marketSellItemButton.interactable = false;
                _marketRefuseButton.interactable = false;
                _marketHaggleButton.interactable = false;
            }
        }
    }

    public int SellingState { get => sellingState; set => sellingState = value; }
}
