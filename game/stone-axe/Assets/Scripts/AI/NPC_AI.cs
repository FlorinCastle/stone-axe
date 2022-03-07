using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AI : MonoBehaviour
{
    private bool _move;
    private bool dismissed;

    [SerializeField] private GameObject _currentTarget;
    private Vector3 _targetPosition;

    private GameMaster gameMasterRef;

    private void Awake()
    {
        gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private Vector3 newDirection;
    private void FixedUpdate()
    {
        if (_move == true) // if moving
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, _targetPosition, 0.1f);
        else if (_move == false)
        {
            Vector3 targetDirection = _targetPosition - this.gameObject.transform.position;
            float singleStep = 2.0f * Time.deltaTime;

            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if (this.gameObject.transform.position.x == _targetPosition.x && this.gameObject.transform.position.z == _targetPosition.z)
        {
            IsMoving = false;
            // todo: do code for npc movement
            Debug.LogWarning("TODO - set up code for npc movement");
        }

        if (Vector3.Angle(transform.forward, _targetPosition - this.transform.position) < 10f)
            IsMoving = true;
    }


    public bool IsMoving { set => _move = value; }
}
