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

    //[SerializeField, HideInInspector] private QuestData _currentStoryQuest;
    [SerializeField, HideInInspector] private TextAsset _currentStoryQuestJson;
    [SerializeField, HideInInspector] private int index = 0;

    [SerializeField] private Quest questRef;

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
        index = gameObject.GetComponent<QuestControl>().CurrentStageIndex;
        if (index < questRef.LoadStoryQuest(_currentStoryQuestJson).questStagesJson.Count)
        {
            //this.gameObject.GetComponent<QuestControl>().CurrentStageIndex = index; // p sure issue is this line
            //index = this.gameObject.GetComponent<QuestControl>().CurrentStageIndex;

            QuestStageJsonData currStage = questRef.LoadStoryQuest(_currentStoryQuestJson).questStagesJson[index]; //_currentStoryQuest.QuestStages[index];
            //Debug.LogWarning("setupDialogueLine - " + _currentStoryQuest.QuestName + " " + index);
            if (currStage.questStageType == "Dialogue")
            {
                _dialogueUI.SetActive(true);
                _nameText.text = currStage.speaker;

                string line = currStage.dialogeLine;
                string temp1 = line.Replace("(playername)", gameObject.GetComponent<GameMaster>().PlayerName);
                string temp2 = temp1.Replace("(shopname)", gameObject.GetComponent<GameMaster>().ShopName);
                _dialogueText.text = temp2;
            }
            else if (currStage.questStageType == "Buy_Item" || currStage.questStageType == "Disassemble_Item" || currStage.questStageType == "Craft_Item" || currStage.questStageType == "Sell_Item" || currStage.questStageType == "Force_Event" || currStage.questStageType == "Have_UI_Open")
            {
                _dialogueUI.SetActive(false);
                //gameObject.GetComponent<QuestControl>().updateQuestProgress(_currentStoryQuest, currStage);

                //Debug.LogWarning("No dialogue for this stage!");
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
        Debug.Log("No more stages! " + questRef.LoadStoryQuest(_currentStoryQuestJson).questName);
        //if (_currentStoryQuest.NextQuest == null)
        _dialogueUI.SetActive(false);
        //gameObject.GetComponent<QuestControl>().updateQuestProgress(_currentStoryQuest, true);
    }
    /*
    public void nextStage()
    {
        index++;
        setupDialogueLine();
    }
    */

    public int CurrentStageIndex { get => index; set => index = value; }
    //public QuestData CurrentQuest { set => _currentStoryQuest = value; }
    public TextAsset CurrentQuestJson { set => _currentStoryQuestJson = value; }
}
