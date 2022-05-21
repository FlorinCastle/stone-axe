
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField] public string _playerName = "test";
    [SerializeField] public string _shopName = "test";
    [SerializeField, HideInInspector] private string _playerSpecies = "";
    [SerializeField, HideInInspector] private int _playerColor = -1;
    [SerializeField] public int _currentCurrency;
    [SerializeField] public int _totalExperience;
    [SerializeField] public int _level;
    [SerializeField] public int _currentSkillPoints;
    private bool adventurerAtCounter;
    [Header("UI and Level")]
    [SerializeField] public GameObject _shopLevel;
    [SerializeField] public GameObject _marketLevel;
    [SerializeField] public GameObject _shopSubUI;
    [SerializeField] public GameObject _marketSubUI;
    [SerializeField] public GameObject _toShopButton;
    [SerializeField] public GameObject _toMarketButton;
    private InventoryData _invData;
    [SerializeField] public InventoryScript _invScript;
    private UIControl _uiControlRef;
    [SerializeField]
    public GameObject saveTrackerParent;
    [Header("Prefabs")]
    [SerializeField] public GameObject _saveHolderPrefab;
    [Header("Save Tracking")]
    [SerializeField] public Toggle _skipTutorialQuestsToggle;
    [SerializeField] public List<SaveTracker> _saveTrackerScripts;
    [SerializeField]
    public List<string> _saveGameList;
    [SerializeField]
    public string _selectedSave;

    [SerializeField]
    public string _mostRecentSave;

    private bool skipTutorial;

    private bool buyAvailable;
    private bool sellAvailable;
    private bool disassembleAvailable;
    private bool craftAvailable;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _invData = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>();
        _invScript = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>();
        _uiControlRef = gameObject.GetComponent<UIControl>();

        loadSaveGames();
    }

    public void setSkipTutorial()
    {
        skipTutorial = _skipTutorialQuestsToggle.isOn;
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
        toggleAdventurers(false);
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

        _uiControlRef.selectActiveMarketUI();

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

        _uiControlRef.selectActiveShopUI();

        gameObject.GetComponent<SellItemControl>().SellingState = 0;

        //if (gameObject.GetComponent<QuestControl>().CurrentQuest != null && gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial")
        if (gameObject.GetComponent<QuestControl>().CurrentQuest != null)
        { if (gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType != "Tutorial")
                gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
        }
        else if (gameObject.GetComponent<QuestControl>().CurrentQuest == null)
            gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();

        if (gameObject.GetComponent<PlayerManager>().PlayerExists == false)
            gameObject.GetComponent<PlayerManager>().spawnPlayer();
    }

    public void startNewGame()
    {
        Debug.Log("GameMaster.startNewGame() - starting new game");
        //_uiControlRef.newGameUIEnabled(false);
        _uiControlRef.unloadNewGameUI();
        _uiControlRef.mainMenuEnabled(false);
        _uiControlRef.gameUIEnabled(true);
        loadShopLevel();
        setTotalExperience(0);
        if (skipTutorial == false)
        {
            gameObject.GetComponent<QuestControl>().resetAllQuests();
            _uiControlRef.shopNoTabsAccessable();
        }
        else if (skipTutorial == true)
        {
            gameObject.GetComponent<QuestControl>().skipTutorialQuests();
            _uiControlRef.shopAllTabsAccessable();
        }
        //Debug.Log("GameMaster.startNewGame(): Skipping Tutorial");
        gameObject.GetComponent<QuestControl>().setupStoryQuests();
    }
    public void startLoadedGame()
    {
        loadSelectedGame();
        _uiControlRef.loadGameUIEnabled(false);
        _uiControlRef.mainMenuEnabled(false);
        _uiControlRef.gameUIEnabled(true);
        loadShopLevel();
    }
    public void backLoad()
    {
        _playerName = "";
        _shopName = "";
        clearSelectedSave();
        _uiControlRef.loadGameUIEnabled(false);
        _uiControlRef.setupMainMenu();
    }

    public void loadShopBuyMenu()
    {
        _uiControlRef.shopEcoMenuEnabled(true);
        _uiControlRef.shopBuyMenuEnabled(true);
        _uiControlRef.disassembleMenuEnabled(false);
        _uiControlRef.shopSellMenuEnabled(false);
        _uiControlRef.craftMenuEnabled(false);

        _uiControlRef.SUI_BuySelected();

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

        _uiControlRef.SUI_SellSelected();

        _uiControlRef.BUI_InvSelected();
        _uiControlRef.IUI_ItemsSelected();

        updatePlayerPosition();
    }
    public void loadDisassembleMenu()
    {
        _uiControlRef.SUI_DisassembleSelected();
        _uiControlRef.disassembleMenuEnabled(true);
        _uiControlRef.shopEcoMenuEnabled(false);
        _uiControlRef.shopBuyMenuEnabled(false);
        _uiControlRef.shopSellMenuEnabled(false);
        _uiControlRef.craftMenuEnabled(false);

        _uiControlRef.openInvUI();
        _uiControlRef.BUI_InvSelected();
        if (_uiControlRef.InventoryItemUIEnabled == true)
        {
            _uiControlRef.openItemInv();
            _uiControlRef.IUI_ItemsSelected();
        }
        else if (_uiControlRef.InventoryPartUIEnabled == true)
        {
            _uiControlRef.openPartInv();
            _uiControlRef.IUI_PartsSelected();
        }
        else
        {
            _uiControlRef.openItemInv();
            _uiControlRef.IUI_ItemsSelected();
        }

        _uiControlRef.SUI_DisassembleSelected();

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

        _uiControlRef.SUI_CraftSelected();
        // bottom ui - recipes ui
        _uiControlRef.BUI_RecipesSelected();

        updatePlayerPosition();
    }
    public void loadMarketSellMenu()
    {
        _uiControlRef.marketSellMenuEnabled(true);
        _uiControlRef.marketQuestMenuEnabled(false);
        _uiControlRef.MUI_SellSelected();

        updatePlayerPosition();
    }
    public void loadMarketQuestBoard()
    {
        _uiControlRef.marketQuestMenuEnabled(true);
        _uiControlRef.marketSellMenuEnabled(false);
        _uiControlRef.MUI_QuestSelected();

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

    public void loadSkillMenu()
    {
        _uiControlRef.skillUIEnabled(true);
        gameObject.GetComponent<SkillManager>().updateSkillPoints();
        gameObject.GetComponent<SkillManager>().setupSkillUI();
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
        if (spawnAdvent != toggle)
        {
            spawnAdvent = toggle;
            if (toggle == true)
                gameObject.GetComponent<AdventurerMaster>().startAdventurerSpawn();
            else if (toggle == false)
                gameObject.GetComponent<AdventurerMaster>().disableAdventurerSpawn();
        }
    }
    public void toggleAdventurerSpawn()
    {
        spawnAdvent = !spawnAdvent;
        if (spawnAdvent == true)
            this.gameObject.GetComponent<AdventurerMaster>().startAdventurerSpawn();
        else if (spawnAdvent == false)
            this.gameObject.GetComponent<AdventurerMaster>().disableAdventurerSpawn();
    }
    public void adventurerEco(AdventurerAI aiRef)
    {
        if (adventurerAtCounter == true)
        {
            gameObject.GetComponent<SellItemControl>().adventurerAtCounter(aiRef);
            gameObject.GetComponent<GenerateItem>().adventurerAtCounter(aiRef);
        }
        else if (adventurerAtCounter == false)
        {
            gameObject.GetComponent<SellItemControl>().adventurerAtCounter(null);
            gameObject.GetComponent<GenerateItem>().adventurerAtCounter(null);
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

    public InventoryScript InvScriptRef { get => _invScript; }
    public bool AdventurerAtCounter { get => adventurerAtCounter; set => adventurerAtCounter = value; }
    public bool ShopActive { get => _shopLevel.activeInHierarchy; }
    public bool MarketActive { get => _marketLevel.activeInHierarchy; }

    public List<SaveTracker> SaveTrackers { get => _saveTrackerScripts; }
    public string SelectedSave { get => _selectedSave; set => _selectedSave = value; }

    public List<string> AllPlayerNames()
    {
        List<string> setPlayerNames = new List<string>();
        foreach (string savePath in _saveGameList)
            if (File.Exists(savePath))
            {
                string saveString = File.ReadAllText(savePath);
                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                setPlayerNames.Add(saveObject.playerName);
            }
        return setPlayerNames;
    }
    public List<string> AllShopNames()
    {
        List<string> setShopNames = new List<string>();
        foreach (string savePath in _saveGameList)
            if (File.Exists(savePath))
            {
                string saveString = File.ReadAllText(savePath);
                SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

                setShopNames.Add(saveObject.shopName);
            }
        return setShopNames;
    }

    public void selectSave(string save)
    {
        SelectedSave = save;
        _playerName = JsonUtility.FromJson<SaveObject>(File.ReadAllText(save)).playerName;
        _shopName = JsonUtility.FromJson<SaveObject>(File.ReadAllText(save)).shopName;
    }

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

        string currentTime = DateTime.Now.ToString();

        Debug.Log("GameMaster.saveGame().currentTime: " + currentTime);

        // put this object's data into save object
        SaveObject saveInvObj = new SaveObject
        {
            playerName = _playerName,
            shopName = _shopName,
            saveDateTime = currentTime,
            currentCurency = _currentCurrency,
            currentExp = _totalExperience,
            level = _level,
            currentSkillPoints = _currentSkillPoints,
            buyAvailable = buyAvailable,
            sellAvailable = sellAvailable,
            disassembleAvailable = disassembleAvailable,
            craftAvailable = craftAvailable,
            playerSave = savePlayer,
            inventoryObjects = itemSaveList,
            partInvObjects = partSaveList,
            enchInvObjects = enchSaveList,
            questSaveObject = questSaveList,
            skillObject = skillSave,
            materialsObject = materialsSave,
        };

        string json = JsonUtility.ToJson(saveInvObj, true);

        if (Directory.Exists(Application.persistentDataPath + "/" + _playerName + "_" + _shopName) == false)
            Directory.CreateDirectory(Application.persistentDataPath + "/" + _playerName + "_" + _shopName);


        //Debug.Log(json);
        string savePath = Application.persistentDataPath + "/" + _playerName + "_" + _shopName + "/save_" + _playerName + "_" + _shopName + "1.json";

        if (File.Exists(savePath) == true)
        {
            string savePath2 = Application.persistentDataPath + "/" + _playerName + "_" + _shopName + "/save_" + _playerName + "_" + _shopName + "2.json";
            if (File.Exists(savePath2))
            {
                string savePath3 = Application.persistentDataPath + "/" + _playerName + "_" + _shopName + "/save_" + _playerName + "_" + _shopName + "3.json";
                if (File.Exists(savePath3))
                    File.Delete(savePath3);
                File.Move(savePath2, savePath3);
            }

            File.Move(savePath, savePath2);
        }

        File.WriteAllText(savePath, json);
        if (!_saveGameList.Contains(savePath))
            _saveGameList.Add(savePath);
        saveSaveGames();
    }
    public void loadGame()
    {
        foreach (string savePath in _saveGameList)
        {
            //if (File.Exists(Application.dataPath + "/save.json"))
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
                    if (saveObject.playerName != null || saveObject.playerName != "") _playerName = saveObject.playerName;
                    else _playerName = gameObject.GetComponent<DefaultValues>().PlayerDefaultName;
                    if (saveObject.shopName != null || saveObject.shopName != "") _shopName = saveObject.shopName;
                    else _shopName = gameObject.GetComponent<DefaultValues>().ShopDefaultName;
                    
                    if (saveObject.buyAvailable == true) _uiControlRef.SUI_BuyEnabled();
                    else buyAvailable = false; _uiControlRef.SUI_BuyDisabled();

                    if (saveObject.sellAvailable == true) _uiControlRef.SUI_SellEnabled();
                    else sellAvailable = false; _uiControlRef.SUI_SellDisabled();

                    if (saveObject.disassembleAvailable == true) _uiControlRef.SUI_DisassembleEnabled();
                    else disassembleAvailable = false; _uiControlRef.SUI_DisassembleDisabled();

                    if (saveObject.craftAvailable == true) _uiControlRef.SUI_CraftEnabled();
                    else craftAvailable = false; _uiControlRef.SUI_CraftDisabled();
                    

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
                _playerName = JsonUtility.FromJson<SaveObject>(File.ReadAllText(_selectedSave)).playerName;
                _shopName = JsonUtility.FromJson<SaveObject>(File.ReadAllText(_selectedSave)).shopName;

                Debug.LogWarning("deleting save game: " + _playerName + " " + _shopName);
                _saveGameList.Remove(_selectedSave);
                if (Directory.Exists(Application.persistentDataPath + "/" + _playerName + "_" + _shopName) == true)
                    Directory.Delete(Application.persistentDataPath + "/" + _playerName + "_" + _shopName, true);
                //File.Delete(_selectedSave);
                saveSaveGames();
            }
            else
                Debug.LogError("Not a valid save path OR file does not exist!");
        }
        else
            Debug.LogError("Not a valid save path OR file does not exist!");

        _uiControlRef.setupLoadGameMenu();
        _uiControlRef.setupMainMenu();
    }

    public void continueGame()
    {
        quickLoadGame();
        _uiControlRef.gameUIEnabled(true);
        loadShopLevel();
        _uiControlRef.mainMenuEnabled(false);
        loadShopSellMenu();
    }

    public void quickLoadGame()
    {
        _selectedSave = _mostRecentSave;
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
        string savePath = Application.persistentDataPath + "/save1.json";
        if (File.Exists(savePath))
        {
            string savePath2 = Application.persistentDataPath + "/save2.json";
            if (File.Exists(savePath2))
            {
                string savePath3 = Application.persistentDataPath + "/save3.json";
                if (File.Exists(savePath3))
                {
                    string savePath4 = Application.persistentDataPath + "/save4.json";
                    if (File.Exists(savePath4))
                        File.Delete(savePath4);
                    File.Move(savePath3, savePath4);
                }
                File.Move(savePath2, savePath3);
            }
            File.Move(savePath, savePath2);
        }
        
        File.WriteAllText(savePath, json);
    }
    public void loadSaveGames()
    {
        foreach (SaveTracker st in _saveTrackerScripts)
            Destroy(st.gameObject);
        _saveTrackerScripts.Clear();

        if (File.Exists(Application.persistentDataPath + "/save1.json"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save1.json");

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
        if (File.Exists(Application.persistentDataPath + "/save1.json"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save1.json");

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

    public string getMostRecentSaveString()
    {
        if (File.Exists(Application.persistentDataPath + "/save1.json"))
        {
            Debug.Log("GameMaster.getMostRecentSaveString() - save1.json exists!");
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save1.json");

            SaveData saveObject = JsonUtility.FromJson<SaveData>(saveString);

            List<string> saveList = saveObject.saveGamePaths;

            string mostRecentSave = File.ReadAllText(saveList[0]);

            var mostRecentDateTime = DateTime.Parse(JsonUtility.FromJson<SaveObject>(mostRecentSave).saveDateTime);

            foreach (string sg_string in saveList)
            {
                var saveDateTime = DateTime.Parse(JsonUtility.FromJson<SaveObject>(File.ReadAllText(sg_string)).saveDateTime);

                if (saveDateTime >= mostRecentDateTime)
                {
                    mostRecentDateTime = saveDateTime;
                    mostRecentSave = sg_string;
                }
            }
            string ret = JsonUtility.FromJson<SaveObject>(File.ReadAllText(mostRecentSave)).playerName + " / "
                + JsonUtility.FromJson<SaveObject>(File.ReadAllText(mostRecentSave)).shopName;

            _mostRecentSave = mostRecentSave;

            return ret;
        }
        return "";
    }

    /*
    private bool buyAvailable;
    private bool sellAvailable;
    private bool disassembleAvailable;
    private bool craftAvailable;
    */
    public bool BuyAvailable { get => buyAvailable; set => buyAvailable = value; }
    public bool SellAvailable { get => sellAvailable; set => sellAvailable = value; }
    public bool DisassembleAvailable { get => disassembleAvailable; set => disassembleAvailable = value; }
    public bool CraftAvailable { get => craftAvailable; set => craftAvailable = value; }

    private class SaveData
    {
        public List<string> saveGamePaths;
    }
    private class SaveObject
    {
        public string playerName;
        public string shopName;
        public string saveDateTime;
        //public string playerSpecies;
        public int currentCurency;
        public int currentExp;
        public int level;
        public int currentSkillPoints;
        public bool buyAvailable;
        public bool sellAvailable;
        public bool disassembleAvailable;
        public bool craftAvailable;
        public PlayerSave playerSave;
        public SaveQuestsObject questSaveObject;
        public List<SaveItemObject> inventoryObjects;
        public List<SavePartObject> partInvObjects;
        public List<SaveEnchantObject> enchInvObjects;
        public SaveSkillsObject skillObject;
        public SaveMaterialsObject materialsObject;
    }
}
