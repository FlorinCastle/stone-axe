using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private GameMaster _gameMasterRef;
    [SerializeField] private int _totalSkillPoints;
    [SerializeField] private int _currentSkillPoints;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _skillPointsText;
    [SerializeField] private GameObject _economicSkillsContent;
    [SerializeField] private GameObject _disassemblySkillsContent;
    [SerializeField] private GameObject _craftingSkillsContent;

    [Header("Prefabs")]
    [SerializeField] private GameObject _skillPrefab;

    [Header("Skill Tracking")]
    [SerializeField] private ECO_IncSellPrice _ECOIncSellPrice;
    [SerializeField] private ECO_DecBuyPrice _ECODecBuyPrice;
    [SerializeField] private ECO_HaggleSuccess _ECOHaggleSuccess;
    [SerializeField] private ECO_EnchBoostedChance _ECOEnchBoostedChance;
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

    public void LoadSkills(SaveSkillsObject skills)
    {
        _totalSkillPoints = skills.totalSkillPoints;
        _currentSkillPoints = skills.currentSkillPoints;
        
        _ECOIncSellPrice.CurrentSkillLevel = skills.ECO_IncSellPricePoints;
        _ECOIncSellPrice.updateSkillTexts();
        _ECODecBuyPrice.CurrentSkillLevel = skills.ECO_DecBuyPricePoints;
        _ECODecBuyPrice.updateSkillTexts();
        _ECOHaggleSuccess.CurrentSkillLevel = skills.ECO_HagglePoints;
        _ECOHaggleSuccess.updateSkillTexts();
        _DISDisassembleChance.CurrentSkillLevel = skills.DIS_DisChancePoints;
        _DISDisassembleChance.updateSkillTexts();
        _DISEnchantRemoval.CurrentSkillLevel = skills.DIS_EnchRemovalPoints;
        _DISEnchantRemoval.updateSkillTexts();
        _DISHitReduction.CurrentSkillLevel = skills.DIS_HitReductionPoints;
        _DISHitReduction.updateSkillTexts();
        _CFTReduceMaterialCost.CurrentSkillLevel = skills.CFT_ReduceMatCostPoints;
        _CFTReduceMaterialCost.updateSkillTexts();
        _CFTHitReduction.CurrentSkillLevel = skills.CFT_HitReductionPoints;
        _CFTHitReduction.updateSkillTexts();


    }
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

    public void setupSkillUI()
    {
        //Debug.LogError("SkillManager.setupSkillUI(): PUT THE CODE FOR CONTROLLING TURNING ON AND OFF THE SKILL POINTS UI HERE!");
        /* GO THROUGH EACH SKILL SCRIPT AND TURN ON/OFF THE ADD AND MINUS BUTTONS FOR POINTS
         */

        _ECOIncSellPrice.checkButtons();
        _ECODecBuyPrice.checkButtons();
        _ECOHaggleSuccess.checkButtons();
        _ECOEnchBoostedChance.checkButtons();
        _DISDisassembleChance.checkButtons();
        _DISEnchantRemoval.checkButtons();
        _DISHitReduction.checkButtons();
        _CFTReduceMaterialCost.checkButtons();
        _CFTHitReduction.checkButtons();
    }

    public void setTotalSkillPoints(int value) { _totalSkillPoints = value; }
    public int GetTotalSkillPoints { get => _totalSkillPoints; }

    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
    public void AddSkillPoint() { _currentSkillPoints++; }
    public void RemoveSkillPoint() { _currentSkillPoints--; }

    public ECO_IncSellPrice IncreaseSellPriceRef { get => _ECOIncSellPrice; }
    public ECO_DecBuyPrice DecreaseBuyPriceRef { get => _ECODecBuyPrice; }
    public ECO_HaggleSuccess HagglePriceRef { get => _ECOHaggleSuccess; }
    public ECO_EnchBoostedChance EnchantChanceRef { get => _ECOEnchBoostedChance; }
    public DIS_DisassembleChance DisassembleItemRef { get => _DISDisassembleChance; }
    public DIS_EnchantRemoval RetreaveEnchantRef { get => _DISEnchantRemoval; }
    public DIS_HitReduction DisassembleHitReductionRef { get => _DISHitReduction; }
    public CFT_ReduceMaterialCost ReduceMatUseRef { get => _CFTReduceMaterialCost; }
    public CFT_HitReduction CraftHitReductionRef { get => _CFTHitReduction; }
}
[System.Serializable]
public class SaveSkillsObject
{
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
