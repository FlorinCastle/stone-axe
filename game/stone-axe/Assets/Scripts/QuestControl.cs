using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestControl : MonoBehaviour
{
    [SerializeField]
    private Quest _questRef;
    [SerializeField]
    private QuestData _chosenQuest;

    [Header("UI")]
    [SerializeField]
    private Text _questName;
    [SerializeField]
    private Text _questText;
    [SerializeField]
    private Button _newQuestButton;

    [SerializeField] private List<QuestData> _repeatableQuests;

    private void Awake()
    {
        _repeatableQuests = _questRef.getRepeatableQuests();
        setupText();
    }

    public void chooseNewQuest()
    {
        int index = Random.Range(0, _repeatableQuests.Count);
        _chosenQuest = _repeatableQuests[index];

        //Debug.Log("chosen quest: " + _chosenQuest.QuestName);
        setupText();
        checkQuest();
    }

    public void setupText()
    {
        if (_chosenQuest != null)
        {
            _questName.text = _chosenQuest.QuestName;
            _questText.text = _chosenQuest.QuestDiscription;
        }
        else
        {
            _questName.text = "quest name";
            _questText.text = "placeholder";
        }
    }

    public void checkQuest()
    {
        if (_chosenQuest != null)
        {

        }
    }
}
