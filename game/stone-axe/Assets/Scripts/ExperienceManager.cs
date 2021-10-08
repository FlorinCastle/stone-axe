using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private GameMaster _gameMasterRef;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private List<int> _levelMarks;

    private void Awake()
    {
        if (_gameMasterRef == null)
            this.gameObject.GetComponent<GameMaster>();
        updateEXPSlider();
    }

    public void addExperience(int value)
    {
        _gameMasterRef.setTotalExperience(_gameMasterRef.GetTotalExperience + value);
        calculateLevel();
        updateEXPSlider();
    }

    public void calculateLevel()
    {
        int counter = 0;
        foreach (int levelMark in _levelMarks)
        {
            if (levelMark <= _gameMasterRef.GetTotalExperience) 
                counter++; 
            else if (levelMark > _gameMasterRef.GetTotalExperience)
                break;
        }
        _gameMasterRef.setLevel(counter);
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
