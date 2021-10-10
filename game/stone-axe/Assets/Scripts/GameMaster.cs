
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private int _currentCurrency;
    [SerializeField] private int _totalExperience;
    [SerializeField] private int _level;
    [SerializeField] private int _currentSkillPoints;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void addCurrency(int value)
    {
        _currentCurrency += value;
    }

    public bool removeCurrency(int value)
    {
        int temp = _currentCurrency - value;

        if (temp >= 0)
        {
            _currentCurrency = temp;
            return true;
        }
        else if (temp < 0)
            return false;

        return false;
    }

    public void setTotalExperience(int value) { _totalExperience = value; }
    public int GetTotalExperience { get => _totalExperience; }

    public void setLevel(int value) { _level = value; }
    public int GetLevel { get => _level; }

    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
}
