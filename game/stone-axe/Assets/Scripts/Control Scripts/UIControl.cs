using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameShopUI;
    [SerializeField] private GameObject optionsPopup;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject marketUI;
    [Header("Sub UI")]
    [SerializeField] private TextMeshProUGUI _currencyText;
    [Header("Shop UI")]
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
    [Space(5)]
    [SerializeField] private Button _buyTabButton;
    [SerializeField] private Button _sellTabButton;
    [SerializeField] private Button _disassembleTabButton;
    [SerializeField] private Button _craftTabButton;
    [Space(5)]
    [SerializeField] private GameObject itemsScrollView;
    [SerializeField] private TextMeshProUGUI _itemsSortText;
    [SerializeField] private GameObject partsScrollView;
    [SerializeField] private TextMeshProUGUI _partsSortText;
    [SerializeField] private GameObject matsScrollView;
    [SerializeField] private TextMeshProUGUI _matsSortText;
    [SerializeField] private GameObject enchantsScrollView;
    [SerializeField] private TextMeshProUGUI _enchantsSortText;
    [Header("Market UI")]
    [SerializeField] private GameObject marketEconomicSubUI;
    [SerializeField] private GameObject questSubUI;
    [Header("Main Menu UI")]
    [SerializeField] private GameObject _mainUIElements;
    [SerializeField] private GameObject _creditsUI;
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private GameObject _loadGameMenu;
    [SerializeField] private GameObject _newGameMenu;
    [Header("New Game UI")]
    [SerializeField] private InputField _playerName;
    [SerializeField] private InputField _shopName;
    [SerializeField] private Button _startNewGameButton;
    [Header("Load Game UI")]
    [SerializeField] private Button _loadGameUIButton;
    [SerializeField] private Button _deleteGameUIButton;

    [Header("Player Creation")]
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

        gameObject.GetComponent<GameMaster>().marketAccessable(false);
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
                else if (optionsPopup.activeInHierarchy == false)
                    optionsPopup.SetActive(true);
            }
        }
    }

    public void setupMainMenu()
    {
        if(File.Exists(Application.persistentDataPath + "/save.txt") && this.gameObject.GetComponent<GameMaster>().checkIfAnySavesExist())
        {
            if (_continueButton != null) _continueButton.interactable = true;
            if (_loadGameButton != null) _loadGameButton.interactable = true;
        }
        else
        {
            if (_continueButton != null) _continueButton.interactable = false;
            if (_loadGameButton != null) _loadGameButton.interactable = false;
        }

        if (_newGameButton != null) _newGameButton.interactable = true;
        if (_settingsButton != null) _settingsButton.interactable = true;
        if (_creditsButton != null) _creditsButton.interactable = true;
    }
    public void setupNewGameMenu()
    {
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
        this.gameObject.GetComponent<GameMaster>().loadSaveGames();
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

    public void unloadUI(GameObject UIInput) { UIInput.SetActive(false); } 
    public void loadUI(GameObject UIInput) { UIInput.SetActive(true); } 
    public void unloadNewGameUI()
    {
        storePlayerVariables();
        PlayerName = "";
        ShopName = "";
        _playerSpeciesDropdown.value = 0;
        _newGameMenu.SetActive(false);
        _startNewGameButton.interactable = false;
    }
    public void mainMenu()
    {
        this.gameObject.GetComponent<GameMaster>().saveGame();
        this.gameObject.GetComponent<GameMaster>().clearSavedData();
        this.gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
        this.gameObject.GetComponent<PlayerManager>().removePlayer();
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

    public void mainMenuEnabled(bool input) { mainMenuUI.SetActive(input); }
    public void gameUIEnabled (bool input) { gameShopUI.SetActive(input); }
    public void optionsUIEnabled(bool input) { optionsPopup.SetActive(input); }

    public void shopEcoMenuEnabled(bool input) { economicSubUI.SetActive(input); }
    public void shopBuyMenuEnabled(bool input) { shopBuyMenu.SetActive(input); }
    public void shopSellMenuEnabled(bool input) { shopSellMenu.SetActive(input); }
    public void disassembleMenuEnabled(bool input) { disassembleSubUI.SetActive(input); }
    public void craftMenuEnabled(bool input) { craftSubUI.SetActive(input); }
    public void marketSellMenuEnabled(bool input) { marketEconomicSubUI.SetActive(input); }
    public void marketQuestMenuEnabled(bool input) { questSubUI.SetActive(input); }
    public void miniGameUIEnabled(bool input) { miniGameUI.SetActive(input); }
    public void questUIEnabled(bool input) { questDetailUI.SetActive(input); }

    public void marketAccessable(bool input)
    {
        //Debug.Log("market accessable: " + input.ToString());
        _toMarketButton.interactable = input;
    }

    public void itemCraftMenuEnabled (bool input)
    {
        itemCraftingUI.SetActive(input);
    }
    public void partCraftMenuEnabled(bool input)
    {
        partCraftingUI.SetActive(input);
    }

    public void shopBuyAccessableOnly()
    {
        Debug.Log("UIControl.shopBuyAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadShopBuyMenu();
        _buyTabButton.interactable = true;
        _sellTabButton.interactable = false;
        _disassembleTabButton.interactable = false;
        _craftTabButton.interactable = false;
    }
    public void shopSellAccessableOnly()
    {
        Debug.Log("UIControl.shopSellAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadShopSellMenu();
        _buyTabButton.interactable = false;
        _sellTabButton.interactable = true;
        _disassembleTabButton.interactable = false;
        _craftTabButton.interactable = false;
    }
    public void shopDisassembleAccessableOnly()
    {
        Debug.Log("UIControl.shopDisassembleAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadDisassembleMenu();
        _buyTabButton.interactable = false;
        _sellTabButton.interactable = false;
        _disassembleTabButton.interactable = true;
        _craftTabButton.interactable = false;
    }
    public void shopCraftAccessableOnly()
    {
        Debug.Log("UIControl.shopCraftAccessableOnly() has been called!");
        gameObject.GetComponent<GameMaster>().loadCraftMenu();
        _buyTabButton.interactable = false;
        _sellTabButton.interactable = false;
        _disassembleTabButton.interactable = false;
        _craftTabButton.interactable = true;
    }
    public void shopAllTabsAccessable()
    {
        _buyTabButton.interactable = true;
        _sellTabButton.interactable = true;
        _disassembleTabButton.interactable = true;
        _craftTabButton.interactable = true;
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
    }
    public void openPartInv()
    {
        openInvUI();
        itemsScrollView.SetActive(false);
        matsScrollView.SetActive(false);
        enchantsScrollView.SetActive(false);
        partsScrollView.SetActive(true);
    }
    public void openMatInv()
    {
        openInvUI();
        itemsScrollView.SetActive(false);
        partsScrollView.SetActive(false);
        enchantsScrollView.SetActive(false);
        matsScrollView.SetActive(true);
    }
    public void openEnchInv()
    {
        openInvUI();
        if (CraftPartUIEnabled == true)
            GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryScript>().setupEnchantInventory();
        itemsScrollView.SetActive(false);
        partsScrollView.SetActive(false);
        matsScrollView.SetActive(false);
        enchantsScrollView.SetActive(true);
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
            //Debug.Log("has all required values");
            _startNewGameButton.interactable = true;
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

    public void loadCredits()
    {
        _mainUIElements.SetActive(false);
        _creditsUI.SetActive(true);
    }
    public void loadMainMenu()
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
}
