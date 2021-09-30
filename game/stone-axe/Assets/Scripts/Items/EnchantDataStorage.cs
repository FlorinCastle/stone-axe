using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantDataStorage : MonoBehaviour
{
    [Header("Enchant Data")]
    [SerializeField] private string _enchantName;
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
