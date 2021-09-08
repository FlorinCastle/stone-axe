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

    
}
