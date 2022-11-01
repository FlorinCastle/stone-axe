using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStageJsonData
{
    public int questStageIndex;
    public string questStageType;

    public string speaker;
    public string dialogeLine;

    public ItemJson reqItem; // can be null (should usually be null)
}

// for use in Quests ONLY
public class ItemJson
{
    public string itemName;
    public List<PartJson> partData;
    public int reqCount;
}
public class PartJson
{
    public string partName;
    public string partMat;
}


/*public class QuestStageJsonData 
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

    public string NPCRef; 
}
[System.Serializable]
public class DialogeStage : QuestStageJsonData
{
    public string speaker;
    public string dialogeLine;
}
[System.Serializable]
public class CraftItemStage : QuestStageJsonData
{
    public string itemName;
    //public int itemCount;
}
[System.Serializable]
public class SellItemStage : QuestStageJsonData
{

}
[System.Serializable]
public class BuyItemStage : QuestStageJsonData
{

}
[System.Serializable]
public class DisassembleItemStage : QuestStageJsonData
{

}
[System.Serializable]
public class HaveCurrencyStage : QuestStageJsonData
{
    public int currencyvalue;
}
[System.Serializable]
public class ForceEventStage : QuestStageJsonData
{
    public EventStage questEvent;
}
[System.Serializable]
public class HaveUIOpenStage : QuestStageJsonData
{
    public string reqUI;
}


[System.Serializable]
public class EventStage
{
    public string questEvent;
}
[System.Serializable]
public class SummonAdventurerEvent : EventStage
{

}
[System.Serializable]
public class GetItemEvent : EventStage
{
    public string itemName;
    public List<string> partMats;
    public int itemCount;
}
[System.Serializable]
public class RemoveQuestItemsEvent : EventStage
{

}
[System.Serializable]
public class ForceForSaleEvent : EventStage
{
    public string itemName;
    public List<string> partMats;
}
[System.Serializable]
public class GetCurrencyEvent : EventStage
{
    public int currencyvalue;
}
[System.Serializable]
public class RemoveCurrencyEvent : EventStage
{
    public int currencyvalue;
}
[System.Serializable]
public class SummonNPCEvent : EventStage
{
    public string NPCRef;
}
[System.Serializable]
public class DimissQuestNPCEvent : EventStage
{

}
[System.Serializable]
public class ForceOpenUIEvent : EventStage
{
    public string forcedUI;
} */
