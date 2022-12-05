using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AI : MonoBehaviour
{
    [SerializeField] private string npcName;
    private bool _move;
    [SerializeField] private bool dismissed;

    [SerializeField] private GameObject _currentTarget;
    private Vector3 _targetPosition;

    [SerializeField, HideInInspector] private GameMaster gameMasterRef;
    [SerializeField, HideInInspector] private Quest questRef;
    [SerializeField, HideInInspector] private QuestControl questControl;

    private void Awake()
    {
        gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        questRef = GameObject.FindGameObjectWithTag("QuestMaster").GetComponent<Quest>();
        questControl = gameMasterRef.gameObject.GetComponent<QuestControl>();
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
            //Debug.LogWarning("TODO - set up code for npc movement");
            if (_currentTarget.GetComponent<WalkingPoint>() == true)
            {
                dismissed = false;
                if (_currentTarget.GetComponent<WalkingPoint>().FinalPoint == true) // if at final point, YEET THYSELF
                    gameMasterRef.gameObject.GetComponent<NPC_Master>().removeNPC(this.gameObject);
                else // otherwise, proceed
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
                else if (dismissed == false)
                {
                    // advance quest
                    //Debug.Log("TODO: setup code for npc quest triggering");
                    
                    if (questControl.CurrentQuest != null &&
                        (questRef.QuestType(questControl.CurrentQuest) == "Tutorial" ||
                        questRef.QuestType(questControl.CurrentQuest) == "Story"))
                    {
                        Debug.Log("TODO re-implement this");
                        if (gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.questStageType == "Force_Event" &&
                            gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.eventData.eventName == "Summon_NPC")
                        {
                            Debug.LogWarning("Quest Notif - NPC at counter");
                            gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();
                        }
                    }
                    

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
    /*public void setupNPC(GameObject NPCRef)
    {

    }*/

    public void setCurentTarget(GameObject target)
    {
        _currentTarget = target;
        _targetPosition = new Vector3(target.transform.position.x, this.gameObject.transform.position.y, target.transform.position.z);
    }

    public string NPCName { get => npcName; }
    public bool IsMoving { set => _move = value; }
    public bool IsDismissed { set => dismissed = value; }
}
