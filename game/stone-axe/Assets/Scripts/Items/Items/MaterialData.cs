using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterialData", menuName = "ScriptableObjects/MaterialData", order = 51)]
public class MaterialData : ScriptableObject
{
    public string _materialName;

    public enum matTypeEnum { Metal, Wood, Cloth, Gemstone };
    public enum subMatTypeEnum { Basic, MagicMetal, Cloth, Leather };
    //[SerializeField]
    public matTypeEnum _materialType;
    //[SerializeField]
    public subMatTypeEnum _subMatType;


    //[SerializeField]
    public int _levelRequirement;
    //[SerializeField]
    //public int _quantity;
    //[SerializeField]
    public int _baseCostPerUnit;
    //[SerializeField]
    public int _addedStrength;
    //[SerializeField]
    public int _addedDextarity;
    //[SerializeField]
    public int _addedIntelligence;
    public List<FilterData> _validFilters;

    public int LevelRequirement { get => _levelRequirement; }
    public string Material { get => _materialName; }
    public string MaterialType { get => _materialType.ToString(); }
    public string SubMaterialType { get => _subMatType.ToString(); }
    //public int MaterialCount { get => _quantity; set => _quantity = value; }
    public List<FilterData> ValidFilters { get => _validFilters; }
    public int BaseCostPerUnit { get => _baseCostPerUnit; }
    public int AddedStrength { get => _addedStrength; }
    public int AddedDextarity { get => _addedDextarity; }
    public int AddedIntelligence { get => _addedIntelligence; }
}
