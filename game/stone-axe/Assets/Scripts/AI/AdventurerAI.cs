using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAI : MonoBehaviour
{
    // REMINDER: forward is along the z-Axis of the object
    private AdventurerMaster _advMaster;
    [SerializeField] private AdventurerData _advRaceRef;
    private bool _move;

    [SerializeField] private GameObject _currentTarget;
    private Vector3 _targetPosition;
    [Header("Body Refs")]
    [SerializeField] private GameObject _headMark;

    private void Awake()
    {
        _advMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<AdventurerMaster>();
    }

    private Vector3 newDirection;

    private void Update()
    {
        if (_move == true)
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, _targetPosition, 0.01f);
        else if (_move == false)
        {
            Vector3 targetDirection = _targetPosition - this.gameObject.transform.position;
            float singleStep = 2.0f * Time.deltaTime;

            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if (this.gameObject.transform.position.x == _targetPosition.x && this.gameObject.transform.position.z == _targetPosition.z)
        {
            Debug.Log("position reached");
            IsMoving = false;
            if (_currentTarget.GetComponent<WalkingPoint>() == true)
                setCurentTarget(_currentTarget.GetComponent<WalkingPoint>().NextPoint);
            else if (_currentTarget.GetComponent<LinePoint>() == true)
                setCurentTarget(_currentTarget.GetComponent<LinePoint>().NextPoint);
        }

        if (Vector3.Angle(transform.forward, _targetPosition - this.transform.position) < 10f)
            IsMoving = true;
    }

    public void setupAdventurer()
    {
        chooseRace();
        setupAdventurerModel();
    }

    public void setCurentTarget(GameObject target)
    {
        _currentTarget = target;
        _targetPosition = new Vector3(target.transform.position.x, this.gameObject.transform.position.y, target.transform.position.z);
    }

    public bool IsMoving { set => _move = value; }

    private void chooseRace()
    {
        List<AdventurerData> adventRef = _advMaster.GetAdvRaceList;
        int i = Random.Range(0, adventRef.Count);
        _advRaceRef = adventRef[i];
    }

    private void setupAdventurerModel()
    {
        if (_advRaceRef != null)
        {
            Instantiate(_advRaceRef.AdventurerHead, _headMark.transform);
        }
        else
            Debug.LogWarning("Adventurer Data for " + this.gameObject.name + " is not assigned! Use chooseRace() first then use setupAdventurerModel()");
    }
}
