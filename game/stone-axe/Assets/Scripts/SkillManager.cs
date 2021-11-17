using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private GameMaster _gameMasterRef;
    [SerializeField] private int _totalSkillPoints;
    [SerializeField] private int _currentSkillPoints;

    [Header("UI")]
    [SerializeField] private Text _skillPointsText;
    [SerializeField] private GameObject _economicSkillsContent;
    [SerializeField] private GameObject _disassemblySkillsContent;
    [SerializeField] private GameObject _craftingSkillsContent;

    [Header("Prefabs")]
    [SerializeField] private GameObject _skillPrefab;

    [Header("Skill Tracking")]
    //[SerializeField] private List<GameObject> _allSkills;
    //[SerializeField] private List<GameObject> _economicSkills;
    //[SerializeField] private List<GameObject> _disasemblySkills;
    //[SerializeField] private List<GameObject> _craftingSkills;
    [SerializeField] private ECO_IncSellPrice _ECOIncSellPrice;
    [SerializeField] private ECO_DecBuyPrice _ECODecBuyPrice;
    [SerializeField] private ECO_HaggleSuccess _ECOHaggleSuccess;
    [SerializeField] private DIS_DisassembleChance _DISDisassembleChance;
    [SerializeField] private DIS_EnchantRemoval _DISEnchantRemoval;
    [SerializeField] private DIS_HitReduction _DISHitReduction;
    [SerializeField] private CFT_ReduceMaterialCost _CFTReduceMaterialCost;
    [SerializeField] private CFT_HitReduction _CFTHitReduction;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    public SaveSkillsObject SaveSkills()
    {
        SaveSkillsObject saveObject = new SaveSkillsObject
        {
            //spentSkillPoints = getSpentSkillPoints(),
            totalSkillPoints = _totalSkillPoints,
            currentSkillPoints = _currentSkillPoints,

            ECO_IncSellPricePoints = _ECOIncSellPrice.CurrentSkillLevel,
            ECO_DecBuyPricePoints = _ECODecBuyPrice.CurrentSkillLevel,
            ECO_HagglePoints = _ECOHaggleSuccess.CurrentSkillLevel,
            DIS_DisChancePoints = _DISDisassembleChance.CurrentSkillLevel,
            DIS_EnchRemovalPoints = _DISEnchantRemoval.CurrentSkillLevel,
            DIS_HitReductionPoints = _DISHitReduction.CurrentSkillLevel,
            CFT_ReduceMatCostPoints = _CFTReduceMaterialCost.CurrentSkillLevel,
            CFT_HitReductionPoints = _CFTHitReduction.CurrentSkillLevel,
        };
        return saveObject;
    }

    public void LoadSkills()
    {

    }

    /*
    public List<int> getSpentSkillPoints()
    {
        List<int> list = new List<int>();

        foreach(GameObject go in _economicSkills)
        {
            if (go.GetComponent<ECO_IncSellPrice>() == true)
                list.Add(go.GetComponent<ECO_IncSellPrice>().CurrentSkillLevel);
            if (go.GetComponent<ECO_DecBuyPrice>() == true)
                list.Add(go.GetComponent<ECO_DecBuyPrice>().CurrentSkillLevel);
            if (go.GetComponent<ECO_HaggleSuccess>() == true)
                list.Add(go.GetComponent<ECO_HaggleSuccess>().CurrentSkillLevel);
        }
        foreach(GameObject go in _disasemblySkills)
        {
            if (go.GetComponent<DIS_DisassembleChance>() == true)
                list.Add(go.GetComponent<DIS_DisassembleChance>().CurrentSkillLevel);
            if (go.GetComponent<DIS_EnchantRemoval>() == true)
                list.Add(go.GetComponent<DIS_EnchantRemoval>().CurrentSkillLevel);
            if (go.GetComponent<DIS_HitReduction>() == true)
                list.Add(go.GetComponent<DIS_HitReduction>().CurrentSkillLevel);
        }
        foreach(GameObject go in _craftingSkills)
        {
            if (go.GetComponent<CFT_ReduceMaterialCost>() == true)
                list.Add(go.GetComponent<CFT_ReduceMaterialCost>().CurrentSkillLevel);
            if (go.GetComponent<CFT_HitReduction>() == true)
                list.Add(go.GetComponent<CFT_HitReduction>().CurrentSkillLevel);
        }

        return list;
    }
    */

    public void updateSkillPoints()
    {
        _skillPointsText.text = _currentSkillPoints.ToString();
    }

    public bool hasFreeSkillPoint()
    {
        if (_currentSkillPoints >= 1)
            return true;
        return false;
    }


    public void setTotalSkillPoints(int value) { _totalSkillPoints = value; }
    public int GetTotalSkillPoints { get => _totalSkillPoints; }

    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
    public void AddSkillPoint() { _currentSkillPoints++; }
    public void RemoveSkillPoint() { _currentSkillPoints--; }
}
[System.Serializable]
public class SaveSkillsObject
{
    //public List<int> spentSkillPoints;
    public int totalSkillPoints;
    public int currentSkillPoints;

    public int ECO_IncSellPricePoints;
    public int ECO_DecBuyPricePoints;
    public int ECO_HagglePoints;

    public int DIS_DisChancePoints;
    public int DIS_EnchRemovalPoints;
    public int DIS_HitReductionPoints;

    public int CFT_ReduceMatCostPoints;
    public int CFT_HitReductionPoints;
}
