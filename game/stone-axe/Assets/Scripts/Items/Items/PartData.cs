using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPartData", menuName = "PartData", order = 52)]
public class PartData : ScriptableObject
{
    [SerializeField]
    private string _partName;
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

    private int ranIndex;

    public string PartName
    {
        get
        {
            return _partName;
        }
    }
    
    public MaterialData Material
    {
        get => _material;
        set => _material = value;
    }

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

    public List<MaterialData> ValidMaterialData
    {
        get
        {
            return _validMaterials;
        }
    }

    public List<string> ValidMaterials
    {
        get
        {
            return _validMaterialTypes;
        }
    }

    public int TotalCurrentValue
    {
        get
        {
            _totalCurrentValue = _baseCost + (_unitsOfMaterialNeeded * _material.BaseCostPerUnit);
            return _totalCurrentValue;
        }
    }

    public int PartStrenght
    {
        get
        {
            
            return _baseStrenght + _material.AddedStrength;
        }
    }

    public int PartDextarity
    {
        get
        {
            return _baseDextarity + _material.AddedDextarity;
        }
    }

    public int PartIntelligence
    {
        get
        {
            return _baseIntelligence + _material.AddedIntelligence;
        }
    }
}
