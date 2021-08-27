using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Tracking")]
    [SerializeField] List<GameObject> mainItemGroups;
    [SerializeField] List<ItemData> _itemDataList;
    int ranItem;
    string textReturn = "";

    public string chooseItem()
    {
        textReturn = "";
        ranItem = Random.Range(0, _itemDataList.Count);
        Debug.Log("chosen item: " + _itemDataList[ranItem].ItemName);
        textReturn += "Item - " + _itemDataList[ranItem].ItemName
            + "\nStats\nStrenght: " + _itemDataList[ranItem].TotalStrength + "\nDextarity: " + _itemDataList[ranItem].TotalDextarity + "\nIntelegence: " + _itemDataList[ranItem].TotalIntelegence
            + "\n\nMaterials\n" + _itemDataList[ranItem].Materials
            + "\n\nValue: " + _itemDataList[ranItem].TotalValue;

        return textReturn;
    }
}

/*
        ranItem = Random.Range(0, mainItemGroups.Count);
        Debug.Log("chosen item: " + mainItemGroups[ranItem].name);
        textReturn += "Item - ";
        if (ranItem == 0) // weapons
        {
            textReturn += "Weapon: \n "
                + mainItemGroups[ranItem].gameObject.GetComponent<Weapon>().chooseWeapon();
        }
        else if (ranItem == 1) // armor
        {

        }
        else if (ranItem == 3) // trinket
        {

        }
        */
