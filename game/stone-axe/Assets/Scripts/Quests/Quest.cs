using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Header("Quest Tracking")]
    [SerializeField] private List<QuestData> _questDataList;
    [SerializeField] private List<QuestData> _tutorialQuests;
    [SerializeField] private List<QuestData> _storyQuests;
    [SerializeField] private List<QuestData> _onCraftItemQuests;
    [SerializeField] private List<QuestData> _onCraftQuestItemQuests;
    [SerializeField] private List<QuestData> _onDisMatQuests;
    [SerializeField] private List<QuestData> _onCraftTotalQuests;
    [SerializeField] private List<QuestData> _repeatableQuests;
    [Header("Quest Jsons")]
    [SerializeField] private List<TextAsset> _questJsons;
    [SerializeField] private List<TextAsset> _tutorialQuestJsons;
    [SerializeField] private List<TextAsset> _storyQuestJsons;
    [SerializeField] private List<TextAsset> _craftItemQuestJsons;
    [SerializeField] private List<TextAsset> _craftQuestItemQuestJsons;
    [SerializeField] private List<TextAsset> _haveMatQuestJsons;
    [SerializeField] private List<TextAsset> _craftItemTotalQuestJsons;
    [SerializeField] private List<TextAsset> _repeatableQuestJsons;

    private void Awake()
    {
        //organizeQuests();
    }
    private void Start()
    {
        organizeQuests();
    }

    public void organizeQuests()
    {
        Debug.Log("Quest.Awake() organizing quests");
        foreach (QuestData quest in _questDataList)
        {
            string questType = quest.QuestType;
            if (questType == "OCC_Item" && !_onCraftItemQuests.Contains(quest))
                _onCraftItemQuests.Add(quest);
            if (questType == "OCC_QuestItem" && !_onCraftQuestItemQuests.Contains(quest))
                _onCraftQuestItemQuests.Add(quest);
            if (questType == "OD_Material" && !_onDisMatQuests.Contains(quest))
                _onDisMatQuests.Add(quest);
            if (questType == "OCC_TotalCrafted" && !_onCraftTotalQuests.Contains(quest))
                _onCraftTotalQuests.Add(quest);
            if (questType == "Tutorial" && !_tutorialQuests.Contains(quest))
                _tutorialQuests.Add(quest);
            if (questType == "Story" && !_storyQuests.Contains(quest))
                _storyQuests.Add(quest);
            if ((questType == "OCC_Item" || questType == "OCC_QuestItem" || questType == "OD_Material" || questType == "OCC_TotalCrafted") && !_repeatableQuests.Contains(quest))
                _repeatableQuests.Add(quest);
        }
        foreach (TextAsset quest in _questJsons)
        {
            BaseQuestJsonData questJson = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(quest).Replace("Assets", "")));
            //Debug.Log(JsonUtility.ToJson(questJson, true));
            if (questJson.questType == "OCC_Item") // add to craft 1 item list
                _craftItemQuestJsons.Add(quest);//Debug.Log("OCC_Item " + quest.name); 
            if (questJson.questType == "OCC_QuestItem") // add to craft quest item list
                Debug.Log("OCC_QuestItem " + quest.name);
            if (questJson.questType == "OD_Material") // add to have material quest list
                Debug.Log("OD_Material " + quest.name);
            if (questJson.questType == "OCC_TotalCrafted") // add to craft many item list
                Debug.Log("OCC_TotalCrafted " + quest.name);
            if (questJson.questType == "Tutorial") // add to tutorial quest list
                Debug.Log("Tutorial " + quest.name);
            if (questJson.questType == "Story") // add to story quest list
                Debug.Log("Story " + quest.name);
            if (questJson.questType == "OCC_Item" || questJson.questType == "OCC_QuestItem" || questJson.questType == "OD_Material" || questJson.questType == "OCC_TotalCrafted") // add to repeatable quests list
                Debug.Log("repeatable " + quest.name);
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
    
    public List<TextAsset> AllQuests { get => _questJsons; }
    public List<TextAsset> TutorialQuests { get => _questJsons; }
    public List<TextAsset> StoryQuests { get => _questJsons; }
    public List<TextAsset> CraftItemQuests { get => _questJsons; }
    public List<TextAsset> CraftQuestItemQuests { get => _questJsons; }
    public List<TextAsset> HaveMaterialQuests { get => _questJsons; }
    public List<TextAsset> CraftManyItemQuests { get => _questJsons; }
    public List<TextAsset> RepeatableQuests { get => _questJsons; }

    public List<QuestData> getAllQuests() { return _questDataList; }
    public List<QuestData> getTutorialQuests() { return _tutorialQuests; }
    public List<QuestData> getStoryQuests() { return _storyQuests; }
    public List<QuestData> getOnCraftItemQuests() { return _onCraftItemQuests; }
    public List<QuestData> getOnCraftQuestItemQuests() { return _onCraftQuestItemQuests; }
    public List<QuestData> getOnDisMatQuests() { return _onDisMatQuests; }
    public List<QuestData> getOnCraftTotalQuests() { return _onCraftTotalQuests; }
    public List<QuestData> getRepeatableQuests() { return _repeatableQuests; }
}
[System.Serializable]
public class QuestObject
{
    public string questName;
    public string questType;
}
