using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAI : MonoBehaviour
{
    // REMINDER: forward is along the z-Axis of the object

    [SerializeField] private GameObject _currentPoint;

    /*
    Rigidbody m_Rigidbody;
    float m_Speed;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Speed = 10.0f;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            m_Rigidbody.velocity = transform.forward * m_Speed;
        }
    }
    */
}
