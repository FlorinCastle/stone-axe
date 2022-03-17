using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointMarker : MonoBehaviour
{
    [SerializeField] private GameObject _hitPoint;

    public void ResetHitPoint()
    {
        _hitPoint = null;
    }
    public void clearHitPoint()
    {
        if (this.gameObject.GetComponentInChildren<HitPoint>() != null)
            this.gameObject.GetComponentInChildren<HitPoint>().clearPoint();
    }

    public GameObject HitPoint { get => _hitPoint; set => _hitPoint = value; }
}
