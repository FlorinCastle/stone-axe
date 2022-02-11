using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject gameShopUI;
    [SerializeField] GameObject optionsPopup;
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject marketUI;
    [Header("Shop UI")]
    [Header("Sub UI")]
    [SerializeField] GameObject economicSubUI;
    [SerializeField] GameObject disassembleSubUI;
    [SerializeField] GameObject craftSubUI;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject skillTreeUI;
    [Header("Market UI")]
    [SerializeField] GameObject marketEconomicSubUI;
    [SerializeField] GameObject questSubUI;
    [Header("Main Menu UI")]
    [SerializeField] Button _continueButton;
    [SerializeField] Button _newGameButton;
    [SerializeField] Button _loadGameButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _creditsButton;
    [SerializeField] GameObject _loadGameMenu;
    [SerializeField] GameObject _newGameMenu;
    [Header("New Game UI")]
    [SerializeField] InputField _playerName;
    [SerializeField] InputField _shopName;
    [SerializeField] Button _startNewGameButton;
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
    }

    public void setupMainMenu()
    {
        if(File.Exists(Application.dataPath + "/save.txt") && this.gameObject.GetComponent<GameMaster>().checkIfAnySavesExist())
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
        if (_creditsButton != null) _creditsButton.interactable = false;
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
            Debug.Log("has all required values");
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
    public bool MarketEcoUIEnabled { get => marketEconomicSubUI.activeInHierarchy; set => marketEconomicSubUI.SetActive(value); }
    public bool MarketQuestUIEnabled { get => questSubUI.activeInHierarchy; set => questSubUI.SetActive(value); }
}
