using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DIS_HitReduction : MonoBehaviour
{
    [SerializeField] private SkillManager _skillManagerRef;
    [SerializeField] private GameMaster _gameMasterRef;
    [SerializeField, HideInInspector] private SoundMaster _soundControl;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel = 3;
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
        _soundControl = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
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

    public void playMouseOverSound() { _soundControl.playButtonHoverSound(); }
    public void playButtonClickSound() { _soundControl.playButtonClickSound(); }

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
                    _skillManagerRef.setupSkillUI();
                }
                checkButtons();
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

        if (_skillManagerRef.hasFreeSkillPoint() && currentLevel != maxLevel)
        {
            addPointButton.GetComponent<Button>().interactable = true; 
        }
        else if (currentLevel == maxLevel)
        {
            addPointButton.GetComponent<Button>().interactable = false;
        }
        else if (_skillManagerRef.hasFreeSkillPoint() == false)
        {
            addPointButton.GetComponent<Button>().interactable = false;
        }
    }

    private void setupSkillLevelText()
    {
        skillLevelText.text = currentLevel + " / " + maxLevel;
    }
    private void setupSkillText()
    {
        skillBodyText.text = "points to disassemble an item is reduced by " + currentLevel + ". minimum to hit 1";
    }
    public int CurrentSkillLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    }


    public float getHitReductionAmount()
    {
        for (int i = 0; i <= maxLevel; i++)
            if (i == currentLevel)
                return (1f * i);
        return 0.0f;
    }
}
