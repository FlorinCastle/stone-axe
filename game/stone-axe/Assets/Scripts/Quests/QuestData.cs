using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "QuestData", order = 56)]
public class QuestData : ScriptableObject
{
    public string _questName;
    [Multiline(2)]
    public string _questDiscription;
    public enum questTypeEnum { Not_Set, OCC_Item, OCC_QuestItem, OD_Material, OCC_TotalCrafted, Tutorial, Story };
    public questTypeEnum _questType;

    public ItemData _requiredItem;
    public MaterialData _requiredMaterial;
    public int _requiredCount;
    public QuestData _nextQuest;

    public string QuestName { get => QuestName; }
    public string QuestDiscription { get => _questDiscription; }
    public string QuestType { get => _questType.ToString(); }

    public ItemData RequiredItem { get => _requiredItem; }
    public MaterialData ReqiredMaterial { get => _requiredMaterial; }
    public int ReqiredCount { get => _requiredCount; }
}
