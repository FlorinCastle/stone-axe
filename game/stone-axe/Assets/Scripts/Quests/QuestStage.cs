using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestStage", menuName = "QuestStage", order = 57)]
public class QuestStage : ScriptableObject
{
    public enum questStageEnum { Not_Set, Dialogue, Craft_Item, Sell_Item, Buy_Item };
    public questStageEnum _questStageType;

    public string _speaker;
    public string _dialogueLine;


    public string StageType { get => _questStageType.ToString(); }
    public string DialogueSpeaker { get => _speaker; }
    public string DialogueLine { get => _dialogueLine; }
}
