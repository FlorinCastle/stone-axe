using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePoint : MonoBehaviour
{
    [SerializeField] private GameObject _nextPoint;

    public GameObject NextPoint { get => _nextPoint; }
}
