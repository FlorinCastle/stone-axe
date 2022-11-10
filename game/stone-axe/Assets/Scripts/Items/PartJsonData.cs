using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartJsonData
{
    public int partID;
    public string partName;
    public int levelRequirement;
    public List<string> validMaterials;
    public List<string> validMaterialTypes;
    public int unitsOfMaterialNeeded;
    public int baseCost;
    public int baseStrength;
    public int baseDextarity;
    public int baseIntelligence;
    public List<string> filters;

    // BETWEEN THESE LINES IS ONLY FOR USE IN CODE! DO NOT PUT DATA INTO THIS FOR SAVING
    public MaterialData materialData;
    // BETWEEN THESE LINES IS ONLY FOR USE IN CODE! DO NOT PUT DATA INTO THIS FOR SAVING
}
