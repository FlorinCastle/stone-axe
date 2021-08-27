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
    private PartData _part2;
    [SerializeField]
    private PartData _part3;
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

    public string ItemName
    {
        get
        {
            return _itemName;
        }
    }

    public int TotalValue
    {
        get
        {
            return _baseCost + _part1.TotalCurrentValue + _part2.TotalCurrentValue + _part3.TotalCurrentValue;
        }
    }

    public int TotalStrength
    {
        get
        {
            return _baseStrenght + _part1.PartStrenght + _part2.PartStrenght + _part3.PartStrenght;
        }
    }

    public int TotalDextarity
    {
        get
        {
            return _baseDextarity + _part1.PartDextarity + _part2.PartDextarity + _part3.PartDextarity;
        }
    }

    public int TotalIntelegence
    {
        get
        {
            return _baseIntelligence + _part1.PartIntelligence + _part2.PartIntelligence + _part3.PartIntelligence;
        }
    }

    public string Materials
    {
        get
        {
            return _part1.Material + "\n" + _part2.Material + "\n" + _part3.Material;
        }
    }
}
