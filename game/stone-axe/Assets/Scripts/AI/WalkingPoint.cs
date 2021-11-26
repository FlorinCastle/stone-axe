using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPoint : MonoBehaviour
{
    [SerializeField] private GameObject _nextPoint;
    [SerializeField] private bool finalPoint;

    public GameObject NextPoint { get => _nextPoint; }
    public bool FinalPoint { get => finalPoint; }
}
