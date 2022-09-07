using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Dev_jsonConverter : MonoBehaviour
{
    [SerializeField] private ItemData _itemToConvert;
    [SerializeField] private PartData _partToConvert;
    [SerializeField] private string itemDirPath;
    [SerializeField] private string partDirPath;

    [SerializeField] private bool replace;

    [SerializeField] private string itemSOPath;
    [SerializeField] private string partSOPath;

    [SerializeField] private List<ItemData> items;
    [SerializeField] private List<PartData> parts;

    [SerializeField] private List<ItemJsonData> itemJsons;
    [SerializeField] private string[] itemsJ;
    // Start is called before the first frame update
    void Start()
    {
        if (_itemToConvert != null) convertItemToJson(_itemToConvert);
        if (_partToConvert != null) convertPartToJson(_partToConvert);
    }

    private string convertPartToJson(PartData part)
    {
        List<string> mats = new List<string>();
        foreach (MaterialData mat in part.ValidMaterialData) { mats.Add(mat.Material); }

        List<string> filterList = new List<string>();
        foreach (FilterData fil in part.ValidFilters) { filterList.Add(fil.FilterName); }

        PartJsonData partData = new PartJsonData
        {
            partID = -1,
            partName = part.PartName,
            levelRequirement = part.PartLevelReq,
            validMaterials = mats,
            validMaterialTypes = part.ValidMaterials,
            unitsOfMaterialNeeded = part.UnitsOfMaterialNeeded,
            baseCost = part.BaseCost,
            baseStrength = part.BaseStrenght,
            baseDextarity = part.BaseDextarity,
            baseIntelligence = part.BaseIntelligence,
            filters = filterList
        };
        string json = JsonUtility.ToJson(partData, true);

        string partPath = partDirPath + "/" + part.PartName + ".json";

        File.WriteAllText(partPath, json);

        return json;
    }

    private string convertItemToJson(ItemData item)
    {
        List<string> partList = new List<string>();
        if (item.Part1 != null) partList.Add(item.Part1.PartName);
        if (item.Part2 != null) partList.Add(item.Part2.PartName);
        if (item.Part3 != null) partList.Add(item.Part3.PartName);

        List<string> filterList = new List<string>();
        foreach (FilterData fil in item.ValidFilters) { filterList.Add(fil.FilterName); }

        ItemJsonData itemData = new ItemJsonData
        {
            itemID = -1,
            itemName = item.ItemName,
            levelRequirement = item.ItemLevel,
            requiredParts = partList,
            baseCost = item.BaseCost,
            baseStrength = item.BaseStrength,
            baseDextarity = item.BaseDextarity,
            baseIntelligence = item.BaseIntelligence,
            filters = filterList
        };
        string json = JsonUtility.ToJson(itemData, true);

        string itemPath = itemDirPath + "/" + item.ItemName + ".json";

        File.WriteAllText(itemPath, json);

        return json;
    }

    private string convertManyItemToJson()
    {
        foreach (ItemData item in items)
        {
            Debug.Log(item.ItemName);
            convertItemToJson(item);
        }
        return "";
    }

    private string convertManyPartToJson()
    {
        foreach (PartData part in parts)
        {
            Debug.Log(part.PartName);
            convertPartToJson(part);
        }
        return "";
    }
}
