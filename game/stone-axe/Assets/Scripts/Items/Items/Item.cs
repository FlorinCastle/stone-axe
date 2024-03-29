using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Tracking")]
    [SerializeField] private ItemJsonPaths pathDatabase;
    //[SerializeField] List<GameObject> mainItemGroups;
    [SerializeField] List<ItemData> _itemDataList;
    int ranItem;
    ItemData _generatedItem;
    ItemJsonDataCode _generatedItemJsonData;

    [SerializeField] List<TextAsset> itemJsons;
    [SerializeField] List<ItemJsonData> items;
    /*
    string textReturn = "";

    private string _itemName;
    private string _materials;
    private string _totalStrenght;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;
    */
    public ItemData chooseItem()
    {
        //generateItem();
        ranItem = Random.Range(0, _itemDataList.Count);
        _generatedItem = _itemDataList[ranItem];

        return _generatedItem;
    }
    public ItemJsonDataCode chooseItemJson()
    {
        Debug.LogWarning("Item.chooseItemJson(): test");
        ranItem = Random.Range(0, itemJsons.Count);
        _generatedItemJsonData = JsonUtility.FromJson<ItemJsonDataCode>(itemJsons[ranItem].text);


        //File.ReadAllText(Application.dataPath + pathDatabase.getItemJsonPath(itemJsons[ranItem]).Replace("Assets", "")));

        //_generatedItemJsonData = JsonUtility.FromJson<ItemJsonDataCode>(File.ReadAllText(Application.dataPath +  pathDatabase.getItemJsonPath(itemJsons[ranItem]).Replace("Assets", "")));
        //_generatedItemJsonData = JsonUtility.FromJson<ItemJsonDataCode>(File.ReadAllText(Application.dataPath + Resources.Load();//Resources.Load(AssetDatabase.GetAssetPath(itemJsons[ranItem]).Replace("Assets",""))));

        return _generatedItemJsonData;
    }
    public ItemJsonDataCode getItemJsonData(string itemName) //(int itemIDin) 
    {
        foreach (TextAsset item in itemJsons)
        {
            string itemCheck = loadJson(item).itemName;
            //int ID = loadJson(item).itemID;
            if (itemCheck == itemName)
                return JsonUtility.FromJson<ItemJsonDataCode>(item.text);//File.ReadAllText(Application.dataPath + pathDatabase.getItemJsonPath(item).Replace("Assets", "")));

                    //JsonUtility.FromJson<ItemJsonDataCode>(File.ReadAllText(Application.dataPath + pathDatabase.getItemJsonPath(item).Replace("Assets", "")));
                    //JsonUtility.FromJson<ItemJsonDataCode>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(item).Replace("Assets", "")));
        }
        Debug.LogWarning("Unable to find Valid Item for ID: [" + itemName + "]");
        return null;
    } 
    /*public ItemJsonData getItemJsonData(string itemName) //(int itemIDin) 
    {
        foreach(TextAsset item in itemJsons)
        {
            string itemCheck = loadJson(item).itemName;
            //int ID = loadJson(item).itemID;
            if (itemCheck == itemName)
                return loadJson(item);
        }
        Debug.LogWarning("Unable to find Valid Item for ID: [" + itemName + "]");
        return null;
    } */

    /*
    public string silence()
    {
        //  store text values (this always comes after the item is generated)
        //  DO NOT REARANGE THE ORDER OR IT WILL MESS UP THE ITEM STAT BLOCK
        _itemName = "Item - " + _generatedItem.ItemName;
        _materials = "\n\nMaterials\n" + _generatedItem.RandomMaterials; // calling this will generate the materials used for the item. DO NOT MOVE IT LOWER IN THE ORDER OR IT WILL BREAK STUFF
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
    */

    public List<ItemData> getItemDataRef() { return _itemDataList; }
    public List<TextAsset> getItemJsonData() { return itemJsons; }

    private ItemJsonData loadJson(TextAsset jsonText)
    {
        return JsonUtility.FromJson<ItemJsonData>(jsonText.text);
            //File.ReadAllText(Application.dataPath + pathDatabase.getItemJsonPath(jsonText).Replace("Assets", "")));
        //JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + pathDatabase.getItemJsonPath(jsonText).Replace("Assets", "")));
        //JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonText).Replace("Assets", "")));
    }
}