using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class Dev_jsonConverter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Item itemScriptRef;
    [SerializeField] private Part partScriptRef;

    [Header("Data")]
    [SerializeField] private ItemData _itemToConvert;
    [SerializeField] private PartData _partToConvert;
    [SerializeField] private string itemDirPath;
    [SerializeField] private string partDirPath;
    [SerializeField] private string questDirPath;

    [SerializeField] private bool replace;

    [SerializeField] private string itemSOPath;
    [SerializeField] private string partSOPath;

    [SerializeField] private List<ItemData> items;
    [SerializeField] private List<PartData> parts;
    [SerializeField] private List<QuestData> quests;

    [SerializeField] private List<ItemJsonData> itemJsons;
    [SerializeField] private string[] itemsJ;
    // Start is called before the first frame update
    void Start()
    {
        /*string path = Application.dataPath + AssetDatabase.GetAssetPath(GameObject.FindGameObjectWithTag("ItemData").GetComponent<Item>().getItemJsonData()[0]).Replace("Assets", "");
        Debug.Log("path: " + path);

        string itemString = File.ReadAllText(path);

        ItemJsonData item = JsonUtility.FromJson<ItemJsonData>(itemString);        
        Debug.Log(item.itemName); */
        //convertManyItemToJson();
        convertManyQuestToJson();

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

        List<PartJsonData> partJsonList = new List<PartJsonData>();
        if (item.Part1 != null) partJsonList.Add(partScriptRef.getPartJsonData(item.Part1.PartName));
        if (item.Part2 != null) partJsonList.Add(partScriptRef.getPartJsonData(item.Part2.PartName));
        if (item.Part3 != null) partJsonList.Add(partScriptRef.getPartJsonData(item.Part3.PartName));

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
    
    private string convertQuestToJson(QuestData quest)
    {

        List<string> questStages = new List<string>();
        foreach (QuestStage stage in quest.QuestStages) { questStages.Add(stage.name); }

        QuestJsonData questData = new QuestJsonData
        {
            questID = -1,
            questName = quest.QuestName,
            requiredPlayerLevel = quest.RequiredPlayerLevel,
            questDescription = quest.QuestDiscription,
            questType = quest.QuestType,
            currencyReward = quest.RewardedCurrency,
            EXPReward = quest.RewardedEXP,
            questStages = questStages
        };

        string json = JsonUtility.ToJson(questData, true);
        string questPath = questDirPath + "/" + quest.QuestName + ".json";
        File.WriteAllText(questPath, json);
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

    private string convertManyQuestToJson()
    {
        foreach (QuestData quest in quests)
        {
            Debug.Log(quest.QuestName);
            convertQuestToJson(quest);
        }
        return "";
    }
}
