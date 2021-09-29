using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantDataStorage : MonoBehaviour
{
    [Header("Enchant Data")]
    [SerializeField] private string _enchantName;

    /*
    private enum enchantBuffType
    {
        StatBuff_STR,
        StatBuff_DEX,
        StatBuff_INT,
        StatBuff_DEF_GEN,
        OtherBuff_FIRE_DMG,
        OtherBuff_ICE_DMG
    }
    [SerializeField] private enchantBuffType _enchantBuffType;
    */
    [Header("Enchant Stats")]
    [SerializeField] private string _enchantBuffType;
    [SerializeField] private int _valueOfBuff;

    public string EnchantName { get => _enchantName; }
    public void setEnchantName(string name) { _enchantName = name; }

    public string EnchantType { get => _enchantBuffType; }
    public void setEnchantType(string type) { _enchantBuffType = type; }

    public int AmountOfBuff { get => _valueOfBuff; }
    public void setAmountOfBuff(int value) { _valueOfBuff = value; }
}
