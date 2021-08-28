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
    private List<MaterialData> _validmaterials;
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

    public string Material
    {
        get
        {
            ranIndex = Random.Range(0, _validmaterials.Count);
            _material = _validmaterials[ranIndex];
            //Debug.Log(_partName + " " + _material.Material);
            return _material.Material;
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
