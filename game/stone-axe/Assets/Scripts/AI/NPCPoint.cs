using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPoint : MonoBehaviour
{
    [SerializeField] private GameObject _nextPoint;
    [SerializeField] private bool isOccupied;

    public GameObject NextPoint { get => _nextPoint; }
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
}
