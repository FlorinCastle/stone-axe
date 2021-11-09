
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private int _currentCurrency;
    [SerializeField] private int _totalExperience;
    [SerializeField] private int _level;
    [SerializeField] private int _currentSkillPoints;
    [Header("UI and Level")]
    [SerializeField] private GameObject _shopLevel;
    [SerializeField] private GameObject _marketLevel;
    [SerializeField] private GameObject _shopSubUI;
    [SerializeField] private GameObject _marketSubUI;
    [SerializeField] private GameObject _toShopButton;
    [SerializeField] private GameObject _toMarketButton;

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

    public void loadMarketLevel()
    {
        _shopLevel.SetActive(false);
        _marketLevel.SetActive(true);
        _shopSubUI.SetActive(false);
        _marketSubUI.SetActive(true);
        _toShopButton.SetActive(true);
        _toMarketButton.SetActive(false);
        this.gameObject.GetComponent<SellItemControl>().SellingState = 1;
    }

    public void loadShopLevel()
    {
        _shopLevel.SetActive(true);
        _marketLevel.SetActive(false);
        _shopSubUI.SetActive(true);
        _marketSubUI.SetActive(false);
        _toShopButton.SetActive(false);
        _toMarketButton.SetActive(true);
        this.gameObject.GetComponent<SellItemControl>().SellingState = 0;
    }

    public void setTotalExperience(int value) { _totalExperience = value; }
    public int GetTotalExperience { get => _totalExperience; }

    public void setLevel(int value) { _level = value; }
    public int GetLevel { get => _level; }

    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
}
