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

    public string PartName
    {
        get => _partName;
        set => _partName = value;
    }

    public MaterialData Material
    {
        get => _material;
        set => _material = value;
    }

    public string MaterialName
    {
        get => _material.Material;
    }

    public int Value
    {
        get => _totalValue;
    }

    public int PartStr
    {
        get => _partStrength;
        set => _partStrength = value;
    }

    public int PartDex
    {
        get => _partDextarity;
        set => _partDextarity = value;
    }

    public int PartInt
    {
        get => _partIntelegence;
        set => _partIntelegence = value;
    }
}
