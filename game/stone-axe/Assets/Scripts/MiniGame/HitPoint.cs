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
        Destroy(this.gameObject);
    }
}
