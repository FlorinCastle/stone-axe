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
    // Start is called before the first frame update
    void Start()
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