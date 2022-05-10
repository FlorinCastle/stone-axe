using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECO_DecBuyPrice : MonoBehaviour
{
    [SerializeField] private SkillManager _skillManagerRef;
    [SerializeField] private GameMaster _gameMasterRef;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private int requiredPlayerLevel = 0;
    [Header("UI")]
    [SerializeField] private GameObject addPointButton;
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private TextMeshProUGUI skillBodyText;
    [SerializeField] private GameObject removePointButton;

    private void Awake()
    {
        _skillManagerRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SkillManager>();
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        if (requiredPlayerLevel <= _gameMasterRef.GetLevel)
        {
            addPointButton.GetComponent<Button>().interactable = true;
            if (currentLevel == maxLevel)
                addPointButton.SetActive(false);
            else
                addPointButton.SetActive(true);
        }
        else
            addPointButton.GetComponent<Button>().interactable = false;
        setupSkillLevelText();
        setupSkillText();
    }

    public void addLevel()
    {
        if (_skillManagerRef.hasFreeSkillPoint())
        {
            currentLevel++;
            if (currentLevel > maxLevel)
            {
                currentLevel = maxLevel;
                checkButtons(); // removeAddButton();
            }
            else
            {
                _skillManagerRef.RemoveSkillPoint();
                _skillManagerRef.updateSkillPoints();
                if (_skillManagerRef.GetCurrentSkillPoints == 0)
                {
                    //addPointButton.GetComponent<Button>().interactable = false;
                    checkButtons();
                    _skillManagerRef.setupSkillUI();
                }
                else if (currentLevel == maxLevel)
                    checkButtons(); //removeAddButton();
            }
        }
        updateSkillTexts();
    }
    public void removeLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;

            _skillManagerRef.AddSkillPoint();
            _skillManagerRef.updateSkillPoints();

            if (currentLevel == 0)
                removePointButton.GetComponent<Button>().interactable = false;
        }
        else if (currentLevel == 0)
            removePointButton.GetComponent<Button>().interactable = false;
        _skillManagerRef.setupSkillUI();
        updateSkillTexts();
    }
    public void updateSkillTexts()
    {
        setupSkillLevelText();
        setupSkillText();

    }
    public void removeAddButton()
    {
        addPointButton.SetActive(false);
    }
    public void checkButtons()
    {
        if (currentLevel > 0)
            removePointButton.GetComponent<Button>().interactable = true;
        else if (currentLevel == 0)
            removePointButton.GetComponent<Button>().interactable = false;

        if (_skillManagerRef.hasFreeSkillPoint())
            addPointButton.GetComponent<Button>().interactable = true;
        else
            addPointButton.GetComponent<Button>().interactable = false;
    }

    private void setupSkillLevelText()
    {
        skillLevelText.text = currentLevel + " / " + maxLevel;
    }
    private void setupSkillText()
    {
        skillBodyText.text = "decrease final buy price by " + (1f * currentLevel).ToString() + "%";

    }
    public int CurrentSkillLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    }


    public float getModifiedBuyPrice()
    {
        for (int i = 0; i <= maxLevel; i++)
            if (i == currentLevel)
                return 1.0f - (0.01f * i);
        return 1.0f;
    }
}
