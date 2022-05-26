using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [SerializeField, HideInInspector] private QuestData _currentStoryQuest;
    [SerializeField, HideInInspector] private int index = 0;

    public void startDialogue()
    {
        index = 0;
        _dialogueUI.SetActive(true);
        setupDialogueLine();
    }
    public void startDialogue (int forceIndex)
    {
        index = forceIndex;
        //Debug.LogWarning("startDialogue - " + _currentStoryQuest.QuestName + " " + index);
        _dialogueUI.SetActive(true);
        setupDialogueLine();
    }

    public void setupDialogueLine()
    {
        index = this.gameObject.GetComponent<QuestControl>().CurrentStageIndex;
        if (index < _currentStoryQuest.QuestStages.Count)
        {
            //this.gameObject.GetComponent<QuestControl>().CurrentStageIndex = index; // p sure issue is this line
            //index = this.gameObject.GetComponent<QuestControl>().CurrentStageIndex;

            QuestStage currStage = _currentStoryQuest.QuestStages[index];
            //Debug.LogWarning("setupDialogueLine - " + _currentStoryQuest.QuestName + " " + index);
            if (currStage.StageType == "Dialogue")
            {
                _dialogueUI.SetActive(true);
                _nameText.text = currStage.DialogueSpeaker;

                string line = currStage.DialogueLine;
                string temp1 = line.Replace("(playername)", gameObject.GetComponent<GameMaster>().PlayerName);
                string temp2 = temp1.Replace("(shopname)", gameObject.GetComponent<GameMaster>().ShopName);
                _dialogueText.text = temp2;
            }
            else if (currStage.StageType == "Buy_Item" || currStage.StageType == "Disassemble_Item" || currStage.StageType == "Craft_Item" || currStage.StageType == "Sell_Item" || currStage.StageType == "Force_Event" || currStage.StageType == "Have_UI_Open")
            {
                _dialogueUI.SetActive(false);
                this.gameObject.GetComponent<QuestControl>().updateQuestProgress(_currentStoryQuest, currStage);

                Debug.LogWarning("No dialogue for this stage!");
            }
            index++;
        }
        else
        {
            dialogeQuestEnd();
        }
    }
    public void dialogeQuestEnd()
    {
        Debug.LogWarning("No more stages! " + _currentStoryQuest.QuestName);
        //if (_currentStoryQuest.NextQuest == null)
        _dialogueUI.SetActive(false);
        this.gameObject.GetComponent<QuestControl>().updateQuestProgress(_currentStoryQuest, true);
    }
    /*
    public void nextStage()
    {
        index++;
        setupDialogueLine();
    }
    */

    public int CurrentStageIndex { get => index; set => index = value; }
    public QuestData CurrentQuest { set => _currentStoryQuest = value; }
}
