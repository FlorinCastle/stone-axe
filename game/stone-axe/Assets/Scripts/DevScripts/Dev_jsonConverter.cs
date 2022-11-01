using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using static QuestStage;
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
        //convertManyQuestToJson();

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
        /*List<QuestStageJsonData> questStagesJson = new List<QuestStageJsonData>();
        foreach (QuestStage stage in quest.QuestStages)
        {
            //questStages.Add(stage.name);
            questStagesJson.Add(convertQuestStageToJson(stage));
        }*/

        List<QuestStageJsonData> questStagesList = new List<QuestStageJsonData>();
        if (quest.QuestStages.Count >= 1)
        {
            foreach (QuestStage stage in quest.QuestStages)
            {
                //questStages.Add(stage.name);

                //var questStageJsonData = convertQuestStageToJson(stage); // important

                //string qStageJson = JsonUtility.ToJson(questStageJsonData, false);
                //questStagesList.Add(questStageJsonData);

                //questStages.Add(questStageJsonData); // important

                //Debug.Log(questStageJsonData);
                //Debug.Log(qStageJson);
            }
        }

        string json = "";
        if (quest.QuestType == "OCC_Item")
        {
            CraftItemQuest questData = new CraftItemQuest
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                requiredItem = quest.RequiredItem.ItemName,
            };
            json = JsonUtility.ToJson(questData, true);
        }
        else if (quest.QuestType == "OCC_QuestItem")
        {
            CraftQuestItemQuest questData = new CraftQuestItemQuest
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                requiredQuestItem = quest.RequiredQuestItem.QuestItemName,
            };
            json = JsonUtility.ToJson(questData, true);
        }
        else if (quest.QuestType == "OD_Material")
        {
            HaveMaterialQuest questData = new HaveMaterialQuest
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                requiredMaterial = quest.ReqiredMaterial.Material,
                requiredCount = quest.ReqiredCount,
            };
            json = JsonUtility.ToJson(questData, true);
        }
        else if (quest.QuestType == "OCC_TotalCrafted")
        {
            CraftManyItemQuest questData = new CraftManyItemQuest
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                requiredItem = quest.RequiredItem.ItemName,
                requiredCount = quest.ReqiredCount,
            };
            json = JsonUtility.ToJson(questData, true);
        }
        else if (quest.QuestType == "Tutorial")
        {
            //Debug.LogWarning("Converting Tutorial Quest: " + quest.QuestName);
            string nextQuest = "";
            if (quest.NextQuest != null)
                nextQuest = quest.NextQuest.QuestName;
            TutorialQuest questData = new TutorialQuest
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                questStages = questStages,
                questStagesJson = questStagesList,
                nextQuest = nextQuest,
                unlockFeatures = new List<string>(),
            };
            json = JsonUtility.ToJson(questData, true);
        }
        else if (quest.QuestType == "Story")
        {
            string nextQuest = "";
            if (quest.NextQuest != null)
                nextQuest = quest.NextQuest.QuestName;
            StoryQuest questData = new StoryQuest
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                questStages = questStages,
                //questStagesJson = questStagesJson,
                nextQuest = nextQuest,
            };
            json = JsonUtility.ToJson(questData, true);
        }
        else
        {
            BaseQuestJsonData questData = new BaseQuestJsonData
            {
                questID = -1,
                questName = quest.QuestName,
                requiredPlayerLevel = quest.RequiredPlayerLevel,
                questDescription = quest.QuestDiscription,
                questType = quest.QuestType,
                currencyReward = quest.RewardedCurrency,
                EXPReward = quest.RewardedEXP,
                //questStages = questStages,
                //questStagesJson = questStagesJson
            };
            json = JsonUtility.ToJson(questData, true);
        }
        

        //Debug.Log(json);
        if (questDirPath != "")
        {
            string questPath = questDirPath + "/" + quest.QuestName + ".json";
            File.WriteAllText(questPath, json);
        }
        return json;
    }
    /*private QuestStageJsonData convertQuestStageToJson(QuestStage stage)
    {
        List<string> mats = new List<string>();
        if (stage.Part1Mat != null) mats.Add(stage.Part1Mat.Material);
        if (stage.Part2Mat != null) mats.Add(stage.Part2Mat.Material);
        if (stage.Part3Mat != null) mats.Add(stage.Part3Mat.Material);

        Debug.Log(stage.name + ": "+ stage.StageType);

        if (stage.StageType == "Not_Set")
        {
            var questStageJsonData = new QuestStageJsonData()
            {
                questStageType = stage.StageType
            };
            return questStageJsonData;
        }
        else if (stage.StageType == "Dialogue")
        {
            var questStageJsonData = new DialogeStage()
            {
                questStageType = stage.StageType,
                speaker = stage.DialogueSpeaker,
                dialogeLine = stage.DialogueLine,
            };

            Debug.Log(JsonUtility.ToJson(questStageJsonData));

            return questStageJsonData;

        }
        else if (stage.StageType == "Craft_Item")
        {
            CraftItemStage questStageJsonData = new CraftItemStage()
            {
                questStageType = stage.StageType,
                itemName = stage.ItemToGet.ItemName
            };
            return questStageJsonData;
        }
        else if (stage.StageType == "Sell_Item")
        {
            SellItemStage questStageJsonData = new SellItemStage()
            {
                questStageType = stage.StageType
            };
            return questStageJsonData;
        }
        else if (stage.StageType == "Buy_Item")
        {
            BuyItemStage questStageJsonData = new BuyItemStage()
            {
                questStageType = stage.StageType
            };
            return questStageJsonData;
        }
        else if (stage.StageType == "Disassemble_Item")
        {
            DisassembleItemStage questStageJsonData = new DisassembleItemStage()
            {
                questStageType = stage.StageType
            };
            return questStageJsonData;
        }
        else if (stage.StageType == "Have_Currency")
        {
            HaveCurrencyStage questStageJsonData = new HaveCurrencyStage()
            {
                questStageType = stage.StageType,
                currencyvalue = stage.CurrencyValue
            };
            return questStageJsonData;
        }
        else if (stage.StageType == "Force_Event")
        {
            ForceEventStage questStageJsonData = new ForceEventStage()
            {
                questStageType = stage.StageType
            };

            if (stage.QuestEvent == "Summon_Adventurer") 
            {
                SummonAdventurerEvent stageEvent = new SummonAdventurerEvent() { };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Get_Item")
            {
                GetItemEvent stageEvent = new GetItemEvent()
                {
                    itemName = stage.ItemToGet.ItemName,
                    partMats = mats,
                    itemCount = stage.CountToGet
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Remove_Quest_Item")
            {
                RemoveQuestItemsEvent stageEvent = new RemoveQuestItemsEvent() { };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Get_Currency")
            {
                GetCurrencyEvent stageEvent = new GetCurrencyEvent()
                {
                    currencyvalue = stage.CurrencyValue
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Remove_Currency")
            {
                RemoveCurrencyEvent stageEvent = new RemoveCurrencyEvent()
                {
                    currencyvalue = stage.CurrencyValue
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Summon_NPC")
            {
                SummonNPCEvent stageEvent = new SummonNPCEvent()
                {
                    NPCRef = stage.NPCRef.name,
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Dismiss_Quest_NPC")
            {
                GetItemEvent stageEvent = new GetItemEvent() { };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Force_For_Sale")
            {
                ForceForSaleEvent stageEvent = new ForceForSaleEvent()
                {
                    itemName = stage.ItemToGet.ItemName,
                    partMats = mats
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Force_Open_UI")
            {
                ForceOpenUIEvent stageEvent = new ForceOpenUIEvent()
                {
                    forcedUI = stage.ForcedUI
                };
                questStageJsonData.questEvent = stageEvent;
            }

            return questStageJsonData;
        }
        else if (stage.StageType == "Have_UI_Open")
        {
            HaveUIOpenStage questStageJsonData = new HaveUIOpenStage()
            {
                questStageType = stage.StageType,
                reqUI = stage.RequiredUI
            };
            return questStageJsonData;
        }
        else Debug.LogWarning("Dev_jsonConverter.convertQuestStageToJson(QuestStage stage): variable 'stage' does not contain a valid StageType!");


        //Debug.LogError("Dev_jsonConverter.convertQuestStageToJson(QuestStage stage): KAT! FINISH WORKING ON THIS");
        return null;
    } */
    /* TODO Fix this
    private string convertQuestStageToJson(QuestStage stage)
    {
        List<string> mats = new List<string>();
        if (stage.Part1Mat != null) mats.Add(stage.Part1Mat.Material);
        if (stage.Part2Mat != null) mats.Add(stage.Part2Mat.Material);
        if (stage.Part3Mat != null) mats.Add(stage.Part3Mat.Material);

        Debug.Log(stage.name + ": " + stage.StageType);

        if (stage.StageType == "Not_Set")
        {
            var questStageJsonData = new QuestStageJsonData()
            {
                questStageType = stage.StageType
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Dialogue")
        {
            var questStageJsonData = new DialogeStage()
            {
                questStageType = stage.StageType,
                speaker = stage.DialogueSpeaker,
                dialogeLine = stage.DialogueLine,
            };

            Debug.Log(JsonUtility.ToJson(questStageJsonData, true));

            return JsonUtility.ToJson(questStageJsonData, true);

        }
        else if (stage.StageType == "Craft_Item")
        {
            CraftItemStage questStageJsonData = new CraftItemStage()
            {
                questStageType = stage.StageType,
                itemName = stage.ItemToGet.ItemName
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Sell_Item")
        {
            SellItemStage questStageJsonData = new SellItemStage()
            {
                questStageType = stage.StageType
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Buy_Item")
        {
            BuyItemStage questStageJsonData = new BuyItemStage()
            {
                questStageType = stage.StageType
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Disassemble_Item")
        {
            DisassembleItemStage questStageJsonData = new DisassembleItemStage()
            {
                questStageType = stage.StageType
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Have_Currency")
        {
            HaveCurrencyStage questStageJsonData = new HaveCurrencyStage()
            {
                questStageType = stage.StageType,
                currencyvalue = stage.CurrencyValue
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Force_Event")
        {
            ForceEventStage questStageJsonData = new ForceEventStage()
            {
                questStageType = stage.StageType
            };

            if (stage.QuestEvent == "Summon_Adventurer")
            {
                SummonAdventurerEvent stageEvent = new SummonAdventurerEvent() { };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Get_Item")
            {
                GetItemEvent stageEvent = new GetItemEvent()
                {
                    itemName = stage.ItemToGet.ItemName,
                    partMats = mats,
                    itemCount = stage.CountToGet
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Remove_Quest_Item")
            {
                RemoveQuestItemsEvent stageEvent = new RemoveQuestItemsEvent() { };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Get_Currency")
            {
                GetCurrencyEvent stageEvent = new GetCurrencyEvent()
                {
                    currencyvalue = stage.CurrencyValue
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Remove_Currency")
            {
                RemoveCurrencyEvent stageEvent = new RemoveCurrencyEvent()
                {
                    currencyvalue = stage.CurrencyValue
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Summon_NPC")
            {
                SummonNPCEvent stageEvent = new SummonNPCEvent()
                {
                    NPCRef = stage.NPCRef.name,
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Dismiss_Quest_NPC")
            {
                GetItemEvent stageEvent = new GetItemEvent() { };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Force_For_Sale")
            {
                ForceForSaleEvent stageEvent = new ForceForSaleEvent()
                {
                    itemName = stage.ItemToGet.ItemName,
                    partMats = mats
                };
                questStageJsonData.questEvent = stageEvent;
            }
            else if (stage.QuestEvent == "Force_Open_UI")
            {
                ForceOpenUIEvent stageEvent = new ForceOpenUIEvent()
                {
                    forcedUI = stage.ForcedUI
                };
                questStageJsonData.questEvent = stageEvent;
            }

            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else if (stage.StageType == "Have_UI_Open")
        {
            HaveUIOpenStage questStageJsonData = new HaveUIOpenStage()
            {
                questStageType = stage.StageType,
                reqUI = stage.RequiredUI
            };
            return JsonUtility.ToJson(questStageJsonData, true);
        }
        else Debug.LogWarning("Dev_jsonConverter.convertQuestStageToJson(QuestStage stage): variable 'stage' does not contain a valid StageType!");

        return null;

    } */

    /* old code from convertQuestStageToJson()
     * QuestStageJsonData stageData = new QuestStageJsonData
    {
        questStageType = stage.StageType,
        //questEvent = stage.QuestEvent,
        //forcedUI = stage.ForcedUI,
        //reqUI = stage.RequiredUI,
        //speaker = stage.DialogueSpeaker,
        //dialogeLine = stage.DialogueLine,
        //itemName = stage.ItemToGet.ItemName,
        //itemCount = stage.CountToGet,
        //partMats = mats,
        //currencyvalue = stage.CurrencyValue,
        //NPCRef = stage.NPCRef.name
    };
    //Debug.Log(JsonUtility.ToJson(stageData, true));

    /*
    if (stage.StageType == "Dialogue") { stageData.speaker = stage.DialogueSpeaker; stageData.dialogeLine = stage.DialogueLine; }
    else if (stage.StageType == "Craft_Item") { }
    else if (stage.StageType == "Sell_Item") { }
    else if (stage.StageType == "Buy_Item") { }
    else if (stage.StageType == "Disassemble_Item") { }
    else if (stage.StageType == "Have_Currency") { }
    else if (stage.StageType == "Force_Event")
    {
        stageData.questEvent = stage.QuestEvent;
        if (stage.QuestEvent == "Summon_Adventurer") { }
        else if (stage.QuestEvent == "Get_Item") { }
        else if (stage.QuestEvent == "Remove_Quest_Item") { }
        else if (stage.QuestEvent == "Get_Currency") { }
        else if (stage.QuestEvent == "Remove_Currency") { }
        else if (stage.QuestEvent == "Summon_NPC") { }
        else if (stage.QuestEvent == "Dismiss_Quest_NPC") { stageData.NPCRef = stage.NPCRef.name; }
        else if (stage.QuestEvent == "Force_For_Sale") { }
        else if (stage.QuestEvent == "Force_Open_UI") { }
    }
    else if (stage.StageType == "Have_UI_Open") { }
    

    //return stageData; */

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
            //Debug.Log(quest.QuestName);
            convertQuestToJson(quest);
        }
        return "";
    }
}
