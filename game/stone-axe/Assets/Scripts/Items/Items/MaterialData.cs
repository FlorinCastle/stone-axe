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
    private int _quantity;
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
        get => _materaialName;
    }

    public string MaterialType
    {
        get => _materialType.ToString();
    }

    public int MaterialCount
    {
        get => _quantity;
    }

    public int BaseCostPerUnit
    {
        get => _baseCostPerUnit;
    }

    public int AddedStrength
    {
        get => _addedStrength;
    }

    public int AddedDextarity
    {
        get => _addedDextarity;
    }

    public int AddedIntelligence
    {
        get => _addedIntelligence;
    }
}
