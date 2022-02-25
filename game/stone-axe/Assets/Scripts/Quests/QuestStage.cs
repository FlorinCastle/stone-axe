using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestStage", menuName = "ScriptableObjects/QuestStage", order = 57)]
public class QuestStage : ScriptableObject
{
    public enum questStageEnum { Not_Set, Dialogue, Craft_Item, Sell_Item, Buy_Item, Disassemble_Item, Force_Event };
    public questStageEnum _questStageType;

    public enum questEvent { Not_Set, Summon_Adventurer, Get_Item, Get_Currency };
    public questEvent _questEvent;

    public string _speaker;
    public string _dialogueLine;
    public ItemData _itemToGet;
    public int _currencyValue;


    public string StageType { get => _questStageType.ToString(); }
    public string QuestEvent { get => _questEvent.ToString(); }
    public string DialogueSpeaker { get => _speaker; }
    public string DialogueLine { get => _dialogueLine; }
}
