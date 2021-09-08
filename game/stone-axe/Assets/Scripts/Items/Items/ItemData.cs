using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ItemData", order = 53)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private string _itemName;
    [SerializeField]
    private PartData _part1;
    [SerializeField]
    private List<PartData> _validPart1;
    [SerializeField]
    private PartData _part2;
    [SerializeField]
    private List<PartData> _validPart2;
    [SerializeField]
    private PartData _part3;
    [SerializeField]
    private List<PartData> _validPart3;
    [SerializeField]
    private int _baseCost;
    //[SerializeField]
    private int _totalValue;
    [SerializeField]
    private int _baseStrenght;
    [SerializeField]
    private int _baseDextarity;
    [SerializeField]
    private int _baseIntelligence;

    //  TODO: revise this script so differnt, simaler parts can be used to make the same item
    
    public string ItemName
    {
        get => _itemName;
    }

    public int TotalValue
    {
        get => _baseCost + _part1.TotalCurrentValue + _part2.TotalCurrentValue + _part3.TotalCurrentValue;
    }

    public int TotalStrength
    {
        get => _baseStrenght + _part1.PartStrenght + _part2.PartStrenght + _part3.PartStrenght;
    }

    public PartData Part1
    {
        get => _part1;
        set => _part1 = value;
    }

    public PartData Part2
    {
        get => _part2;
        set => _part2 = value;
    }

    public PartData Part3
    {
        get => _part3;
        set => _part3 = value;
    }

    public int TotalDextarity
    {
        get => _baseDextarity + _part1.PartDextarity + _part2.PartDextarity + _part3.PartDextarity;
    }

    public int TotalIntelegence
    {
        get => _baseIntelligence + _part1.PartIntelligence + _part2.PartIntelligence + _part3.PartIntelligence;
    }

    public string RandomMaterials
    {
        get => _part1.RandomMaterial + "\n" + _part2.RandomMaterial + "\n" + _part3.RandomMaterial;
    }
    
    public string Materials
    {
        get => _part1.RandomMaterial + "\n" + _part2.RandomMaterial + "\n" + _part3.RandomMaterial;
    }

    public List<PartData> ValidParts1
    {
        get => _validPart1;
    }

    public List<PartData> ValidParts2
    {
        get => _validPart2;
    }

    public List<PartData> ValidParts3
    {
        get => _validPart3;
    }

}
