using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAI : MonoBehaviour
{
    // REMINDER: forward is along the z-Axis of the object
    private AdventurerMaster _advMaster;
    [SerializeField] private AdventurerData _advRaceRef;
    private bool _move;
    private bool dismissed;

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
        if (_move == true) // if moving
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, _targetPosition, 0.01f);
        else if (_move == false) // rotate toward next point, but don't move
        {
            Vector3 targetDirection = _targetPosition - this.gameObject.transform.position;
            float singleStep = 2.0f * Time.deltaTime;

            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if (this.gameObject.transform.position.x == _targetPosition.x && this.gameObject.transform.position.z == _targetPosition.z) // if adventurer is at a point
        {
            //Debug.Log("position reached");
            IsMoving = false;
            if (_currentTarget.GetComponent<WalkingPoint>() == true) // if at walking point
            {
                dismissed = false;
                if (_currentTarget.GetComponent<WalkingPoint>().FinalPoint == true)
                { // if at final walking point, yeet thyself! 
                    _advMaster.removeAdventurer(this.gameObject);
                }
                else
                { // otherwise, proceed
                    setCurentTarget(_currentTarget.GetComponent<WalkingPoint>().NextPoint);
                }
            }
            else if (_currentTarget.GetComponent<LinePoint>() == true) // if at line point
            {
                _currentTarget.GetComponent<LinePoint>().IsOccupied = true;
                if (_currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<LinePoint>() != null
                    && _currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<LinePoint>().IsOccupied == false)
                { // if at line point and next point is also a line point that does not have an adventurer
                    _currentTarget.GetComponent<LinePoint>().IsOccupied = false;
                    dismissed = false;
                    setCurentTarget(_currentTarget.GetComponent<LinePoint>().NextPoint);
                }
                else if (_currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<LinePoint>() != null && _currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<LinePoint>().IsOccupied == true)
                { // if at line point and next point is also a line point that has an adventurer waiting
                    dismissed = false;
                    //Debug.Log("waiting");
                }
                else if (dismissed == true && _currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<WalkingPoint>() != null)
                { // if at line point and next point is a walking point and adventurer has been dismissed (head of line, basically)
                    _currentTarget.GetComponent<LinePoint>().IsOccupied = false;
                    GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().AdventurerAtCounter = false;
                    GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().adventurerAtCounter();
                    GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GenerateItem>().adventurerAtCounter();
                    dismissed = false;
                    setCurentTarget(_currentTarget.GetComponent<LinePoint>().NextPoint);
                }
                else if (_currentTarget.GetComponent<LinePoint>().HeadOfLine == true)
                { // if at line point that is head of line
                    GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>().AdventurerAtCounter = true;
                    GameObject.FindGameObjectWithTag("GameMaster").GetComponent<SellItemControl>().adventurerAtCounter();
                    GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GenerateItem>().adventurerAtCounter();
                }
            }
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
    public bool IsDismissed { set => dismissed = value; }

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