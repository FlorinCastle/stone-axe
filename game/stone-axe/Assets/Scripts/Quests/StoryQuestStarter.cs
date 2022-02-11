using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryQuestStarter : MonoBehaviour
{
    [Header("Data")]
    //[SerializeField]
    private QuestControl _questControlRef;
    [SerializeField] private QuestData _questRef;
    [Header("UI")]
    [SerializeField] private GameObject _basicDetails;
    [SerializeField] private TextMeshProUGUI _text; 

    private void Awake()
    {
        _questControlRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<QuestControl>();
    }

    public void startQuest()
    {
        _questControlRef.startStoryQuest(_questRef);
        _questControlRef.removeStarter();
    }

    public void setupText()
    {
        _text.text = _questRef.QuestName;
    }

    public void showDetails() { _basicDetails.SetActive(true); }
    public void hideDetails() { _basicDetails.SetActive(false); }

    public QuestData QuestRef { get => _questRef; set => _questRef = value; }
}
