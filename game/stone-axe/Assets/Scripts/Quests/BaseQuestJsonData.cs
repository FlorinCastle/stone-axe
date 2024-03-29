using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseQuestJsonData
{
    public string questID;
    public string questName;
    public QuestPrerequisits questPrereqs;
    //public int requiredPlayerLevel;
    public string questDescription;
    public string questType;

    public int currencyReward;
    public int EXPReward;

    /*public List<string> questStages;
    public List<QuestStageJsonData> questStagesJson;

    public string requiredItem;
    public string requiredMaterial;
    public string requiredQuestItem;
    public int requiredCount;
    public string nextQuest;
    public List<string> unlockedQuests; */
}
public class CraftItemQuest: BaseQuestJsonData
{
    public string requiredItem;
}
public class CraftQuestItemQuest: BaseQuestJsonData
{
    public string requiredQuestItem;
}
public class HaveMaterialQuest: BaseQuestJsonData
{
    public string requiredMaterial;
    public int requiredCount;
}
public class CraftManyItemQuest : BaseQuestJsonData
{
    public string requiredItem;
    public int requiredCount;
}
public class StoryQuest : BaseQuestJsonData
{
    //public List<string> questStages;
    public List<QuestStageJsonData> questStagesJson;
    public string nextQuest;
    public List<string> unlockedQuests;

    public List<string> unlockFeatures;

    public bool isComplete;
}
public class TutorialQuest : BaseQuestJsonData 
{
    //public List<string> questStages;
    public List<QuestStageJsonData> questStagesJson;
    public string nextQuest;
    public List<string> unlockedQuests;

    public List<string> unlockFeatures;

    public bool isComplete;
}

[System.Serializable]
public class QuestPrerequisits
{
    public int requiredPlayerLevel;
    List<string> requiredCompletedQuests;
}