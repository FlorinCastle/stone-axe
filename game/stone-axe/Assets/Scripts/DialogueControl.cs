using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _dialogueText;

    private QuestData _currentStoryQuest;
    private int index = 0;

    public void startDialogue()
    {
        index = 0;
        _dialogueUI.SetActive(true);
        setupDialogueLine();
    }

    public void setupDialogueLine()
    {
        if (index < _currentStoryQuest.QuestStages.Count)
        {
            QuestStage currStage = _currentStoryQuest.QuestStages[index];

            _nameText.text = currStage.DialogueSpeaker;
            _dialogueText.text = currStage.DialogueLine;
            index++;
        }
        else
            Debug.LogWarning("No more stages!");
    }

    public QuestData CurrentQuest { set => _currentStoryQuest = value; }
}
