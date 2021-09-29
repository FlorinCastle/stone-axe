using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enchant : MonoBehaviour
{
    [Header("Enchant Tracking")]
    [SerializeField] List<EnchantData> _enchantDataList;
    int ranEnchant;
    EnchantData _generatedEnchant;

    public EnchantData chooseEnchant()
    {
        ranEnchant = Random.Range(0, _enchantDataList.Count);
        _generatedEnchant = _enchantDataList[ranEnchant];

        return _generatedEnchant;
    }
}
