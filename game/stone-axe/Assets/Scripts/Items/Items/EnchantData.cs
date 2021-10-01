using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnchantData", menuName = "EnchantData", order = 54)]
public class EnchantData : ScriptableObject
{
    [Header("Enchant Data")]
    [SerializeField] private string _enchantName;
    [SerializeField] private int _minBuff;
    [SerializeField] private int _maxBuff;
    [SerializeField] private int _baseValuePerLevel;

    private enum enchantBuffType
    {
        StatBuff_STR,
        StatBuff_DEX,
        StatBuff_INT,
        StatBuff_DEF_GEN,
        OtherBuff_FIRE_DMG,
        OtherBuff_ICE_DMG
    }
    [Header("Enchant Stats")]
    [SerializeField] private enchantBuffType _enchantBuffType;

    public string EnchantName { get => _enchantName; }
    public string EnchantType { get => _enchantBuffType.ToString(); } 
    public int GetRandomBuff { get => Random.Range(_minBuff, _maxBuff+1); }
    public int BaseAddedValuePerLevel { get => _baseValuePerLevel; }
}
