using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_GenerateItems : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private GenerateItem _generateItemRef;
    [SerializeField] private int _itemsToGenerateCount = 3;
    [Header("Parts")]
    [SerializeField] private DisassembleItemControl _dissasembleItemRef;
    [SerializeField] private int _totalItemPartsToGenerateCount = 3;
    [Header("Enchants")]
    [SerializeField] private int _totalEnchantsToGenerate = 1;

    void Start()
    {
        /*
        if (_generateItemRef != null)
        {
            for (int i = 0; i < _itemsToGenerateCount; i++)
            {
                _generateItemRef.GenerateRandomItem();
                _generateItemRef.forceInsertItem();
                //Debug.Log("Creating item #" + (i+1));
            }
        }
        */
        if (GameObject.FindGameObjectWithTag("GameMaster").GetComponent<UIControl>().ShopUIActive)
        {
            StartCoroutine(generateItems());
            if (_generateItemRef != null)
            {
                for (int j = 0; j < _totalEnchantsToGenerate; j++)
                {
                    _generateItemRef.GenerateRandomEnchant();
                    _generateItemRef.forceInsertEnchant();
                }
            }

            if (_dissasembleItemRef != null)
            {
                for (int p = 0; p < _totalItemPartsToGenerateCount; p++)
                {
                    _generateItemRef.GenerateRandomItem();
                    _generateItemRef.forceDisassembleItem();
                    //Debug.Log("Creating and disassembling item #" + (p+1));
                }
            }
        }
        
    }

    private IEnumerator generateItems()
    {
        if (_generateItemRef != null)
        {
            for (int i = 0; i < _itemsToGenerateCount; i++)
            {
                _generateItemRef.GenerateRandomItem();
                _generateItemRef.forceInsertItem();
                //Debug.Log("Creating item #" + (i+1));
            }
        }
        yield return null;
    }
}
