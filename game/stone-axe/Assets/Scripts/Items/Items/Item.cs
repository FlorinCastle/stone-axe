using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Tracking")]
    //[SerializeField] List<GameObject> mainItemGroups;
    [SerializeField] List<ItemData> _itemDataList;
    int ranItem;
    ItemData _generatedItem;
    string textReturn = "";

    private string _itemName;
    private string _materials;
    private string _totalStrenght;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;

    public ItemData chooseItem()
    {
        ranItem = Random.Range(0, _itemDataList.Count);
        _generatedItem = _itemDataList[ranItem];

        return _generatedItem;
    }

    public string silence()
    {
        // generate the item
        ranItem = Random.Range(0, _itemDataList.Count);
        _generatedItem = _itemDataList[ranItem];

        //  store text values (this always comes after the item is generated)
        //  DO NOT REARANGE THE ORDER OR IT WILL MESS UP THE ITEM STAT BLOCK
        _itemName = "Item - " + _generatedItem.ItemName;
        _materials = "\n\nMaterials\n" + _generatedItem.Materials; // calling this will generate the materials used for the item. DO NOT MOVE IT LOWER IN THE ORDER OR IT WILL BREAK STUFF
        _totalStrenght = "\nStrenght: " + _generatedItem.TotalStrength;
        _totalDex = "\nDextarity: " + _generatedItem.TotalDextarity;
        _totalInt = "\nIntelegence: " + _generatedItem.TotalIntelegence;
        _totalValue = "\n\nValue: " + _generatedItem.TotalValue;

        textReturn = "";    // clear return text
        textReturn += _itemName
            + "\nStats" + _totalStrenght
            + _totalDex
            + _totalInt
            + _materials
            + _totalValue;

        return textReturn;
    }

    public List<ItemData> getItemDataRef()
    {
        return _itemDataList;
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
