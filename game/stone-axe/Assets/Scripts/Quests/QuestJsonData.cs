using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestJsonData
{
    public int questID;
    public string questName;
    public int requiredPlayerLevel;
    public string questDescription;
    public string questType;
    public List<string> questStages;

    public int currencyReward;
    public int EXPReward;

    public string requiredItem;
    public string requiredMaterial;
    public string requiredQuestItem;
    public int requiredCount;
    public string nextQuest;
    public List<string> unlockedQuests;
}
