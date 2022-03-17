using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    [SerializeField] private MiniGameControl _mGControlRef;
    private void Awake()
    {
        if (_mGControlRef == null)
            _mGControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<MiniGameControl>();
    }

    public void pointHit()
    {
        _mGControlRef.increaseHitCount();
        this.gameObject.GetComponentInParent<HitPointMarker>().ResetHitPoint();
        Destroy(this.gameObject);
    }

    public void clearPoint()
    {
        this.gameObject.GetComponentInParent<HitPointMarker>().ResetHitPoint();
        Destroy(this.gameObject);
    }
}
