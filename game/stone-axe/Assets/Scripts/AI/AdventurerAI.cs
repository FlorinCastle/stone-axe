using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAI : MonoBehaviour
{
    // REMINDER: forward is along the z-Axis of the object
    private AdventurerMaster _advMaster;
    [SerializeField] private AdventurerData _advRaceRef;
    private Vector4 _advColorRef;
    [SerializeField]
    private bool _move;
    private bool dismissed;

    [SerializeField] private GameObject _currentTarget;
    private GameObject _prevTarget;
    private Vector3 _targetPosition;

    [SerializeField] private GameObject chatty_focus;

    [SerializeField] private GameObject mySpeechBubble;

    [Header("Body Refs")]
    [SerializeField] private GameObject _headMark;

    private GameMaster gameMasterRef;
    private Quest questRef;
    private QuestControl questControlRef;

    private void Awake()
    {
        gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        questRef = GameObject.FindGameObjectWithTag("QuestMaster").GetComponent<Quest>();
        questControlRef = gameMasterRef.gameObject.GetComponent<QuestControl>();
        _advMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<AdventurerMaster>();
    }

    private Vector3 newDirection;
    private bool selected = false;
    private bool beingHandled = false;
    private void FixedUpdate()
    {
        if (_move == true) // if moving
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, _targetPosition, 0.1f);
            gameObject.GetComponent<Animator>().SetBool("Walk", true);
        }
        else if (_move == false) // rotate toward next point, but don't move
        {
            Vector3 targetDirection = _targetPosition - this.gameObject.transform.position;
            float singleStep = 2.0f * Time.deltaTime;

            newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
            gameObject.GetComponent<Animator>().SetBool("Walk", false);
        }

        if (this.gameObject.transform.position.x == _targetPosition.x && this.gameObject.transform.position.z == _targetPosition.z) // if adventurer is at a point
        {
            //Debug.Log("position reached");
            IsMoving = false;
            //gameObject.GetComponent<Animator>().SetBool("Walk", false);

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
                    if (beingHandled == false)
                    {
                        StartCoroutine(WaitThenGo());
                    }
                }
                else if (_currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<LinePoint>() != null && _currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<LinePoint>().IsOccupied == true)
                { // if at line point and next point is also a line point that has an adventurer waiting
                    dismissed = false;
                    gameObject.GetComponent<Animator>().SetBool("Walk", false);
                    /*
                    if (_prevTarget.GetComponent<LinePoint>() == true)
                        _prevTarget.GetComponent<LinePoint>().IsOccupied = false;
                    */
                }
                else if (dismissed == true && _currentTarget.GetComponent<LinePoint>().NextPoint.GetComponent<WalkingPoint>() != null)
                { // if at line point and next point is a walking point and adventurer has been dismissed (head of line, basically)
                    _currentTarget.GetComponent<LinePoint>().IsOccupied = false;
                    gameMasterRef.AdventurerAtCounter = false;
                    if (selected == false)
                    {
                        selected = true;
                        gameMasterRef.adventurerEco(this);
                    }

                    dismissed = false;
                    gameObject.GetComponent<Animator>().SetBool("Walk", true);
                    setCurentTarget(_currentTarget.GetComponent<LinePoint>().NextPoint);

                    mySpeechBubble.GetComponent<SpeechBubbleTrackObject>().chat("and im off");
                }
                else if (_currentTarget.GetComponent<LinePoint>().HeadOfLine == true)
                { // if at line point that is head of line
                    gameMasterRef.AdventurerAtCounter = true;
                    gameObject.GetComponent<Animator>().SetBool("Walk", false);
                    if (selected == false)
                    {
                        selected = true;
                        gameMasterRef.adventurerEco(this);
                    }

                    if (questControlRef.CurrentQuest != null &&
                        (questRef.QuestType(questControlRef.CurrentQuest) == "Tutorial" ||
                        questRef.QuestType(questControlRef.CurrentQuest) == "Story")) 
                    {
                        //Debug.Log("TODO rewrite this");
                        if (questControlRef.CurrentStage.questStageType == "Force_Event" &&
                            questControlRef.CurrentStage.eventData.eventName == "Summon_Adventurer")
                        {
                            //Debug.Log("Quest Notif - Adventurer at counter");
                            questControlRef.nextStage();
                        }
                        /*if (gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.StageType == "Force_Event" &&
                            gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.QuestEvent == "Summon_Adventurer")
                        {
                            Debug.Log("Quest Notif - Adventurer at counter");
                            gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();
                        }*/
                    }
                }
            }
        }

        if (Vector3.Angle(transform.forward, _targetPosition - this.transform.position) < 10f && !(this.gameObject.transform.position.x == _targetPosition.x && this.gameObject.transform.position.z == _targetPosition.z))
        {
            IsMoving = true;
        }
    }

    public void setupAdventurer()
    {
        chooseRace();
        setupAdventurerModel();
        chooseAdvColor();

        gameObject.name = _advRaceRef.AdventurerSpecies + "_Color" + _advMaster.getAdvColorRef(_advRaceRef.AdventurerSpecies, _advColor);
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
    //private Renderer _renderer;
    private Renderer[] _renderers;
    private MaterialPropertyBlock _propBlock;
    [SerializeField] private Color32 _advColor;
    private void chooseAdvColor()
    {
        _renderers = this.gameObject.GetComponentsInChildren<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        _advColor = _advMaster.chooseAdvColor(_advRaceRef.AdventurerSpecies);
        foreach (Renderer ren in _renderers)
        {
            ren.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", _advColor);
            ren.SetPropertyBlock(_propBlock);
        }
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

    private IEnumerator WaitThenGo()
    {
        //Debug.Log(gameObject.name + ".AdventurerAI.WaitThenGo() Coroutine has started!");
        beingHandled = true;

        //if (mySpeechBubble != null)
        //    mySpeechBubble.GetComponent<SpeechBubbleTrackObject>().chat();

        if (_advMaster.GetMyIndex(gameObject) != 0)
        {
            Debug.Log(gameObject.name + ".AdventurerAI.WaitThenGo(): waiting for " + _advMaster.AdventurerWaitTime + " seconds");
            yield return new WaitForSeconds(_advMaster.AdventurerWaitTime);
        }
        _currentTarget.GetComponent<LinePoint>().IsOccupied = false;
        dismissed = false;
        setCurentTarget(_currentTarget.GetComponent<LinePoint>().NextPoint);

        beingHandled = false;
    }

    public GameObject CurrentTarget { get => _currentTarget; }
    public string AdventurerType { get => _advRaceRef.AdventurerSpecies; }
    public GameObject MyChatBubble { get => mySpeechBubble; set => mySpeechBubble = value; }
    public GameObject ChattyFocus { get => chatty_focus; }
}
