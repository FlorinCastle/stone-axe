using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
