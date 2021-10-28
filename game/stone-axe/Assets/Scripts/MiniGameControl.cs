using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        _craftingMinigameUI.SetActive(false);
        _disassemblyMinigameUI.SetActive(false);
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
    }
}
