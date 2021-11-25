using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePoint : MonoBehaviour
{
    [SerializeField] private GameObject _nextPoint;
    [SerializeField]  private bool isOccupied;
    [SerializeField] private bool headOfLine;

    public GameObject NextPoint { get => _nextPoint; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
    public bool HeadOfLine { get => headOfLine; }
}
