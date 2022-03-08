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
            if (_currentTarget.GetComponent<WalkingPoint>() == true)
            {
                dismissed = false;
                setCurentTarget(_currentTarget.GetComponent<WalkingPoint>().NextPoint);
            }
            else if (_currentTarget.GetComponent<NPCPoint>() == true)
            {
                _currentTarget.GetComponent<NPCPoint>().IsOccupied = true;
                if (_currentTarget.GetComponent<NPCPoint>().NextPoint.GetComponent<NPCPoint>() != null
                    && _currentTarget.GetComponent<NPCPoint>().NextPoint.GetComponent<NPCPoint>().IsOccupied == false)
                {
                    _currentTarget.GetComponent<NPCPoint>().IsOccupied = false;
                    dismissed = false;
                    setCurentTarget(_currentTarget.GetComponent<NPCPoint>().NextPoint);
                }
                else if (_currentTarget.GetComponent<NPCPoint>().NextPoint.GetComponent<NPCPoint>() != null
                    && _currentTarget.GetComponent<NPCPoint>().NextPoint.GetComponent<NPCPoint>().IsOccupied == true)
                {
                    dismissed = false;
                }
                else if (dismissed == true && _currentTarget.GetComponent<NPCPoint>().NextPoint.GetComponent<WalkingPoint>() != null)
                {
                    _currentTarget.GetComponent<NPCPoint>().IsOccupied = false;
                    dismissed = false;
                    setCurentTarget(_currentTarget.GetComponent<NPCPoint>().NextPoint);
                }
            }
        }

        if (Vector3.Angle(transform.forward, _targetPosition - this.transform.position) < 10f)
            IsMoving = true;
    }
    public void setupNPC(GameObject NPCRef)
    {

    }

    public void setCurentTarget(GameObject target)
    {
        _currentTarget = target;
        _targetPosition = new Vector3(target.transform.position.x, this.gameObject.transform.position.y, target.transform.position.z);
    }


    public bool IsMoving { set => _move = value; }
    public bool IsDismissed { set => dismissed = value; }
}
