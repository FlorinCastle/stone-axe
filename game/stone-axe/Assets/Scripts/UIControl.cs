using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void unloadUI(GameObject UIInput)
    {
        UIInput.SetActive(false);
    }

    public void loadUI(GameObject UIInput)
    {
        UIInput.SetActive(true);
    }

    public void mainMenu()
    {
        this.gameObject.GetComponent<GameMaster>().saveGame();
        this.gameObject.GetComponent<GameMaster>().clearSavedData();
        this.gameObject.GetComponent<AdventurerMaster>().removeAllAdventurers();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
