using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestStage", menuName = "ScriptableObjects/QuestStage", order = 57)]
public class QuestStage : ScriptableObject
{
    public enum questStageEnum { Not_Set, Dialogue, Craft_Item, Sell_Item, Buy_Item, Disassemble_Item, Have_Currency, Force_Event };
    public questStageEnum _questStageType;

    public enum questEvent { Not_Set, Summon_Adventurer, Get_Item, Get_Currency, Remove_Currency, Summon_NPC, Dismiss_Quest_NPC, Force_For_Sale };
    public questEvent _questEvent;

    public string _speaker;
    [TextArea(1, 10)]
    public string _dialogueLine;
    public ItemData _item;
    public MaterialData _part1Mat;
    public MaterialData _part2Mat;
    public MaterialData _part3Mat;
    public int _currencyValue;
    public GameObject _npcRef;


    public string StageType { get => _questStageType.ToString(); }
    public string QuestEvent { get => _questEvent.ToString(); }
    public string DialogueSpeaker { get => _speaker; }
    public string DialogueLine { get => _dialogueLine; }
    public ItemData ItemToGet { get => _item; }
    public MaterialData Part1Mat { get => _part1Mat; }
    public MaterialData Part2Mat { get => _part2Mat; }
    public MaterialData Part3Mat { get => _part3Mat; }
    public int CurrencyValue { get => _currencyValue; }
    public GameObject NPCRef { get => _npcRef; }
}
