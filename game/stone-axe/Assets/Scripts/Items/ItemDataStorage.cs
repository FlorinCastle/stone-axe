using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataStorage : MonoBehaviour
{
    [Header("Item Data")]
    [SerializeField] private string _itemName;
    [SerializeField] private int _totalValue;

    [Header("Item Stats")]
    [SerializeField] private int _totalStrength;
    [SerializeField] private int _totalDextarity;
    [SerializeField] private int _totalIntelegence;

    [Header("Parts")]
    [SerializeField] private PartDataStorage _part1;
    [SerializeField] private PartDataStorage _part2;
    [SerializeField] private PartDataStorage _part3;


    public void setItemName(string name) { _itemName = name; }
    public string ItemName { get => _itemName; }

    public void setTotalValue(int value) { _totalValue = value; }
    public int TotalValue { get => _totalValue; }

    public void setTotalStrenght(int value) { _totalStrength = value; }
    public int TotalStrength{ get => _totalStrength; }

    public void setTotalDex(int value) { _totalDextarity = value; }
    public int TotalDextarity { get => _totalDextarity; }

    public void setTotalInt(int value) { _totalIntelegence = value; }
    public int TotalIntelegence { get => _totalIntelegence; }

    public void setPart1(PartDataStorage value) { _part1 = value; }
    public PartDataStorage Part1 { get => _part1; }

    public void setPart2(PartDataStorage value) { _part2 = value; }
    public PartDataStorage Part2 { get => _part2; }

    public void setPart3(PartDataStorage value) { _part3 = value; }
    public PartDataStorage Part3 { get => _part3; }

}
