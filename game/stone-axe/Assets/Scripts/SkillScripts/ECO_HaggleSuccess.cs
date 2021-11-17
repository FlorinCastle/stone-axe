using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECO_HaggleSuccess : MonoBehaviour
{
    [SerializeField] private SkillManager _skillManagerRef;
    [SerializeField] private GameMaster _gameMasterRef;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private int requiredPlayerLevel = 0;
    [Header("UI")]
    [SerializeField] private GameObject addPointButton;
    [SerializeField] private Text skillLevelText;
    [SerializeField] private Text skillBodyText;

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
                removeAddButton();
            }
            else
            {
                _skillManagerRef.RemoveSkillPoint();
                _skillManagerRef.updateSkillPoints();
                if (_skillManagerRef.GetCurrentSkillPoints == 0)
                {
                    addPointButton.GetComponent<Button>().interactable = false;
                }
                else if (currentLevel == maxLevel)
                    removeAddButton();
            }
        }
        setupSkillLevelText();
        setupSkillText();
    }

    public void removeAddButton()
    {
        addPointButton.SetActive(false);
    }

    private void setupSkillLevelText()
    {
        skillLevelText.text = currentLevel + " / " + maxLevel;
    }

    private void setupSkillText()
    {
        skillBodyText.text = "haggle success chance is " + (30f + (10f * currentLevel)) + "%, increasing sell price by +" + (5f + (1f * currentLevel)) + "%";
    }

    public int CurrentSkillLevel
    {
        get => currentLevel;
    }

    public float getHaggleChance()
    {
        for (int i = 0; i <= maxLevel; i++)
            if (i == currentLevel)
                return (30f + 10f * i);
        return 30.0f;
    }

    public float getModifiedPrice()
    {
        for (int i = 0; i <= maxLevel; i++)
            if (i == currentLevel)
                return (0.05f + (0.05f * i));
        return 0.05f;
    }
}
