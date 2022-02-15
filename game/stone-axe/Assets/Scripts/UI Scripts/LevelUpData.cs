using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelUpData", menuName = "ScriptableObjects/LevelUpData", order = 60)]
public class LevelUpData : ScriptableObject
{
    [SerializeField]
    public int _levelNumber;
    [SerializeField]
    public string _levelName;

    [Header("Level Unlocks")]
    [SerializeField]
    public List<ItemData> _itemRecipes;
    [SerializeField]
    public List<PartData> _partRecipes;
    [SerializeField]
    public List<MaterialData> _materials;

    public string LevelName { get => _levelName; }
    public int LevelNumber { get => _levelNumber; }
    public List<ItemData> ItemRecipeUnlocks { get => _itemRecipes; }
    public List<PartData> PartRecipeUnlocks { get => _partRecipes; }
    public List<MaterialData> MaterialUnlocks { get => _materials; }
}
