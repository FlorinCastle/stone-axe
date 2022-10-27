using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStageJsonData 
{
    public string questStageType;
    /*public string questEvent;

    public string forcedUI;
    public string reqUI;

    public string speaker;
    public string dialogeLine;

    public string itemName;
    public int itemCount;
    public List<string> partMats;

    public int currencyvalue;

    public string NPCRef; */
}
public class DialogeStage : QuestStageJsonData
{
    public string speaker;
    public string dialogeLine;
}
public class CraftItemStage : QuestStageJsonData
{
    public string itemName;
    //public int itemCount;
}
public class SellItemStage : QuestStageJsonData
{

}
public class BuyItemStage : QuestStageJsonData
{

}
public class DisassembleItemStage : QuestStageJsonData
{

}
public class HaveCurrencyStage : QuestStageJsonData
{
    public int currencyvalue;
}
public class ForceEventStage : QuestStageJsonData
{
    public EventStage questEvent;
}
public class HaveUIOpenStage : QuestStageJsonData
{
    public string reqUI;
}


public class EventStage
{
    public string questEvent;
}
public class SummonAdventurerEvent : EventStage
{

}

public class GetItemEvent : EventStage
{
    public string itemName;
    public List<string> partMats;
    public int itemCount;
}
public class RemoveQuestItemsEvent : EventStage
{

}
public class ForceForSaleEvent : EventStage
{
    public string itemName;
    public List<string> partMats;
}
public class GetCurrencyEvent : EventStage
{
    public int currencyvalue;
}
public class RemoveCurrencyEvent : EventStage
{
    public int currencyvalue;
}
public class SummonNPCEvent : EventStage
{
    public string NPCRef;
}
public class DimissQuestNPCEvent : EventStage
{

}
public class ForceOpenUIEvent : EventStage
{
    public string forcedUI;
}
