using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameControl : MonoBehaviour
{
    [Header("Crafting")]
    [SerializeField] private GameObject _craftingMinigameUI;
    [SerializeField] private CFT_HitReduction _craftHitSkill;
    [Header("Disassembly")]
    [SerializeField] private GameObject _disassemblyMinigameUI;
    [SerializeField] private DIS_HitReduction _disassembleHitSkill;
    [Header("Prefabs")]
    [SerializeField] private GameObject _hitPointPrefab;

    [Header("Data")]
    [SerializeField] private List<GameObject> _hitPointMarkers;
    [SerializeField] private List<int> chosenHitPoints;
    [Header("UI")]
    [SerializeField] private Text _pointsToHitText;
    [SerializeField] private Button _disassembleCompleteButton;
    [SerializeField] private Button _craftCompletButton;

    private UIControl _uiControlRef;

    private int baseHitPointCount = 5;
    private int finalPointsToHit = 5;
    private int pointsHit = 0;

    private void Awake()
    {
        _uiControlRef = gameObject.GetComponent<UIControl>();
        _craftingMinigameUI.SetActive(false);
        _disassemblyMinigameUI.SetActive(false);
        updateHitpointText();
        _disassembleCompleteButton.interactable = false;
        _craftCompletButton.interactable = false;
    }

    public void startCraftingMiniGame()
    {
        _uiControlRef.miniGameUIEnabled(true);
        _craftingMinigameUI.SetActive(true);
        _disassemblyMinigameUI.SetActive(false);
        populateHitPoints();
        if (this.gameObject.GetComponent<QuestControl>().CurrentQuest != null &&
            this.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial")
        {
            Debug.LogWarning("Quest Notif - Craft Minigame");
            this.gameObject.GetComponent<QuestControl>().nextStage();
        }
    }
    public void stopCraftingMiniGame()
    {
        _craftingMinigameUI.SetActive(false);
        _uiControlRef.itemCraftMenuEnabled(false);
        _uiControlRef.partCraftMenuEnabled(false);
        _uiControlRef.craftMenuEnabled(false);
        _uiControlRef.miniGameUIEnabled(false);
    }

    public void startDisassemblyMiniGame()
    {
        //Debug.Log("Disassembly MiniGame Start!");
        _uiControlRef.miniGameUIEnabled(true);
        _craftingMinigameUI.SetActive(false);
        _disassemblyMinigameUI.SetActive(true);
        populateHitPoints();
        if (this.gameObject.GetComponent<QuestControl>().CurrentQuest != null &&
            this.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial")
        {
            Debug.LogWarning("Quest Notif - Disassemble Minigame");
            this.gameObject.GetComponent<QuestControl>().nextStage();
        }
    }
    public void stopDisassembleMiniGame()
    {
        _disassemblyMinigameUI.SetActive(false);
        _uiControlRef.miniGameUIEnabled(false);
    }

    private void populateHitPoints()
    {
        resetHitPoints();
        chosenHitPoints.Clear();
        
        if (_disassemblyMinigameUI.activeInHierarchy == true)
            finalPointsToHit = calculateDisasembleHitPoints();
        else if (_craftingMinigameUI.activeInHierarchy == true)
            finalPointsToHit = calculateCraftHitPoints();
        
        for (int j = finalPointsToHit; j > 0; j--)
        {
            int k = Random.Range(0, _hitPointMarkers.Count);
            if (chosenHitPoints.Contains(k) == false)
                chosenHitPoints.Add(k);
            else if (chosenHitPoints.Contains(k) == true)
            {
                Debug.Log("MiniGameControl.chosenHitPoints contains " + k);
                j++;
            }
        }

        foreach (int l in chosenHitPoints)
        {
            GameObject ph = Instantiate(_hitPointPrefab);
            ph.transform.SetParent(_hitPointMarkers[l].transform, false);
        }
        
    }

    private int calculateDisasembleHitPoints()
    {
        int calc = baseHitPointCount - _disassembleHitSkill.CurrentSkillLevel;
        if (calc < 1) return 1;
        else return calc;
    }
    private int calculateCraftHitPoints()
    {
        int calc = baseHitPointCount - _craftHitSkill.CurrentSkillLevel;
        if (calc < 1) return 1;
        else return calc;
    }

    public void increaseHitCount()
    {
        pointsHit++;
        updateHitpointText();
        if (pointsHit == finalPointsToHit)
        {
            _disassembleCompleteButton.interactable = true;
            _craftCompletButton.interactable = true;
        }
    }

    public void resetHitPoints()
    {
        _disassembleCompleteButton.interactable = false;
        _craftCompletButton.interactable = false;

        foreach(int i in chosenHitPoints)
        {
            _hitPointMarkers[i].gameObject.GetComponent<HitPointMarker>().clearHitPoint();
        }
        chosenHitPoints.Clear();

        pointsHit = 0;
        updateHitpointText();
    }

    public void updateHitpointText()
    {
        _pointsToHitText.text = "points to hit (" + pointsHit + "/" + finalPointsToHit + ")";
    }
}
