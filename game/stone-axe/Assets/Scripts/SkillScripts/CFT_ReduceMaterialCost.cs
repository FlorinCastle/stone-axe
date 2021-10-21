using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFT_ReduceMaterialCost : MonoBehaviour
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
                if (currentLevel == maxLevel)
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
        skillBodyText.text = "reduce amount of consumed material by " + (5f * currentLevel) + "%";
    }

    public float getModifiedMatAmount()
    {
        for (int i = 0; i <= maxLevel; i++)
            if (i == currentLevel)
                return 1.0f - (0.05f * i);
        return 1.0f;
    }
}
