using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPartData", menuName = "ScriptableObjects/PartData", order = 52)]
public class PartData : ScriptableObject
{
    [SerializeField]
    private string _partName;
    [SerializeField]
    private int _partLevelReq;
    [SerializeField]
    private MaterialData _material;
    [SerializeField]
    private List<MaterialData> _validMaterials;
    [SerializeField]
    private List<string> _validMaterialTypes;
    [SerializeField]
    private int _unitsOfMaterialNeeded;
    [SerializeField]
    private int _baseCost;
    //[SerializeField]
    private int _totalCurrentValue;
    [Header("Stats")]
    [SerializeField]
    private int _baseStrenght;
    [SerializeField]
    private int _baseDextarity;
    [SerializeField]
    private int _baseIntelligence;

    [SerializeField]
    private List<FilterData> _filters;

    private int ranIndex;

    public string PartName { get => _partName; }
    public int PartLevelReq { get => _partLevelReq; }
    public MaterialData Material { get => _material; set => _material = value; }

    public string RandomMaterial
    {
        get
        {
            ranIndex = Random.Range(0, _validMaterials.Count);
            _material = _validMaterials[ranIndex];
            //Debug.Log(_partName + " " + _material.Material);
            return _material.Material;
        }
    }

    public List<MaterialData> ValidMaterialData { get => _validMaterials; }

    // bug here
    public List<string> ValidMaterials { get => _validMaterialTypes; }

    public int UnitsOfMaterialNeeded { get => _unitsOfMaterialNeeded; }

    public int TotalCurrentValue
    {
        get
        {
            _totalCurrentValue = _baseCost + (_unitsOfMaterialNeeded * _material.BaseCostPerUnit);
            return _totalCurrentValue;
        }
    }

    public int BaseCost { get { return _baseCost; } }

    public int PartStrenght { get { return _baseStrenght + _material.AddedStrength; } }
    public int BaseStrenght { get { return _baseStrenght; } }
    public int PartDextarity { get { return _baseDextarity + _material.AddedDextarity; } }
    public int BaseDextarity { get { return _baseDextarity; } }
    public int PartIntelligence { get { return _baseIntelligence + _material.AddedIntelligence; } } 
    public int BaseIntelligence { get { return _baseIntelligence; } }

    public List<FilterData> ValidFilters { get => _filters; }
}
