using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private GameMaster _gameMasterRef;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _levelTxt;
    [SerializeField] private GameObject _levelUpMenu;
    [SerializeField] private TextMeshProUGUI _levelUpMenuText;
    [SerializeField] private List<int> _levelMarks;

    [SerializeField] private SkillManager _skillMgrRef;

    [SerializeField] private List<LevelUpData> _levelUpData;

    private int prevUpdatedLevel = 0;
    //private float prevEXPPercent = 0f;

    private void Awake()
    {
        if (_gameMasterRef == null)
            this.gameObject.GetComponent<GameMaster>();
        //updateEXPSlider();
        updateEXPSlider();
        calculateLevel();
    }

    public void addExperience(int value)
    {
        _gameMasterRef.setTotalExperience(_gameMasterRef.GetTotalExperience + value);
        updateEXPSlider();
        calculateLevel();
    }

    public void calculateLevel()
    {
        int counter = 0;
        foreach (int levelMark in _levelMarks)
        {
            if (levelMark <= _gameMasterRef.GetTotalExperience)
            {
                counter++;
                if (_gameMasterRef.GetLevel < counter)
                    _skillMgrRef.AddSkillPoint();
            }
            else if (levelMark > _gameMasterRef.GetTotalExperience && counter > prevUpdatedLevel)
            {
                setupLevelUpMenu(counter);
                //_gameMasterRef.updateLevelLocks();
                break;
            }
        }
        _gameMasterRef.setLevel(counter);
        _gameMasterRef.setCurrentSkillPoints(counter);
        _skillMgrRef.setTotalSkillPoints(counter);
        _levelTxt.text = "level: " + _gameMasterRef.GetLevel;
        prevUpdatedLevel = _gameMasterRef.GetLevel;
        _gameMasterRef.updateLevelLocks();
    }

    public void updateEXPSlider()
    {
        int i = -1;
        foreach (int levelMark in _levelMarks)
        {
            if (levelMark > _gameMasterRef.GetTotalExperience)
            {
                float a = 0.0f;
                if (i == -1)
                    a = 10.0f;
                else
                    a = (levelMark - _levelMarks[i]) * 1.0f;

                float b = levelMark - _gameMasterRef.GetTotalExperience * 1.0f;

                _expSlider.value = 1.0f - b / a;

                break;
            }
            i++;
        }
    }

    public void setupLevelUpMenu(int level)
    {
        _levelUpMenuText.text = "leveled up to Level " + level + "!\n";
        LevelUpData currLvl = _levelUpData[level];

        if (currLvl.ItemRecipeUnlocks.Count > 0)
        {
            if (currLvl.ItemRecipeUnlocks.Count > 1)
                _levelUpMenuText.text += "\nItem Recipes Unlocked!";
            else
                _levelUpMenuText.text += "\nItem Recipe Unlocked!";

            foreach (ItemData itemRec in currLvl.ItemRecipeUnlocks)
                _levelUpMenuText.text += "\n" + itemRec.ItemName;
        }
        if (currLvl.PartRecipeUnlocks.Count > 0)
        {
            if (currLvl.PartRecipeUnlocks.Count > 1)
                _levelUpMenuText.text += "\nPart Recipes Unlocked!";
            else
                _levelUpMenuText.text += "\nPart Recipe Unlocked!";

            foreach (PartData partRec in currLvl.PartRecipeUnlocks)
                _levelUpMenuText.text += "\n" + partRec.PartName;
        }
        if (currLvl.MaterialUnlocks.Count > 0)
        {
            if (currLvl.PartRecipeUnlocks.Count > 1)
                _levelUpMenuText.text += "\nMaterials Unlocked!";
            else
                _levelUpMenuText.text += "\nMaterial Unlocked!";

            foreach (MaterialData matUnlock in currLvl.MaterialUnlocks)
                _levelUpMenuText.text += "\n" + matUnlock.Material;
        }
        _levelUpMenu.SetActive(true);
    }

    public void collapseLevelUpMenu()
    {
        _levelUpMenu.SetActive(false);
        _levelUpMenuText.text = "placeholder\nif you see this, something went wrong";
    }

    public bool LevelUpUIActive() { return _levelUpMenu.activeInHierarchy; }

    /* does not work for some reason >:(
    public void smoothUpdateEXPSlider()
    {
        int i = -1;
        foreach (int levelMark in _levelMarks)
        {
            if (levelMark > prevUpdatedLevel)
            {
                Debug.Log("test");
                if (_gameMasterRef.GetTotalExperience > prevEXPPercent)
                {
                    StartCoroutine(SmoothEXPSlider(_gameMasterRef.GetTotalExperience/10.0f));
                    break;
                }
            }
            /*
            if (levelMark > _gameMasterRef.GetTotalExperience)
            {
                float a = 10.0f;
                if (i != -1) 
                    a = (levelMark - _levelMarks[i]) * 1.0f;  

                float b = levelMark - _gameMasterRef.GetTotalExperience * 1.0f;


                _expSlider.value = 1.0f - b / a;
                prevEXPPercent = _expSlider.value;

                break;
            }
            
            i++;
        }
    }

    private IEnumerator SmoothEXPSlider(float finalPercentValue)
    {
        float totalSliderTime = 0f;
        totalSliderTime += Time.deltaTime;

        Debug.Log(finalPercentValue);
        while (_expSlider.value < finalPercentValue)
        {
            _expSlider.value = Mathf.Lerp(prevEXPPercent, finalPercentValue, totalSliderTime / 5);
            yield return null;
        }
        //yield return null;
        /*
        foreach (int levelMark in _levelMarks)
        {
            _expSlider.value = Mathf.Lerp(prevEXPPercent, finalPercentValue, totalSliderTime / 5);
            yield return null;
        }
    }
    */
}
