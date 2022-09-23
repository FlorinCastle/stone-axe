using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryQuestStarter : MonoBehaviour
{
    [Header("Data")]
    //[SerializeField]
    private QuestControl _questControlRef;
    [SerializeField] private QuestData _questRef;
    [SerializeField] private BaseQuestJsonData _questJsonData;
    [SerializeField] private GameObject _selfRef;
    [Header("UI")]
    [SerializeField] private GameObject _basicDetails;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _buttonRef;

    private void Awake()
    {
        _questControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<QuestControl>();
    }

    public void startQuest()
    {
        if (_questControlRef.gameObject.GetComponent<GameMaster>().GetLevel >= _questRef.RequiredPlayerLevel &&
            _questControlRef.CurrentQuest == null)
        {
            _questControlRef.startStoryQuest(_questRef);
            //_questControlRef.removeStarter();
            _questControlRef.removeStarter(_selfRef);
            hideDetails();
        }
        else
        {
            StartCoroutine(cantStartQuest());
        }
    }

    public void setupText()
    {
        if (_questRef != null) _text.text = _questRef.QuestName;
        else if (_questJsonData != null) _text.text = _questJsonData.questName;
        else Debug.LogError(gameObject.name + ".StoryQuestStarter: _questRef and _questJsonData is null!");
    }

    public void showDetails() { _basicDetails.SetActive(true); }
    public void hideDetails() { _basicDetails.SetActive(false); }

    public QuestData QuestRef { get => _questRef; set => _questRef = value; }
    public BaseQuestJsonData QuestJsonRef { get => _questJsonData; set => _questJsonData = value; }
    public GameObject SelfRef { get => _selfRef; }

    IEnumerator cantStartQuest()
    {
        if (_questControlRef.CurrentQuest != null)
        {
            _text.text = "Quest already active!";
        }
        else
            _text.text = "Level not high enough! Level: " + _questRef.RequiredPlayerLevel;

        yield return new WaitForSeconds(5f);
        setupText();

        yield return null;
    }
}
