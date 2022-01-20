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
    [SerializeField] private List<int> _levelMarks;

    [SerializeField] private SkillManager _skillMgrRef;

    private void Awake()
    {
        if (_gameMasterRef == null)
            this.gameObject.GetComponent<GameMaster>();
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
            else if (levelMark > _gameMasterRef.GetTotalExperience)
                break;
        }
        _gameMasterRef.setLevel(counter);
        _gameMasterRef.setCurrentSkillPoints(counter);
        _skillMgrRef.setTotalSkillPoints(counter);
        _levelTxt.text = "level: " + _gameMasterRef.GetLevel;
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
                    a = 0.0f;
                else 
                    a = (levelMark - _levelMarks[i]) * 1.0f;  

                float b = levelMark - _gameMasterRef.GetTotalExperience * 1.0f;

                _expSlider.value = 1.0f - b / a;

                break;
            }
            i++;
        }
    }
}
