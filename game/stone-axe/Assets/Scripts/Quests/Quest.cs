using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Header("Quest Tracking")]
    [SerializeField] private List<QuestData> _questDataList;
    //private List<QuestData> _tutorialQuests;
    //private List<QuestData> _storyQuests;
    //[SerializeField] private List<QuestData> _onCraftItemQuests;
    //[SerializeField] private List<QuestData> _onCraftQuestItemQuests;
    //[SerializeField] private List<QuestData> _onDisMatQuests;
    //[SerializeField] private List<QuestData> _onCraftTotalQuests;
    //[SerializeField] private List<QuestData> _repeatableQuests;
    [Header("Quest Jsons")]
    [SerializeField] private List<TextAsset> _questJsons;
    [SerializeField] private List<TextAsset> _tutorialQuestJsons;
    [SerializeField] private List<TextAsset> _storyQuestJsons;
    private List<StoryQuest> storyQuests;
    [SerializeField] private List<TextAsset> _craftItemQuestJsons;
    private List<CraftItemQuest> craftItemQuests;
    [SerializeField] private List<TextAsset> _craftQuestItemQuestJsons;
    private List<CraftQuestItemQuest> craftQuestItemQuests;
    [SerializeField] private List<TextAsset> _haveMatQuestJsons;
    private List<HaveMaterialQuest> haveMatQuests;
    [SerializeField] private List<TextAsset> _craftItemTotalQuestJsons;
    private List<CraftManyItemQuest> craftManyQuest;
    [SerializeField] private List<TextAsset> _repeatableQuestJsons;

    private List<TutorialQuest> _tutorialQuestData;
    private List<StoryQuest> _storyQuestData;

    [SerializeField, HideInInspector] private List<TextAsset> processedJsons;

    private bool questsOrganized = false;

    private void Awake()
    {
        storyQuests = new List<StoryQuest>();
        craftItemQuests = new List<CraftItemQuest>();
        craftQuestItemQuests = new List<CraftQuestItemQuest>();
        haveMatQuests = new List<HaveMaterialQuest>();
        craftManyQuest = new List<CraftManyItemQuest>();

        organizeQuests();
        processQuests();
    }
    private void Start()
    {
        organizeQuests();
    }

    public void organizeQuests()
    {
        if (questsOrganized == false)
        {
            Debug.Log("Quest.Awake() organizing quests");
            foreach (TextAsset quest in _questJsons)
            {
                BaseQuestJsonData questJson = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                //Debug.Log(JsonUtility.ToJson(questJson, true));
                if (questJson.questType == "OCC_Item" && !_craftItemQuestJsons.Contains(quest))        // add to craft 1 item list
                    _craftItemQuestJsons.Add(quest);
                if (questJson.questType == "OCC_QuestItem" && !_craftQuestItemQuestJsons.Contains(quest))   // add to craft quest item list
                    _craftQuestItemQuestJsons.Add(quest);
                if (questJson.questType == "OD_Material")       // add to have material quest list
                    _haveMatQuestJsons.Add(quest);
                if (questJson.questType == "OCC_TotalCrafted")  // add to craft many item list
                    _craftItemTotalQuestJsons.Add(quest);
                if (questJson.questType == "Tutorial")          // add to tutorial quest list
                    _tutorialQuestJsons.Add(quest);
                if (questJson.questType == "Story")             // add to story quest list
                    _storyQuestJsons.Add(quest);
                if (questJson.questType == "OCC_Item" || questJson.questType == "OCC_QuestItem" || questJson.questType == "OD_Material" || questJson.questType == "OCC_TotalCrafted") // add to repeatable quests list
                    _repeatableQuestJsons.Add(quest);           //Debug.Log("repeatable " + quest.name);      
            }
        }            
        questsOrganized = true;
    }
    public void processQuests()
    {
        Debug.Log("Quest.processQuests() processing quests");
        foreach(TextAsset quest in _questJsons)
        {
            if (processedJsons.Contains(quest) == false)
            {
                BaseQuestJsonData questJson = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                if (questJson.questType == "OCC_Item")
                {
                    CraftItemQuest craftQuest = JsonUtility.FromJson<CraftItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                    if (!craftItemQuests.Contains(craftQuest))
                        craftItemQuests.Add(craftQuest);
                }
                if (questJson.questType == "OCC_QuestItem")
                {
                    CraftQuestItemQuest craftQuest = JsonUtility.FromJson<CraftQuestItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                    if (!craftQuestItemQuests.Contains(craftQuest))
                        craftQuestItemQuests.Add(craftQuest);
                }
                if (questJson.questType == "OD_Material")
                {
                    HaveMaterialQuest craftQuest = JsonUtility.FromJson<HaveMaterialQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                    if (!haveMatQuests.Contains(craftQuest))
                        haveMatQuests.Add(craftQuest);
                }
                if (questJson.questType == "OCC_TotalCrafted")
                {
                    CraftManyItemQuest craftQuest = JsonUtility.FromJson<CraftManyItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                    if (!craftManyQuest.Contains(craftQuest))
                        craftManyQuest.Add(craftQuest);
                }
                if (questJson.questType == "Tutorial" || questJson.questType == "Story")
                {
                    StoryQuest storyQuest = JsonUtility.FromJson<StoryQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
                    if (!storyQuests.Contains(storyQuest))
                        storyQuests.Add(storyQuest);
                }
                //Debug.Log("Quest.processQuests(): Quest [" + questJson.questName + "] has been processed");
                processedJsons.Add(quest);
            }
        }

    }

    // TODO
    public QuestObject saveQuest(QuestData currentQuest)
    {
        QuestObject questObject = new QuestObject
        {
            questName = "",
            questType = "Not_Set",
        };
        if (currentQuest != null)
        {
            questObject.questName = currentQuest.QuestName;
            questObject.questType = currentQuest.QuestType;
        }
        return questObject;
    }
    public QuestObject saveQuest(TextAsset currentQuest)
    {
        QuestObject questObject = new QuestObject
        {
            questName = "",
            questType = "Not_Set",
        };
        if (currentQuest != null)
        {
            questObject.questName = QuestName(currentQuest);
            questObject.questType = QuestType(currentQuest);            
        }
        return questObject;
    }
    public QuestObject saveQuest(StoryQuest currentQuest)
    {
        QuestObject questObject = new QuestObject
        {
            questName = "",
            questType = "Not_Set",
            isComplete = false,
        };
        if (currentQuest != null)
        {
            questObject.questName = currentQuest.questName;
            questObject.questType = currentQuest.questType;
            questObject.isComplete = currentQuest.isComplete;
        }
        return questObject;

    }
    
    public string QuestName(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.questName;
    }
    public string QuestDescription(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.questDescription;
    }
    public int QuestLevel(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.requiredPlayerLevel;
    }
    public string QuestType(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.questType;
    }
    public string RequiredMat(TextAsset questAsset)
    {
        HaveMaterialQuest quest = JsonUtility.FromJson<HaveMaterialQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.requiredMaterial;
    }
    public string RequiredItem(TextAsset questAsset)
    {
        CraftItemQuest quest = JsonUtility.FromJson<CraftItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.requiredItem;
    }
    public string RequiredQuestItem(TextAsset questAsset)
    {
        CraftQuestItemQuest quest = JsonUtility.FromJson<CraftQuestItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.requiredQuestItem;
    }
    public int RewardedCurrency(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.currencyReward;
    }
    public int RewardedExperience(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest.EXPReward;
    }

    public BaseQuestJsonData LoadQuestData(TextAsset questAsset)
    {
        BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest;
    }
    public CraftItemQuest LoadCraftItemQuest(TextAsset questAsset)
    {
        CraftItemQuest quest = JsonUtility.FromJson<CraftItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest;
    }
    public CraftQuestItemQuest LoadCraftQuestItemQuest(TextAsset questAsset)
    {
        CraftQuestItemQuest quest = JsonUtility.FromJson<CraftQuestItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest;
    }
    public CraftManyItemQuest LoadCraftManyItemQuest(TextAsset questAsset)
    {
        CraftManyItemQuest quest = JsonUtility.FromJson<CraftManyItemQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest;
    }
    public HaveMaterialQuest LoadHaveMaterialQuest(TextAsset questAsset)
    {
        HaveMaterialQuest quest = JsonUtility.FromJson<HaveMaterialQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest;

    }
    public StoryQuest LoadStoryQuest(TextAsset questAsset)
    {
        StoryQuest quest = JsonUtility.FromJson<StoryQuest>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(questAsset).Replace("Assets", "")));
        return quest;
    }

    public TextAsset FetchQuestTextAssestByName(string input)
    {
        foreach(TextAsset quest in _questJsons)
            if (QuestName(quest) == input)
                return quest;
        return null;
    }

    public List<TextAsset> AllQuests { get => _questJsons; }
    public List<TextAsset> TutorialQuests { get => _tutorialQuestJsons; }
    public List<TextAsset> StoryQuests { get => _storyQuestJsons; }
    public List<TextAsset> CraftItemQuests { get => _craftItemQuestJsons; }
    public List<TextAsset> CraftQuestItemQuests { get => _craftQuestItemQuestJsons; }
    public List<TextAsset> HaveMaterialQuests { get => _haveMatQuestJsons; }
    public List<TextAsset> CraftManyItemQuests { get => _craftItemTotalQuestJsons; }
    public List<TextAsset> RepeatableQuests { get => _repeatableQuestJsons; }

    //public List<QuestData> getAllQuests() { return _questDataList; }
    //public List<QuestData> getTutorialQuests() { return _tutorialQuests; }
    //public List<QuestData> getStoryQuests() { return _storyQuests; }
    //public List<QuestData> getOnCraftItemQuests() { return _onCraftItemQuests; }
    //public List<QuestData> getOnCraftQuestItemQuests() { return _onCraftQuestItemQuests; }
    //public List<QuestData> getOnDisMatQuests() { return _onDisMatQuests; }
    //public List<QuestData> getOnCraftTotalQuests() { return _onCraftTotalQuests; }
    //public List<QuestData> getRepeatableQuests() { return _repeatableQuests; }
}
[System.Serializable]
public class QuestObject
{
    public string questName;
    public string questType;
    public bool isComplete;
}
