using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public enum dataSection { UI, ShopUI, ButtonGroupShopUI, ButtonGroupMarketUI, ButtonGroupBottomUI, ButtonGroupInvUI, MarketUI, MainMenuUI, LoadGameUI, NewGameUI};
    public dataSection selectSection;

    [Header("UI")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameShopUI;
    [SerializeField] private GameObject optionsPopup;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject marketUI;
    [Header("Shop UI")]
    [SerializeField] private TextMeshProUGUI _currencyText;
    [SerializeField] private GameObject economicSubUI;
    [SerializeField] private GameObject disassembleSubUI;
    [SerializeField] private GameObject craftSubUI;
    [SerializeField] private GameObject itemCraftingUI;
    [SerializeField] private GameObject partCraftingUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject receipeUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject questUI;
    [SerializeField] private GameObject shopSellMenu;
    [SerializeField] private GameObject shopBuyMenu;
    [SerializeField] private GameObject miniGameUI;
    [SerializeField] private GameObject questDetailUI;
    [SerializeField] private GameObject dialogeUI;
    [SerializeField] private Button _toMarketButton;
    /*[Space(5)]
    [SerializeField] private Button _buyTabButton;
    [SerializeField] private Button _sellTabButton;
    [SerializeField] private Button _disassembleTabButton;
    [SerializeField] private Button _craftTabButton; */
    [Space(5)]
    [SerializeField] private GameObject itemsScrollView;
    [SerializeField] private TextMeshProUGUI _itemsSortText;
    [SerializeField] private GameObject partsScrollView;
    [SerializeField] private TextMeshProUGUI _partsSortText;
    [SerializeField] private GameObject matsScrollView;
    [SerializeField] private TextMeshProUGUI _matsSortText;
    [SerializeField] private GameObject enchantsScrollView;
    [SerializeField] private TextMeshProUGUI _enchantsSortText;
    [Header("Button Group - ShopUI")]
    [SerializeField] private ButtonGroup _shopUIGroupScript;
    [SerializeField] private Button _buyUIButton;
    [SerializeField] private Button _sellUIButton;
    [SerializeField] private Button _disaUIButton;
    [SerializeField] private Button _craftUIButton;
    [Header("Button Group - MarketUI")]
    [SerializeField] private ButtonGroup _marketUIGroupScript;
    [SerializeField] private Button _mSellUIButton;
    [SerializeField] private Button _mQuestUIButton;
    [Header("ButtonGroup - BottomUI")]
    [SerializeField] private ButtonGroup _bottomUIGroupScript;
    [SerializeField] private Button _invUIButton;
    [SerializeField] private Button _recipesUIButton;
    [Header("ButtonGroup - InvUI")]
    [SerializeField] private ButtonGroup _invUIGroupScript;
    [SerializeField] private Button _itemsUIButton;
    [SerializeField] private Button _partsUIButton;
    [SerializeField] private Button _matsUIButton;
    [SerializeField] private Button _enchUIButton;
    [Header("Market UI")]
    [SerializeField] private GameObject marketEconomicSubUI;
    [SerializeField] private GameObject questSubUI;
    [Header("Main Menu UI")]
    [SerializeField] private GameObject _mainUIElements;
    [SerializeField] private GameObject _creditsUI;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private TextMeshProUGUI _continueText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private GameObject _loadGameMenu;
    [SerializeField] private GameObject _newGameMenu;
    [Header("Load Game UI")]
    [SerializeField] private Button _loadGameUIButton;
    [SerializeField] private Button _deleteGameUIButton;
    [Header("New Game UI")]
    [SerializeField] private InputField _playerName;
    [SerializeField] private InputField _shopName;
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private GameObject _isNotValidUI;
    [SerializeField, HideInInspector] private string playerSpecies = "";
    [SerializeField, HideInInspector] private int playerColor = -1;
    [SerializeField] private TMP_Dropdown _playerSpeciesDropdown;
    [SerializeField] private GameObject _colorMenu;
    [SerializeField] private GameObject _colorMenuParent;
    [SerializeField] private GameObject _colorSamplePrefab;
    private List<string> speciesOptions;
    private List<Color32> playerColors;
    [SerializeField, HideInInspector] private List<GameObject> colorButtonRefs;

    private void Awake()
    {
        //gameObject.GetComponent<GameMaster>().marketAccessable(false);
        // check if ui objects have something assigned
        if (mainMenuUI == null)
            Debug.LogError("Main Menu UI is not assigned");
        if (gameShopUI == null)
            Debug.LogError("Game Shop UI is not assigned");
        if (optionsPopup == null)
            Debug.LogError("Options Popup is not assigned");
        if (economicSubUI == null)
            Debug.LogError("Economic Sub UI is not assigned");
        if (disassembleSubUI == null)
            Debug.LogError("Disassemble Sub UI is not assigned");
        if (craftSubUI == null)
            Debug.LogError("Craft Sub UI is not assigned");
        if (inventoryUI == null)
            Debug.LogError("Inventory UI is not assigned");
        if (skillTreeUI == null)
            Debug.LogError("Skill Tree UI is not assigned");

        _startNewGameButton.interactable = false;
        setupMainMenu();
        setupCreditsText();
        updateCurrencyText();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (dialogeUI.activeInHierarchy == false)
            {
                if (optionsPopup.activeInHierarchy == true)
                    optionsPopup.SetActive(false);
                else if (skillTreeUI.activeInHierarchy == true)
                    skillTreeUI.SetActive(false);
                else if (questUI.activeInHierarchy == true)
                    questUI.SetActive(false);
                else if (gameObject.GetComponent<ExperienceManager>().LevelUpUIActive() == true)
                    gameObject.GetComponent<ExperienceManager>().collapseLevelUpMenu();
                else if (miniGameUI.activeInHierarchy == true)
                    gameObject.GetComponent<MiniGameControl>().stopCraftingMiniGame();
                else if (optionsPopup.activeInHierarchy == false)
                    optionsPopup.SetActive(true);
            }
        }
    }

    public void setupMainMenu()
    {
        if(File.Exists(Application.persistentDataPath + "/save1.json") && this.gameObject.GetComponent<GameMaster>().checkIfAnySavesExist())
        {
            if (_continueButton != null) setupContinueUI(); //_continueButton.interactable = true;
            if (_loadGameButton != null) _loadGameButton.interactable = true;
        }
        else
        {
            if (_continueButton != null) _continueButton.interactable = false; setupContinueUI();
            if (_loadGameButton != null) _loadGameButton.interactable = false;
        }

        if (_newGameButton != null) _newGameButton.interactable = true;
        if (_settingsButton != null) _settingsButton.interactable = true;
        if (_creditsButton != null) _creditsButton.interactable = true;
    }
    public void setupNewGameMenu()
    {
        _newGameMenu.SetActive(true);

        _startNewGameButton.interactable = false;
        _playerSpeciesDropdown.ClearOptions();
        speciesOptions = new List<string>();
        speciesOptions.Add("Select Species");
        foreach(AdventurerData species in this.gameObject.GetComponent<PlayerManager>().PlayerSpecies)
        {
            speciesOptions.Add(species.AdventurerSpecies);
        }
        _playerSpeciesDropdown.AddOptions(speciesOptions);
    }
    public void setupPlayerColorMenu()
    {
        if (_playerSpeciesDropdown.value != 0)
        {
            if (colorButtonRefs.Count != 0)
            {
                foreach(GameObject go in colorButtonRefs)
                    Destroy(go);
                colorButtonRefs.Clear();
            }

            _colorMenu.SetActive(true);
            string speciesName = speciesOptions[_playerSpeciesDropdown.value];
            //Debug.LogWarning(speciesName);
            if (speciesName == "Elf")
                playerColors = this.gameObject.GetComponent<AdventurerMaterials>().ElfColors;
            else if (speciesName == "Human")
                playerColors = this.gameObject.GetComponent<AdventurerMaterials>().HumanColors;
            else if (speciesName == "Lizardman")
                playerColors = this.gameObject.GetComponent<AdventurerMaterials>().LizardColors;

            PlayerSpecies = speciesName;
            int i = 0;
            foreach(Color32 color in playerColors)
            {
                GameObject go = Instantiate(_colorSamplePrefab, _colorMenuParent.transform);
                go.GetComponent<PlayerColor>().setupColor(color, i);
                colorButtonRefs.Add(go);
                i++;
            }
        }
        else if (_playerSpeciesDropdown.value == 0)
        {
            _colorMenu.SetActive(false);
        }
    }
    public void setupLoadGameMenu()
    {
        _loadGameMenu.SetActive(true);

        gameObject.GetComponent<GameMaster>().loadSaveGames();
        List<SaveTracker> saveTrackers = this.gameObject.GetComponent<GameMaster>().SaveTrackers;
        foreach (SaveTracker st in saveTrackers)
        {
            //Debug.Log("Save Tracker index: " + st.Index);
            if (File.Exists(st.SaveReference))
            {
                st.PlayerName = this.gameObject.GetComponent<GameMaster>().playerNameFromSaveString(st.SaveReference);
                st.ShopName = this.gameObject.GetComponent<GameMaster>().shopNameFromSaveString(st.SaveReference);
                st.setupTexts();
            }
            else
            {
                Debug.LogWarning("No save assigned to " + st.gameObject.name);
                st.PlayerName = "player name";
                st.ShopName = "shop name";
                st.setupTexts();
            }
        }
    }
    public void setupCreditsText()
    {
        string phs = "";
        CreditsManager creditsRef = this.gameObject.GetComponentInChildren<CreditsManager>();
        if (creditsRef.ProductionCredit != "")
            phs += "Production Credit\n" + creditsRef.ProductionCredit + "\n\n";

        if (creditsRef.MusicCredit != "")
            phs += "Music Credit\n" + creditsRef.MusicCredit + "\n\n";

        if (creditsRef.ArtCredit != "")
            phs += "Art Credit\n" + creditsRef.ArtCredit + "\n\n";

        if (creditsRef.OtherCredit != "")
            phs += "Other Credit\n" + creditsRef.OtherCredit + "\n\n";

        _creditsText.text = phs;
    }

    public void setupContinueUI()
    {
        if (File.Exists(Application.persistentDataPath + "/save1.json") && gameObject.GetComponent<GameMaster>().checkIfAnySavesExist())
        {
            //Debug.Log("UIControl.setupContinueUI() - save1.json exists!");
            _continueButton.interactable = true;

            _continueText.text = "continue game: " + gameObject.GetComponent<GameMaster>().getMostRecentSaveString(); 
        }
        else
        {
            _continueButton.interactable = false;
            _continueText.text = "continue game: no games saved";
        }
    }

    public void unloadUI(GameObject UIInput) { UIInput.SetActive(false); } 
    public void loadUI(GameObject UIInput) { UIInput.SetActive(true); } 
    public void unloadNewGameUI()
    {
        //storePlayerVariables();
        PlayerName = "";
        ShopName = "";
        _playerSpeciesDropdown.value = 0;
        _startNewGameButton.interactable = false;
        storePlayerVariables();
        _isNotValidUI.SetActive(false);
        _newGameMenu.SetActive(false);
    }
    public void mainMenu()
    {
        //this.gameObject.GetComponent<GameMaster>().saveGame();
        gameObject.GetComponent<GameMaster>().clearSavedData();
        gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
        gameObject.GetComponent<PlayerManager>().removePlayer();
        setupMainMenu();
    } 
    public void quitGame() { Application.Quit(); }
    public void doDialogue()
    {
        // TODO modular lines to signify quests
        List<QuestData> tutQuests = GameObject.FindGameObjectWithTag("QuestMaster").GetComponent<Quest>().getTutorialQuests();
        this.gameObject.GetComponent<DialogueControl>().CurrentQuest = tutQuests[0];
        this.gameObject.GetComponent<QuestControl>().forceSetQuest(tutQuests[0]);

        // only required line
        this.gameObject.GetComponent<DialogueControl>().startDialogue();
    }

    public void saveGameSelected(bool value)
    {
        _loadGameUIButton.interactable = value;
        _deleteGameUIButton.interactable = value;
    }

    public void mainMenuEnabled (bool input) { mainMenuUI.SetActive(input); }
    public void gameUIEnabled (bool input) { gameShopUI.SetActive(input); }
    public void optionsUIEnabled (bool input) { optionsPopup.SetActive(input); }
    public void newGameUIEnabled (bool input) { _newGameMenu.SetActive(input); }
    public void loadGameUIEnabled (bool input) { _loadGameMenu.SetActive(input); }

    public void shopEcoMenuEnabled(bool input) { economicSubUI.SetActive(input); }
    public void shopBuyMenuEnabled(bool input) { shopBuyMenu.SetActive(input); }
    public void shopSellMenuEnabled(bool input) { shopSellMenu.SetActive(input); }
    public void disassembleMenuEnabled(bool input) { disassembleSubUI.SetActive(input); }
    public void craftMenuEnabled(bool input) { craftSubUI.SetActive(input); }
    public void marketSellMenuEnabled(bool input) { marketEconomicSubUI.SetActive(input); }
    public void marketQuestMenuEnabled(bool input) { questSubUI.SetActive(input); }
    public void miniGameUIEnabled(bool input) { miniGameUI.SetActive(input); }
    public void questUIEnabled(bool input) { questDetailUI.SetActive(input); }

    public void skillUIEnabled(bool input) { skillTreeUI.SetActive(input); }

    public void marketAccessable(bool input)
    {
        //Debug.Log("market accessable: " + input.ToString());
        _toMarketButton.interactable = input;
    }

    public void itemCraftMenuEnabled (bool input) { itemCraftingUI.SetActive(input); }
    public void partCraftMenuEnabled(bool input) { partCraftingUI.SetActive(input); }

    public void shopBuyAccessableOnly()
    {
        Debug.Log("UIControl.shopBuyAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadShopBuyMenu();
        _buyUIButton.interactable = false; // this one
        _sellUIButton.interactable = false;
        _disaUIButton.interactable = false;
        _craftUIButton.interactable = false;

        gameObject.GetComponent<QuestControl>().nextStage();
    }
    public void shopSellAccessableOnly()
    {
        Debug.Log("UIControl.shopSellAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadShopSellMenu();
        _buyUIButton.interactable = false;
        _sellUIButton.interactable = false; // this one
        _disaUIButton.interactable = false;
        _craftUIButton.interactable = false;

        gameObject.GetComponent<QuestControl>().nextStage();
    }
    public void shopDisassembleAccessableOnly() // for use in tutorial only
    {
        Debug.Log("UIControl.shopDisassembleAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadDisassembleMenu();
        _buyUIButton.interactable = false;
        _sellUIButton.interactable = false;
        _disaUIButton.interactable = false; // this one
        _craftUIButton.interactable = false;

        gameObject.GetComponent<QuestControl>().nextStage();
    }
    public void shopCraftAccessableOnly() // for use in tutorial only
    {
        Debug.Log("UIControl.shopCraftAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadCraftMenu();
        _buyUIButton.interactable = false;
        _sellUIButton.interactable = false;
        _disaUIButton.interactable = false;
        _craftUIButton.interactable = false; // this one

        gameObject.GetComponent<QuestControl>().nextStage();
    }
    public void shopNoTabsAccessable()
    {
        _buyUIButton.interactable = false;
        _sellUIButton.interactable = false;
        _disaUIButton.interactable = false;
        _craftUIButton.interactable = false;
    }
    public void shopAllTabsAccessable()
    {
        _buyUIButton.interactable = true;
        _sellUIButton.interactable = true;
        _disaUIButton.interactable = true;
        _craftUIButton.interactable = true;
    }

    public void openInvUI()
    {
        receipeUI.SetActive(false);
        inventoryUI.SetActive(true);
        if (ShopCraftUIEnabled == true)
        {
            GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().setupInventory();
        }
    }
    public void openRecipesUI()
    {
        inventoryUI.SetActive(false);
        receipeUI.SetActive(true);
    }

    public void openPart1Inv()
    {
        // if part 1 'receipe' is a itemdata
        if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part1Type().Equals("item"))
        {
            openItemInv();
        }
        // else if part 1 'recipe' is an partdata
        else if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part1Type().Equals("part"))
        {
            openPartInv();
        }
        else if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part1Type() == null)
            Debug.LogWarning("UIControl.openPart1Inv(): something broke!");
    }
    public void openPart2Inv()
    {
        // if part 2 'receipe' is a itemdata
        if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part2Type() == "item")
        {
            openItemInv();
        }
        // else if part 2 'recipe' is an partdata
        else if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part2Type() == "part")
        {
            openPartInv();
        }
        else if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part2Type() == null)
            Debug.LogWarning("UIControl.openPart2Inv(): something broke!");
    }
    public void openPart3Inv()
    {
        // if part 3 'receipe' is a itemdata
        if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part3Type() == "item")
        {
            openItemInv();
        }
        // else if part 3 'recipe' is an partdata
        else if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part3Type() == "part")
        {
            openPartInv();
        }
        else if (GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>().Part3Type() == null)
            Debug.LogWarning("UIControl.openPart3Inv(): something broke!");
    }

    public void openItemInv()
    {
        openInvUI();
        matsScrollView.SetActive(false);
        enchantsScrollView.SetActive(false);
        partsScrollView.SetActive(false);
        itemsScrollView.SetActive(true);
        GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>().correctItemIndex();
        GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().setupItemInventory();
        BUI_InvSelected();
        IUI_ItemsSelected();
    }
    public void openPartInv()
    {
        openInvUI();
        itemsScrollView.SetActive(false);
        matsScrollView.SetActive(false);
        enchantsScrollView.SetActive(false);
        partsScrollView.SetActive(true);
        GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>().correctPartIndex();
        GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().setupPartInventory();
        BUI_InvSelected();
        IUI_PartsSelected();
    }
    public void openMatInv()
    {
        openInvUI();
        itemsScrollView.SetActive(false);
        partsScrollView.SetActive(false);
        enchantsScrollView.SetActive(false);
        matsScrollView.SetActive(true);
        GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().setupMatInventory();
        BUI_InvSelected();
        IUI_MatsSelected();
    }
    public void openEnchInv()
    {
        openInvUI();
        itemsScrollView.SetActive(false);
        partsScrollView.SetActive(false);
        matsScrollView.SetActive(false);
        enchantsScrollView.SetActive(true);
        GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().setupEnchantInventory();
        BUI_InvSelected();
        IUI_EnchSelected();
    }

    public void setupInvFilterUI()
    {
        setupItemInvFilterUI();
        setupPartInvFilterUI();
        setupMatInvFilterUI();
        setupEnchInvFilterUI();
    }
    private void setupItemInvFilterUI()
    {
        _itemsSortText.text = "sort: " + gameObject.GetComponent<GameMaster>().InvScriptRef.CurrentItemFilter.FilterName;
    }
    public void nextItemInvFilter()
    {
        gameObject.GetComponent<GameMaster>().InvScriptRef.nextItemFilter();
        setupItemInvFilterUI();
    }
    private void setupPartInvFilterUI()
    {
        _partsSortText.text = "sort: " + gameObject.GetComponent<GameMaster>().InvScriptRef.CurrentPartFilter.FilterName;
    }
    public void nextPartInvFilter()
    {
        gameObject.GetComponent<GameMaster>().InvScriptRef.nextPartFilter();
        setupPartInvFilterUI();
    }
    private void setupMatInvFilterUI()
    {
        _matsSortText.text = "sort: " + gameObject.GetComponent<GameMaster>().InvScriptRef.CurrentMatFilter.FilterName;
    }
    public void nextMatInvFilter()
    {
        gameObject.GetComponent<GameMaster>().InvScriptRef.nextMatFilter();
        setupMatInvFilterUI();
    }
    private void setupEnchInvFilterUI()
    {
        _enchantsSortText.text = "sort: " + gameObject.GetComponent<GameMaster>().InvScriptRef.CurrentEnchFilter.FilterName;
    }
    public void nextEnchInvFilter()
    {
        gameObject.GetComponent<GameMaster>().InvScriptRef.nextEnchFilter();
        setupEnchInvFilterUI();
    }

    public void updatingInputData()
    {
        _startNewGameButton.interactable = false;
    }
    public void updatePlayerSpecies()
    {
        if (_playerSpeciesDropdown.value == 0)
        {
            playerSpecies = "";
            //this.gameObject.GetComponent<PlayerManager>().setPlayerHead("");
        }
        else
        {
            playerSpecies = speciesOptions[_playerSpeciesDropdown.value];
            this.gameObject.GetComponent<PlayerManager>().setPlayerHead(playerSpecies);
        }
    }
    public void updatePlayerColor(int value, Color32 color)
    {
        playerColor = value;
        this.gameObject.GetComponent<PlayerManager>().setPlayerColor(value, color);

        foreach(GameObject go in colorButtonRefs)
        {
            if (go.GetComponent<PlayerColor>().ColorIndexRef != value)
                go.GetComponent<PlayerColor>().setButtonInteractable();
            else
                go.GetComponent<PlayerColor>().setButtonNotInteractable();
        }
    }
    public void checkInputData()
    {
        if (_playerName.text != "" && _shopName.text != "" && playerSpecies != "" && playerColor != -1)
        {
            if (checkString(_playerName.text) || checkString(_shopName.text))
            {
                _isNotValidUI.SetActive(true);
                _isNotValidUI.GetComponentInChildren<TextMeshProUGUI>().text = "Player name or Shop name contains an invalid input!";
            }
            else if (gameObject.GetComponent<GameMaster>().AllPlayerNames().Contains(_playerName.text) == false && gameObject.GetComponent<GameMaster>().AllShopNames().Contains(_shopName.text) == false)
            {
                //Debug.Log("has all required values");
                _startNewGameButton.interactable = true;
                _isNotValidUI.SetActive(false);
                storePlayerVariables();
            }
            else
            {
                _isNotValidUI.SetActive(true);
                _isNotValidUI.GetComponentInChildren<TextMeshProUGUI>().text = "Save with this Player Name and Shop Name already exists!";
            }
        }
        else if (gameObject.GetComponent<GameMaster>().AllPlayerNames().Contains(_playerName.text) == true && gameObject.GetComponent<GameMaster>().AllShopNames().Contains(_shopName.text) == true)
        { // if texts are already a save
            _isNotValidUI.SetActive(true);
            _isNotValidUI.GetComponentInChildren<TextMeshProUGUI>().text = "Save with this Player Name and Shop Name already exists!";
        }
        else if (_playerName.text == "" || _shopName.text == "" || playerSpecies == "" || playerColor == -1)
            _startNewGameButton.interactable = false;
    }
    public void storePlayerVariables()
    {
        this.gameObject.GetComponent<GameMaster>().PlayerName = PlayerName;
        this.gameObject.GetComponent<GameMaster>().ShopName = ShopName;
        this.gameObject.GetComponent<GameMaster>().PlayerSpecies = PlayerSpecies;
        this.gameObject.GetComponent<GameMaster>().PlayerColor = PlayerColor;
    }
    public void hideHighlights()
    {
        foreach (SaveTracker st in this.gameObject.GetComponent<GameMaster>().SaveTrackers)
        {
            st.hideHighlight();
        }
        
    }

    private bool checkString(string input)
    {
        foreach(string st in gameObject.GetComponent<DefaultValues>().BadNameInputs)
        {
            if (input.Contains(st))
                return true;
        }
        return false;
    }

    public void loadCredits()
    {
        _mainUIElements.SetActive(false);
        _creditsUI.SetActive(true);
    }
    public void loadMainMenuFromCredits()
    {
        _creditsUI.SetActive(false);
        _mainUIElements.SetActive(true);
    }

    public void updateCurrencyText()
    {
        _currencyText.text = this.gameObject.GetComponent<GameMaster>().CurrentCurrency.ToString();
    }

    public bool MainMenuUIActive { get => mainMenuUI.activeInHierarchy; }
    public bool ShopUIActive { get => gameShopUI.activeInHierarchy; }
    public bool OptionsUIActive { get => optionsPopup.activeInHierarchy; }
    public string PlayerName { get => _playerName.text; set => _playerName.text = value; }
    public string ShopName { get => _shopName.text; set => _shopName.text = value; }
    public string PlayerSpecies { get => playerSpecies; set => playerSpecies = value; }
    public int PlayerColor { get => playerColor; set => playerColor = value; }

    public bool ShopUIEnabled { get => shopUI.activeInHierarchy; }
    public bool MarketUIEnabled { get => marketUI.activeInHierarchy; }

    public bool ShopEcoUIEnabled { get => economicSubUI.activeInHierarchy; set => economicSubUI.SetActive(value); }
    public bool ShopDisUIEnabled { get => disassembleSubUI.activeInHierarchy; set => disassembleSubUI.SetActive(value); }
    public bool ShopCraftUIEnabled { get => craftSubUI.activeInHierarchy; set => craftSubUI.SetActive(value); }
    public bool CraftItemUIEnabled { get => itemCraftingUI.activeInHierarchy; }
    public bool CraftPartUIEnabled { get => partCraftingUI.activeInHierarchy; }
    public bool MarketEcoUIEnabled { get => marketEconomicSubUI.activeInHierarchy; set => marketEconomicSubUI.SetActive(value); }
    public bool MarketQuestUIEnabled { get => questSubUI.activeInHierarchy; set => questSubUI.SetActive(value); }
    public bool InventoryItemUIEnabled { get => itemsScrollView.activeInHierarchy; }
    public bool InventoryPartUIEnabled { get => partsScrollView.activeInHierarchy; }
    public bool InventoryMatUIEnabled { get => matsScrollView.activeInHierarchy; }
    public bool InventoryEnchantUIEnabled { get => enchantsScrollView.activeInHierarchy; }

    public void selectActiveShopUI()
    {
        if (shopBuyMenu.activeInHierarchy == true)
            SUI_BuySelected();
        else if (shopSellMenu.activeInHierarchy == true)
            SUI_SellSelected();
        else if (disassembleSubUI.activeInHierarchy == true)
            SUI_DisassembleSelected();
        else if (craftSubUI.activeInHierarchy == true)
            SUI_CraftSelected();

        if (inventoryUI.activeInHierarchy == true)
            BUI_InvSelected();
        else if (receipeUI.activeInHierarchy == true)
            BUI_RecipesSelected();
    }
    
    // Shop ui buy button
    public void SUI_BuySelected() { _shopUIGroupScript.ButtonSelected(_buyUIButton); }
    public void SUI_BuyDisabled() { _shopUIGroupScript.ButtonDisabled(_buyUIButton); }
    public void SUI_BuyEnabled() { _shopUIGroupScript.ButtonEnabled(_buyUIButton); }
    // shop ui sell button
    public void SUI_SellSelected() { _shopUIGroupScript.ButtonSelected(_sellUIButton); }
    public void SUI_SellDisabled() { _shopUIGroupScript.ButtonDisabled(_sellUIButton); }
    public void SUI_SellEnabled() { _shopUIGroupScript.ButtonEnabled(_sellUIButton); }
    // shop ui disassemble button
    public void SUI_DisassembleSelected() { _shopUIGroupScript.ButtonSelected(_disaUIButton); }
    public void SUI_DisassembleDisabled() { _shopUIGroupScript.ButtonDisabled(_disaUIButton); }
    public void SUI_DisassembleEnabled() { _shopUIGroupScript.ButtonEnabled(_disaUIButton); }
    // shop ui craft button
    public void SUI_CraftSelected() { _shopUIGroupScript.ButtonSelected(_craftUIButton); }
    public void SUI_CraftDisabled() { _shopUIGroupScript.ButtonDisabled(_craftUIButton); }
    public void SUI_CraftEnabled() { _shopUIGroupScript.ButtonEnabled(_craftUIButton); }

    public void selectActiveMarketUI()
    {
        if (marketEconomicSubUI.activeInHierarchy == true)
            MUI_SellSelected();
        else if (questSubUI.activeInHierarchy == true)
            MUI_QuestSelected();
    }

    public ButtonGroup MUIButton_Script { get => _marketUIGroupScript; }
    public void MUI_SellSelected() { _marketUIGroupScript.ButtonSelected(_mSellUIButton); }
    public void MUI_QuestSelected() { _marketUIGroupScript.ButtonSelected(_mQuestUIButton); }

    public ButtonGroup BUIButton_Script { get => _bottomUIGroupScript; }
    public void BUI_InvSelected() { _bottomUIGroupScript.ButtonSelected(_invUIButton); }
    public void BUI_RecipesSelected() {  _bottomUIGroupScript.ButtonSelected(_recipesUIButton); }

    public ButtonGroup IUIButton_Script { get => _invUIGroupScript; }
    public void IUI_ItemsSelected() { _invUIGroupScript.ButtonSelected(_itemsUIButton); }
    public void IUI_PartsSelected() { _invUIGroupScript.ButtonSelected(_partsUIButton); }
    public void IUI_MatsSelected() { _invUIGroupScript.ButtonSelected(_matsUIButton); }
    public void IUI_EnchSelected() { _invUIGroupScript.ButtonSelected(_enchUIButton); }

    /*
    [Header("Button Group - ShopUI")]
    [SerializeField] private ButtonGroup _shopUIGroupScript;
    [SerializeField] private Button _buyUIButton;
    [SerializeField] private Button _sellUIButton;
    [SerializeField] private Button _disaUIButton;
    [SerializeField] private Button _craftUIButton;
    [Header("Button Group - MarketUI")]
    [SerializeField] private ButtonGroup _marketUIGroupScript;
    [SerializeField] private Button _mSellUIButton;
    [SerializeField] private Button _mQuestUIButton;
    [Header("ButtonGroup - BottomUI")]
    [SerializeField] private ButtonGroup _bottomUIGroupScript;
    [SerializeField] private Button _invUIButton;
    [SerializeField] private Button _recipesUIButton;
    [Header("ButtonGroup - InvUI")]
    [SerializeField] private ButtonGroup _invUIGroupScript;
    [SerializeField] private Button _itemsUIButton;
    [SerializeField] private Button _partsUIButton;
    [SerializeField] private Button _matsUIButton;
    [SerializeField] private Button _enchUIButton;
    */
}
