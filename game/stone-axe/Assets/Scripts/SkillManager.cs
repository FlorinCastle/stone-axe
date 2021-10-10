using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private GameMaster _gameMasterRef;
    [SerializeField] private int _totalSkillPoints;
    [SerializeField] private int _currentSkillPoints;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }



    public void setTotalSkillPoints(int value) { _totalSkillPoints = value; }
    public int GetTotalSkillPoints { get => _totalSkillPoints; }

    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
}
