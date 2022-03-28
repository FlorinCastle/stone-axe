using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "ScriptableObjects/QuestData", order = 56)]
public class QuestData : ScriptableObject
{
    public string _questName;
    [TextArea(1, 10)]
    public string _questDiscription;
    public enum questTypeEnum { Not_Set, OCC_Item, OCC_QuestItem, OD_Material, OCC_TotalCrafted, Tutorial, Story };
    public questTypeEnum _questType;
    public List<QuestStage> _questStages;

    public ItemData _requiredItem;
    public MaterialData _requiredMaterial;
    public QuestItemData _requiredQuestItem;
    public int _requiredCount;
    public QuestData _nextQuest;
    public List<QuestData> _unlocksQuests;
    public int _requiredPlayerLevel;

    public bool _storyQuestComplete;

    public int _rewardCurrency;
    public int _rewardEXP;

    public string QuestName { get => _questName; }
    public string QuestDiscription { get => _questDiscription; }
    public string QuestType { get => _questType.ToString(); }
    public List<QuestStage> QuestStages { get => _questStages; }

    public ItemData RequiredItem { get => _requiredItem; }
    public MaterialData ReqiredMaterial { get => _requiredMaterial; }
    public QuestItemData RequiredQuestItem { get => _requiredQuestItem; }
    public int ReqiredCount { get => _requiredCount; }
    public int RequiredPlayerLevel { get => _requiredPlayerLevel; }
    public bool StoryQuestComplete { get => _storyQuestComplete; set => _storyQuestComplete = value; }
    public QuestData NextQuest { get => _nextQuest; }
    public List<QuestData> QuestUnlocks { get => _unlocksQuests; }
    public int RewardedCurrency { get => _rewardCurrency; }
    public int RewardedEXP { get => _rewardEXP; }
}
