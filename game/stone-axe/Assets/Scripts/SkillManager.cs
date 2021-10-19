using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private GameMaster _gameMasterRef;
    [SerializeField] private int _totalSkillPoints;
    [SerializeField] private int _currentSkillPoints;

    [Header("UI")]
    [SerializeField] private Text _skillPointsText;
    [SerializeField] private GameObject _economicSkillsContent;
    [SerializeField] private GameObject _disassemblySkillsContent;
    [SerializeField] private GameObject _craftingSkillsContent;

    [Header("Prefabs")]
    [SerializeField] private GameObject _skillPrefab;

    [Header("Skill Tracking")]
    [SerializeField] private List<GameObject> _economicSkills;
    [SerializeField] private List<GameObject> _disasemblySkills;
    [SerializeField] private List<GameObject> _craftingSkills;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    public void updateSkillPoints()
    {
        _skillPointsText.text = _currentSkillPoints.ToString();
    }


    public void setTotalSkillPoints(int value) { _totalSkillPoints = value; }
    public int GetTotalSkillPoints { get => _totalSkillPoints; }

    public void setCurrentSkillPoints(int value) { _currentSkillPoints = value; }
    public int GetCurrentSkillPoints { get => _currentSkillPoints; }
    public void AddSkillPoint() { _currentSkillPoints++; }
}
