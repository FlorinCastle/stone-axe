using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Header("Quest Tracking")]
    [SerializeField] List<QuestData> _questDataList;
    [SerializeField] List<QuestData> _tutorialQuests;
    [SerializeField] List<QuestData> _storyQuests;
    [SerializeField] List<QuestData> _onCraft_ItemQuests;
    [SerializeField] List<QuestData> _onCraft_QuestItemQuests;
    [SerializeField] List<QuestData> _onDis_MatQuests;
    [SerializeField] List<QuestData> _onCraft_TotalQuests;

    private void Awake()
    {
        foreach(QuestData quest in _questDataList)
        {
            string questType = quest.QuestType;
            if (questType == "OCC_Item")
                _onCraft_ItemQuests.Add(quest);
            else if (questType == "OCC_QuestItem")
                _onCraft_QuestItemQuests.Add(quest);
            else if (questType == "OD_Material")
                _onDis_MatQuests.Add(quest);
            else if (questType == "OCC_TotalCrafted")
                _onCraft_TotalQuests.Add(quest);
            else if (questType == "Tutorial")
                _tutorialQuests.Add(quest);
            else if (questType == "Story")
                _storyQuests.Add(quest);
        }
    }


    public List<QuestData> getTutorialQuests() { return _tutorialQuests; }
    public List<QuestData> getStoryQuests() { return _storyQuests; }
    public List<QuestData> getOnCraftItemQuests() { return _onCraft_ItemQuests; }
    public List<QuestData> getOnCraftQuestItemQuests() { return _onCraft_QuestItemQuests; }
    public List<QuestData> getOnDisMatQuests() { return _onDis_MatQuests; }
    public List<QuestData> getOnCraftTotalQuests() { return _onCraft_TotalQuests; }
}
