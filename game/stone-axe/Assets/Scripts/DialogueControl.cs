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
            this.gameObject.GetComponent<QuestControl>().CurrentStageIndex = index;
            QuestStage currStage = _currentStoryQuest.QuestStages[index];
            if (currStage.StageType == "Dialogue")
            {
                _nameText.text = currStage.DialogueSpeaker;
                _dialogueText.text = currStage.DialogueLine;
            }
            else if (currStage.StageType == "Buy_Item" || currStage.StageType == "Disassemble_Item" || currStage.StageType == "Craft_Item" || currStage.StageType == "Sell_Item" || currStage.StageType == "Force_Event" )
            {
                _dialogueUI.SetActive(false);
                this.gameObject.GetComponent<QuestControl>().updateQuestProgress(_currentStoryQuest, currStage);

                Debug.LogWarning("No dialogue for this stage!");
            }
            index++;
        }
        else
        {
            Debug.LogWarning("No more stages!");
            this.gameObject.GetComponent<QuestControl>().updateQuestProgress(_currentStoryQuest, true);
            _dialogueUI.SetActive(false);
        }
    }

    public QuestData CurrentQuest { set => _currentStoryQuest = value; }
}
