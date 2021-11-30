
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private int _currentCurrency;
    [SerializeField] private int _totalExperience;
    [SerializeField] private int _level;
    [SerializeField] private int _currentSkillPoints;
    private bool adventurerAtCounter;
    [Header("UI and Level")]
    [SerializeField] private GameObject _shopLevel;
    [SerializeField] private GameObject _marketLevel;
    [SerializeField] private GameObject _shopSubUI;
    [SerializeField] private GameObject _marketSubUI;
    [SerializeField] private GameObject _toShopButton;
    [SerializeField] private GameObject _toMarketButton;
    private InventoryData _invData;
    private InventoryScript _invScript;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _invData = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>();
        _invScript = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
    }

    public void addCurrency(int value)
    {
        _currentCurrency += value;
    }
    public bool removeCurrency(int value)
    {
        int temp = _currentCurrency - value;

        if (temp >= 0)
        {
            _currentCurrency = temp;
            return true;
        }
        else if (temp < 0)
            return false;

        return false;
    }

    public void loadMarketLevel()
    {
        _shopLevel.SetActive(false);
        _marketLevel.SetActive(true);
        _shopSubUI.SetActive(false);
        _marketSubUI.SetActive(true);
        _toShopButton.SetActive(true);
        _toMarketButton.SetActive(false);
        this.gameObject.GetComponent<SellItemControl>().SellingState = 1;
        this.gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
    }
    public void loadShopLevel()
    {
        _shopLevel.SetActive(true);
        _marketLevel.SetActive(false);
        _shopSubUI.SetActive(true);
        _marketSubUI.SetActive(false);
        _toShopButton.SetActive(false);
        _toMarketButton.SetActive(true);
        this.gameObject.GetComponent<SellItemControl>().SellingState = 0;
        this.gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
        if (this.gameObject.GetComponent<PlayerManager>().PlayerExists == false)
            this.gameObject.GetComponent<PlayerManager>().spawnPlayer();
    }


    public void setTotalExperience(int value) { _totalExperience = value; }
    public int GetTotalExperience { get => _totalExperience; } 
    public void setLevel(int value) { _level = value; }
    public int GetLevel { get => _level; } 
    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
    public bool AdventurerAtCounter { get => adventurerAtCounter; set => adventurerAtCounter = value; }
    public bool ShopActive { get => _shopLevel.activeInHierarchy; }
    public bool MarketActive { get => _marketLevel.activeInHierarchy; }

    public void clearSavedData()
    {
        _invData.gameObject.GetComponent<InventoryScript>().forceClearItemInventory();
        _invData.gameObject.GetComponent<InventoryScript>().forceClearPartInventory();
        _invData.gameObject.GetComponent<InventoryScript>().forceClearEnchantInventory();
    }
    public void saveGame()
    {
        // get all data to save
        List<SaveItemObject> itemSaveList = new List<SaveItemObject>();
        foreach (GameObject item in _invData.ItemInventory)
        {
            if (item != null)
            {
                SaveItemObject saveData = item.GetComponent<ItemDataStorage>().SaveItem();
                itemSaveList.Add(saveData);
                // Debug.Log(json);
            }
        }

        List<SavePartObject> partSaveList = new List<SavePartObject>();
        foreach (GameObject part in _invData.PartInventory)
        {
            if (part != null)
            {
                SavePartObject saveData = part.GetComponent<PartDataStorage>().SavePart();
                partSaveList.Add(saveData);
            }
        }

        List<SaveEnchantObject> enchSaveList = new List<SaveEnchantObject>();
        foreach (GameObject enc in _invData.EnchantInventory)
        {
            if (enc != null)
            {
                SaveEnchantObject saveData = enc.GetComponent<EnchantDataStorage>().SaveEnchant();
                enchSaveList.Add(saveData);
            }
        }

        //List<SaveQuestsObject> questSaveList = new List<SaveQuestsObject>();
        // save the quest progress
        SaveQuestsObject questSaveList = this.gameObject.GetComponent<QuestControl>().saveQuests();

        SaveSkillsObject skillSave = this.GetComponent<SkillManager>().SaveSkills();

        SaveMaterialsObject materialsSave = _invData.saveMaterials();

        PlayerSave savePlayer = this.GetComponent<PlayerManager>().savePlayer();

        // put this object's data into save object
        SaveObject saveInvObj = new SaveObject
        {
            currentCurency = _currentCurrency,
            currentExp = _totalExperience,
            level = _level,
            currentSkillPoints = _currentSkillPoints,
            playerSave = savePlayer,
            inventoryObjects = itemSaveList,
            partInvObjects = partSaveList,
            enchInvObjects = enchSaveList,
            questSaveObject = questSaveList,
            skillObject = skillSave,
            materialsObject = materialsSave,
        };

        string json = JsonUtility.ToJson(saveInvObj, true);
        //Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }
    public void loadGame()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            // load the saved items & materials
            foreach (SaveItemObject item in saveObject.inventoryObjects)
            {
                _invScript.InsertItem(_invScript.convertItemData(item));
            }
            foreach (SavePartObject part in saveObject.partInvObjects)
            {
                _invScript.InsertPart(_invScript.convertPartData(part));
            }
            foreach (SaveEnchantObject ench in saveObject.enchInvObjects)
            {
                _invScript.InsertEnchatment(_invScript.convertEnchantData(ench));
            }
            _invData.loadMaterials(saveObject.materialsObject);

            // load out all the other data
            // load out assigned skill points
            this.gameObject.GetComponent<SkillManager>().LoadSkills(saveObject.skillObject);
            // load out quest data
            this.gameObject.GetComponent<QuestControl>().LoadQuests(saveObject.questSaveObject);

            // load out this gameobject's data
            _currentCurrency = saveObject.currentCurency;
            _totalExperience = saveObject.currentExp;
            _level = saveObject.level;
            _currentSkillPoints = saveObject.currentSkillPoints;
            // load out the player data
            this.gameObject.GetComponent<PlayerManager>().loadPlayerData(saveObject.playerSave);
        }
        else
            Debug.LogWarning("No save data!");
    }
    private bool spawnAdvent = false;
    public void toggleAdventurerSpawn()
    {
        spawnAdvent = !spawnAdvent;
        if (spawnAdvent == true)
            this.gameObject.GetComponent<AdventurerMaster>().startAdventurerSpawn();
        else if (spawnAdvent == false)
            this.gameObject.GetComponent<AdventurerMaster>().disableAdventurerSpawn();
    }


    private class SaveObject
    {
        public int currentCurency;
        public int currentExp;
        public int level;
        public int currentSkillPoints;
        public PlayerSave playerSave;
        public SaveQuestsObject questSaveObject;
        public List<SaveItemObject> inventoryObjects;
        public List<SavePartObject> partInvObjects;
        public List<SaveEnchantObject> enchInvObjects;
        public SaveSkillsObject skillObject;
        public SaveMaterialsObject materialsObject;
    }
}
