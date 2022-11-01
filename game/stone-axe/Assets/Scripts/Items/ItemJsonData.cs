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

    /*// BETWEEN THESE LINES IS ONLY FOR USE IN CODE! DO NOT PUT DATA INTO THIS FOR SAVING
    public List<PartJsonData> parts;
    public bool isEnchanted;
    public EnchantData ench;
    // BETWEEN THESE LINES IS ONLY FOR USE IN CODE! DO NOT PUT DATA INTO THIS FOR SAVING */
}
public class ItemJsonDataCode : ItemJsonData
{
    public List<PartJsonData> parts;
    public bool isEnchanted;
    public EnchantData ench;
}