
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private string _playerName = "test";
    [SerializeField] private string _shopName = "test";
    [SerializeField, HideInInspector] private string _playerSpecies = "";
    [SerializeField, HideInInspector] private int _playerColor = -1;
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
    [SerializeField] private InventoryScript _invScript;
    private UIControl _uiControlRef;
    [SerializeField]
    private GameObject saveTrackerParent;
    [Header("Prefabs")]
    [SerializeField] private GameObject _saveHolderPrefab;
    [Header("Save Tracking")]
    [SerializeField] private List<SaveTracker> _saveTrackerScripts;
    [SerializeField]
    private List<string> _saveGameList;
    [SerializeField]
    private string _selectedSave;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _invData = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>();
        _invScript = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
        _uiControlRef = gameObject.GetComponent<UIControl>();

        loadSaveGames();
    }

    public int CurrentCurrency { get => _currentCurrency; }
    public void addCurrency(int value)
    {
        _currentCurrency += value;
        this.gameObject.GetComponent<UIControl>().updateCurrencyText();
    }
    public bool removeCurrency(int value)
    {
        int temp = _currentCurrency - value;

        if (temp >= 0)
        {
            _currentCurrency = temp;
            this.gameObject.GetComponent<UIControl>().updateCurrencyText();
            return true;
        }
        else if (temp < 0)
            return false;
        return false;
    }

    public void marketAccessable(bool input)
    {
        gameObject.GetComponent<UIControl>().marketAccessable(input);
        if (input == false)
            loadShopLevel();
    }

    public void loadMainMenu()
    {
        _uiControlRef.optionsUIEnabled(false);
        _shopLevel.SetActive(false);
        _uiControlRef.mainMenuEnabled(true);
        _uiControlRef.gameUIEnabled(false);
        saveGame();
        _uiControlRef.mainMenu();
    }

    public void shopToMarket()
    {
        loadMarketLevel();
        gameObject.GetComponent<SellItemControl>().clearSellMenu();
        updatePlayerPosition();
    }
    public void MarketToShop()
    {
        loadShopLevel();
        gameObject.GetComponent<SellItemControl>().clearSellMenu();
        updatePlayerPosition();
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

        //if (gameObject.GetComponent<QuestControl>().CurrentQuest != null && gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial")
        if (gameObject.GetComponent<QuestControl>().CurrentQuest != null)
        { if (gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType != "Tutorial")
                this.gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
        }
        else if (gameObject.GetComponent<QuestControl>().CurrentQuest == null)
            this.gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();

        if (this.gameObject.GetComponent<PlayerManager>().PlayerExists == false)
            this.gameObject.GetComponent<PlayerManager>().spawnPlayer();
    }

    public void loadShopBuyMenu()
    {
        _uiControlRef.shopEcoMenuEnabled(true);
        _uiControlRef.shopBuyMenuEnabled(true);
        _uiControlRef.disassembleMenuEnabled(false);
        _uiControlRef.shopSellMenuEnabled(false);
        _uiControlRef.craftMenuEnabled(false);

        updatePlayerPosition();
    }
    public void loadShopSellMenu()
    {
        _uiControlRef.shopEcoMenuEnabled(true);
        _uiControlRef.shopSellMenuEnabled(true);
        _uiControlRef.disassembleMenuEnabled(false);
        _uiControlRef.shopBuyMenuEnabled(false);
        _uiControlRef.craftMenuEnabled(false);

        _uiControlRef.openItemInv();

        updatePlayerPosition();
    }
    public void loadDisassembleMenu()
    {
        _uiControlRef.disassembleMenuEnabled(true);
        _uiControlRef.shopEcoMenuEnabled(false);
        _uiControlRef.shopBuyMenuEnabled(false);
        _uiControlRef.shopSellMenuEnabled(false);
        _uiControlRef.craftMenuEnabled(false);

        _uiControlRef.openInvUI();
        if (_uiControlRef.InventoryItemUIEnabled == true)
            _uiControlRef.openItemInv();
        else if (_uiControlRef.InventoryPartUIEnabled == true)
            _uiControlRef.openPartInv();
        else
            _uiControlRef.openItemInv();

        updatePlayerPosition();
    }
    public void loadCraftMenu()
    {
        _uiControlRef.craftMenuEnabled(true);
        _uiControlRef.disassembleMenuEnabled(false);
        _uiControlRef.shopEcoMenuEnabled(false);
        _uiControlRef.shopBuyMenuEnabled(false);
        _uiControlRef.shopSellMenuEnabled(false);

        _uiControlRef.openRecipesUI();

        updatePlayerPosition();
    }
    public void loadMarketSellMenu()
    {
        _uiControlRef.marketSellMenuEnabled(true);
        _uiControlRef.marketQuestMenuEnabled(false);

        updatePlayerPosition();
    }
    public void loadMarketQuestBoard()
    {
        _uiControlRef.marketQuestMenuEnabled(true);
        _uiControlRef.marketSellMenuEnabled(false);

        updatePlayerPosition();
    }

    public void loadQuestMenu()
    {
        _uiControlRef.questUIEnabled(true);
        gameObject.GetComponent<QuestControl>().setupText();
    }
    public void unloadQuestMenu()
    {
        _uiControlRef.questUIEnabled(false);
    }

    public void updateLevelLocks()
    {
        if (gameObject.GetComponent<QuestControl>().CurrentQuest != null)
            Debug.Log("temp");
        else
            GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>().setupRecipeGrid();
    }
    public void updatePlayerPosition()
    {
        if (this.gameObject.GetComponent<UIControl>().ShopEcoUIEnabled == true)
            this.gameObject.GetComponent<PlayerManager>().warpToCounter();
        if (this.gameObject.GetComponent<UIControl>().ShopDisUIEnabled == true)
            this.gameObject.GetComponent<PlayerManager>().warpToDisassemble();
        if (this.gameObject.GetComponent<UIControl>().ShopCraftUIEnabled == true)
            this.gameObject.GetComponent<PlayerManager>().warpToCraft();
        if (this.gameObject.GetComponent<UIControl>().MarketEcoUIEnabled == true)
            this.gameObject.GetComponent<PlayerManager>().warpToStall();
        if (this.gameObject.GetComponent<UIControl>().MarketQuestUIEnabled == true)
            this.gameObject.GetComponent<PlayerManager>().warpToQuestBoard();

    }

    private bool spawnAdvent = false;
    public void toggleAdventurers(bool toggle)
    {
        spawnAdvent = toggle;
        if (toggle == true)
            gameObject.GetComponent<AdventurerMaster>().startAdventurerSpawn();
        else if (toggle == false)
            gameObject.GetComponent<AdventurerMaster>().disableAdventurerSpawn();
    }
    public void toggleAdventurerSpawn()
    {
        spawnAdvent = !spawnAdvent;
        if (spawnAdvent == true)
            this.gameObject.GetComponent<AdventurerMaster>().startAdventurerSpawn();
        else if (spawnAdvent == false)
            this.gameObject.GetComponent<AdventurerMaster>().disableAdventurerSpawn();
    }
    public void adventurerEco()
    {
        if (adventurerAtCounter == true)
        {
            gameObject.GetComponent<SellItemControl>().adventurerAtCounter();
            gameObject.GetComponent<GenerateItem>().adventurerAtCounter();
        }
        else if (adventurerAtCounter == false)
        {
            gameObject.GetComponent<SellItemControl>().adventurerAtCounter();
            gameObject.GetComponent<GenerateItem>().adventurerAtCounter();
        }
        else
            Debug.LogWarning("GameMaster.adventurerEco(): something broke!");
    }

    public string PlayerName { get => _playerName; set => _playerName = value; }
    public string ShopName { get => _shopName; set => _shopName = value; }
    public string PlayerSpecies { get => _playerSpecies; set => _playerSpecies = value; }
    public int PlayerColor { get => _playerColor; set => _playerColor = value; }

    public void setTotalExperience(int value) { _totalExperience = value; }
    public int GetTotalExperience { get => _totalExperience; }
    public void setLevel(int value) { _level = value; }
    public int GetLevel { get => _level; }
    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
    public bool AdventurerAtCounter { get => adventurerAtCounter; set => adventurerAtCounter = value; }
    public InventoryScript InvScriptRef { get => _invScript; }
    public bool ShopActive { get => _shopLevel.activeInHierarchy; }
    public bool MarketActive { get => _marketLevel.activeInHierarchy; }
    public List<SaveTracker> SaveTrackers { get => _saveTrackerScripts; }
    public string SelectedSave { get => _selectedSave; set => _selectedSave = value; }

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
            playerName = _playerName,
            shopName = _shopName,
            playerSpecies = _playerSpecies,
            //playerColor = _playerColor,
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
        string savePath = Application.persistentDataPath + "/save_" + _playerName + "_" + _shopName + ".txt";
        File.WriteAllText(savePath, json);
        if (!_saveGameList.Contains(savePath))
            _saveGameList.Add(savePath);
        saveSaveGames();
    }
    public void loadGame()
    {
        foreach (string savePath in _saveGameList)
        {
            //if (File.Exists(Application.dataPath + "/save.txt"))
            if (_selectedSave != "" && savePath == _selectedSave)
            {
                if (File.Exists(savePath))
                {
                    string saveString = File.ReadAllText(savePath);

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

                    //  load out all the other data
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
                    //Debug.Log(saveObject.playerSave.playerHead);
                    this.gameObject.GetComponent<PlayerManager>().loadPlayerData(saveObject.playerSave);
                    Debug.Log("loaded save: " + saveObject.playerName + " " + saveObject.shopName);
                }
                else
                    Debug.LogWarning("No save data!");
            }
            else
                Debug.LogWarning("No save selected!");
        }
    }
    public void deleteSaveGame()
    {
        if (File.Exists(_selectedSave))
        {
            if (_saveGameList.Contains(_selectedSave))
            {
                Debug.LogWarning("deleting file: " + _selectedSave);
                _saveGameList.Remove(_selectedSave);
                File.Delete(_selectedSave);
                saveSaveGames();
            }
            else
                Debug.LogWarning("Not a valid save path OR file does not exist!");
        }
        else
            Debug.LogWarning("Not a valid save path OR file does not exist!");
    }

    public void quickLoadGame()
    {
        _selectedSave = _saveGameList[0];
        loadGame();
    }
    public void loadSelectedGame() { loadGame(); }

    public void saveSaveGames()
    {
        SaveData saveObj = new SaveData
        {
            saveGamePaths = _saveGameList,
        };
        string json = JsonUtility.ToJson(saveObj, true);
        string savePath = Application.persistentDataPath + "/save.txt";
        File.WriteAllText(savePath, json);
    }
    public void loadSaveGames()
    {
        foreach (SaveTracker st in _saveTrackerScripts)
            Destroy(st.gameObject);
        _saveTrackerScripts.Clear();

        if (File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save.txt");

            SaveData saveObject = JsonUtility.FromJson<SaveData>(saveString);

            _saveGameList = saveObject.saveGamePaths;
            // store the paths of the saved games in the save trackers 
            int i = 0;
            foreach (string sg_string in _saveGameList)
            {
                //_saveTrackerScripts[i].SaveReference = sg_string;
                i++;
                GameObject stTemp = Instantiate(_saveHolderPrefab, saveTrackerParent.transform);
                stTemp.GetComponent<SaveTracker>().SaveReference = sg_string;
                stTemp.GetComponent<SaveTracker>().Index = i;
                _saveTrackerScripts.Add(stTemp.GetComponent<SaveTracker>());
            }
        }
        else
            Debug.LogWarning("No save game data!");
    }

    public bool checkIfAnySavesExist()
    {
        if (File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save.txt");

            SaveData saveObject = JsonUtility.FromJson<SaveData>(saveString);
            List<string> saveList = saveObject.saveGamePaths;
            if (saveList.Count > 0)
                return true;
            else
                return false;
        }
        else { Debug.LogWarning("No save game data exists!"); return false; }
    }

    public void clearSelectedSave()
    {
        SelectedSave = "";
        foreach(SaveTracker st in _saveTrackerScripts)
        {
            st.hideHighlight();
            st.disableButtons();
        }
    }

    public string playerNameFromSaveString(string saveReference)
    {
        string saveString = File.ReadAllText(saveReference);
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        return saveObject.playerName;
    }
    public string shopNameFromSaveString(string saveReference)
    {
        string saveString = File.ReadAllText(saveReference);
        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        return saveObject.shopName;
    }


    private class SaveData
    {
        public List<string> saveGamePaths;
    }
    private class SaveObject
    {
        public string playerName;
        public string shopName;
        public string playerSpecies;
        //public int playerColor;
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
