using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDataStorage : MonoBehaviour
{
    [Header("Part Data")]
    [SerializeField] private string _partName;
    [SerializeField] private MaterialData _material;
    [SerializeField] private int _totalValue;

    [Header("Part Stats")]
    [SerializeField] private int _partStrength;
    [SerializeField] private int _partDextarity;
    [SerializeField] private int _partIntelegence;

    public string PartName { get => _partName; }
    public void setPartName(string name) { _partName = name; }

    public MaterialData Material { get => _material; }
    public void setMaterial(MaterialData mat) { _material = mat; }

    public string MaterialName { get => _material.Material; }

    public int Value { get => _totalValue; }
    public void setValue(int value) { _totalValue = value; }

    public int PartStr  { get => _partStrength; }
    public void setPartStr(int value) { _partStrength = value; }

    public int PartDex { get => _partDextarity; }
    public void setPartDex(int value) { _partDextarity = value; }

    public int PartInt { get => _partIntelegence; }
    public void setPartInt(int value) { _partIntelegence = value; }
}
