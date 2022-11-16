using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestSheet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _bodyText;
    //[SerializeField] private QuestData _questRef;
    [SerializeField] private TextAsset _questJsonRef;
    private QuestControl questControl;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _rerollButton;
    [SerializeField] private Button _selfButtonRef;

    private void Awake()
    {
        questControl = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<QuestControl>();
        _acceptButton.interactable = false;
        _rerollButton.interactable = false;
    }

    public void selectQuest()
    {
        if (questControl.questChosen() == false)
        {
            questControl.chooseQuestSheet(this);
            _acceptButton.interactable = true;
            _rerollButton.interactable = true;
        }
    }

    public void confirmQuest()
    {
        _bodyText.text = "quest accepted";
        _selfButtonRef.interactable = false;
        _acceptButton.interactable = false;
        _rerollButton.interactable = false;
    }

    public void setQuestDetails()
    {
        /*if (_questRef != null)
        {
            _headerText.text = _questRef.QuestName;
            _bodyText.text = "quest type: " + _questRef.QuestType + "\n\n" + _questRef.QuestDiscription + "\n\nquest level: " + _questRef.RequiredPlayerLevel;
            _selfButtonRef.interactable = true;
        }*/
        if (_questJsonRef != null)
        {
            BaseQuestJsonData quest = JsonUtility.FromJson<BaseQuestJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(_questJsonRef).Replace("Assets", "")));

            _headerText.text = quest.questName;
            _bodyText.text = "quest type: " + quest.questType + "\n\n" + quest.questDescription + "\n\nquest level: " + quest.questPrereqs.requiredPlayerLevel;
            _selfButtonRef.interactable = true;
        }
        else
            Debug.LogWarning("Quest ref for " + this.gameObject.name + " is not assigned");
    }

    //public QuestData Quest { get => _questRef; set => _questRef = value; }
    public TextAsset QuestJson { get => _questJsonRef; set => _questJsonRef = value; }
}
