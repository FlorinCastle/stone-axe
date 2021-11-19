using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Header("Quest Tracking")]
    [SerializeField] List<QuestData> _questDataList;
    [SerializeField, HideInInspector] List<QuestData> _tutorialQuests;
    [SerializeField, HideInInspector] List<QuestData> _storyQuests;
    [SerializeField, HideInInspector] List<QuestData> _onCraftItemQuests;
    [SerializeField, HideInInspector] List<QuestData> _onCraftQuestItemQuests;
    [SerializeField, HideInInspector] List<QuestData> _onDisMatQuests;
    [SerializeField, HideInInspector] List<QuestData> _onCraftTotalQuests;
    [SerializeField, HideInInspector] List<QuestData> _repeatableQuests;

    private void Awake()
    {
        foreach(QuestData quest in _questDataList)
        {
            string questType = quest.QuestType;
            if (questType == "OCC_Item")
                _onCraftItemQuests.Add(quest);
            if (questType == "OCC_QuestItem")
                _onCraftQuestItemQuests.Add(quest);
            if (questType == "OD_Material")
                _onDisMatQuests.Add(quest);
            if (questType == "OCC_TotalCrafted")
                _onCraftTotalQuests.Add(quest);
            if (questType == "Tutorial")
                _tutorialQuests.Add(quest);
            if (questType == "Story")
                _storyQuests.Add(quest);
            if (questType == "OCC_Item" || questType == "OCC_QuestItem" || questType == "OD_Material" || questType == "OCC_TotalCrafted")
                _repeatableQuests.Add(quest);
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
