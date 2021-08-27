using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterialData", menuName = "MaterialData", order = 51)]
public class MaterialData : ScriptableObject
{
    private enum matTypeEnum
    {
        Metal,
        Wood,
        Cloth
    };
    [SerializeField]
    private string _materaialName;
    [SerializeField]
    private matTypeEnum _materialType;
    [SerializeField]
    private int _levelRequirement;
    [SerializeField]
    private int _baseCostPerUnit;
    [Header("Extra Stats")]
    [SerializeField]
    private int _addedStrength;
    [SerializeField]
    private int _addedDextarity;
    [SerializeField]
    private int _addedIntelligence;


    public string Material
    {
        get
        {
            return _materaialName;
        }
    }
    public int BaseCostPerUnit
    {
        get
        {
            return _baseCostPerUnit;
        }
    }

    public int AddedStrength
    {
        get
        {
            return _addedStrength;
        }
    }

    public int AddedDextarity
    {
        get
        {
            return _addedDextarity;
        }
    }

    public int AddedIntelligence
    {
        get
        {
            return _addedIntelligence;
        }
    }
}
