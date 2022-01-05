using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject gameShopUI;
    [SerializeField] GameObject optionsPopup;
    [Header("Sub UI")]
    [SerializeField] GameObject economicSubUI;
    [SerializeField] GameObject disassembleSubUI;
    [SerializeField] GameObject craftSubUI;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject skillTreeUI;
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

    private void setupMainMenu()
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
    private void setupNewGameMenu()
    {
        _startNewGameButton.interactable = false;
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
    public void checkInputData()
    {
        if (_playerName.text != "" && _shopName.text != "")
        {
            Debug.Log("has all required text");
            _startNewGameButton.interactable = true;
        }
        else if (_playerName.text == "" || _shopName.text == "")
            _startNewGameButton.interactable = false;
    }
    public void storePlayerVariables()
    {
        this.gameObject.GetComponent<GameMaster>().PlayerName = PlayerName;
        this.gameObject.GetComponent<GameMaster>().ShopName = ShopName;
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
}
