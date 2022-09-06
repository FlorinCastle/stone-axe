using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJsonData
{
    public int itemID;
    public string itemName;
    public int levelRequirement;
    public List<string> requiredParts;
    public int baseCost;
    public int baseStrength;
    public int baseDextarity;
    public int baseIntelligence;

    public List<string> filters;
}
