using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPoint : MonoBehaviour
{
    [SerializeField] private GameObject _nextPoint;

    public GameObject NextPoint { get => _nextPoint; }
}
