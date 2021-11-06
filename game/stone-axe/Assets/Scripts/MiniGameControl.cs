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
    [SerializeField, HideInInspector]private List<int> chosenHitPoints;
    [Header("UI")]
    [SerializeField] private Text _pointsToHitText;
    [SerializeField] private Button _disassembleCompleteButton;
    [SerializeField] private Button _craftCompletButton;

    private int baseHitPointCount = 5;
    private int pointsHit = 0;

    private void Awake()
    {
        _craftingMinigameUI.SetActive(false);
        _disassemblyMinigameUI.SetActive(false);
        updateHitpointText();
        _disassembleCompleteButton.interactable = false;
        _craftCompletButton.interactable = false;
    }

    public void startCraftingMiniGame()
    {
        _craftingMinigameUI.SetActive(true);
        _disassemblyMinigameUI.SetActive(false);
    }

    public void startDisassemblyMiniGame()
    {
        _craftingMinigameUI.SetActive(false);
        _disassemblyMinigameUI.SetActive(true);
        populateHitPoints();
    }

    private void populateHitPoints()
    {
        chosenHitPoints.Clear();
        
        for (int j = 0; j < baseHitPointCount; j++)
        {
            int k = Random.Range(0, _hitPointMarkers.Count);
            if (chosenHitPoints.Contains(k) == false)
                chosenHitPoints.Add(k);
            else if (chosenHitPoints.Contains(k))
                j--;
        }

        foreach(int l in chosenHitPoints)
        {
            GameObject ph = Instantiate(_hitPointPrefab);
            ph.transform.SetParent(_hitPointMarkers[l].transform, false);
        }
    }

    public void increaseHitCount()
    {
        pointsHit++;
        updateHitpointText();
        if (pointsHit == baseHitPointCount)
        {
            _disassembleCompleteButton.interactable = true;
            _craftCompletButton.interactable = true;
        }
    }

    public void resetHitPoints()
    {
        _disassembleCompleteButton.interactable = false;
        _craftCompletButton.interactable = false;

        pointsHit = 0;
    }

    public void updateHitpointText()
    {
        _pointsToHitText.text = "points to hit (" + pointsHit + "/" + baseHitPointCount + ")";
    }
}
