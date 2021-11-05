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
    [SerializeField]
    private Button _completeQuestButton;

    [SerializeField] private List<QuestData> _repeatableQuests;
    private int reqItemCount = 0;
    private int currentItemCount = 0;
    private bool shut = false;

    private void Awake()
    {
        _repeatableQuests = _questRef.getRepeatableQuests();
        setupText();
        
    }

    private void FixedUpdate()
    {
        if (_chosenQuest != null && shut)
            if (_chosenQuest.QuestType == "OD_Material")
                updateQuestProgress(_chosenQuest.ReqiredMaterial);
    }

    public void chooseNewQuest()
    {
        int index = Random.Range(0, _repeatableQuests.Count);
        _chosenQuest = _repeatableQuests[index];

        //Debug.Log("chosen quest: " + _chosenQuest.QuestName);
        setupQuest();
        setupText();
        _completeQuestButton.interactable = false;
    }

    public void setupText()
    {
        if (_chosenQuest != null)
        {
            _questName.text = _chosenQuest.QuestName;
            _questText.text = _chosenQuest.QuestDiscription;
            if (_chosenQuest.QuestType == "OCC_Item")
            {
                _questName.text += " (" + currentItemCount + "/" + reqItemCount + ")";
                _questText.text += ": " + _chosenQuest.RequiredItem.ItemName;
            }
            else if (_chosenQuest.QuestType == "OD_Material")
            {
                _questName.text += " (" + _chosenQuest.ReqiredMaterial.MaterialCount + "/" + reqItemCount + ")";
                _questText.text += ": " + _chosenQuest.ReqiredMaterial.Material;
                shut = true;
            }
        }
        else
        {
            _questName.text = "quest name";
            _questText.text = "placeholder";
        }
    }

    public void setupQuest()
    {
        if (_chosenQuest != null)
        {
            if (_chosenQuest.QuestType == "OCC_Item")
            {
                reqItemCount = 1;
            }
            else if (_chosenQuest.QuestType == "OCC_QuestItem")
            {
                reqItemCount = 1;
            }
            else if (_chosenQuest.QuestType == "OD_Material")
            {
                reqItemCount = _chosenQuest.ReqiredCount;
            }
            else if (_chosenQuest.QuestType == "OCC_TotalCrafted")
            {
                reqItemCount = _chosenQuest.ReqiredCount;
            }
        }
    }

    //overload 1 (basic item crafting)
    public void updateQuestProgress(ItemData craftedItemRecipe)
    {
        if (_chosenQuest != null)
        {
            if (_chosenQuest.QuestType == "OCC_Item" ||
                _chosenQuest.QuestType == "OCC_TotalCrafted")
            {
                if (_chosenQuest.RequiredItem == craftedItemRecipe)
                {
                    currentItemCount++;
                    setupText();
                    if (currentItemCount >= reqItemCount)
                    {
                        //Debug.Log("TODO: quest can be completed");
                        _completeQuestButton.interactable = true;
                    }
                    else
                    {
                        _completeQuestButton.interactable = false;
                    }

                }
            }
        }
    }
    /* // overload 2 (quest item crafting)
    public void updateQuestProgress()
    {

    }
    */
    // overload 3 (material quest)
    public void updateQuestProgress(MaterialData materialData)
    {
        if (_chosenQuest != null)
            if (_chosenQuest.QuestType == "OD_Material")
            {
                if (_chosenQuest.ReqiredMaterial == materialData)
                {
                    if (materialData.MaterialCount >= _chosenQuest.ReqiredCount)
                    {
                        //Debug.Log("TODO: quest can be completed");
                        shut = false;
                        _completeQuestButton.interactable = true;
                    }
                    else
                        _completeQuestButton.interactable = false;
                }
            }
    }

    // overload 4 (story quest)

    // overload 5 (tutorial quest)

    public void completeQuest()
    {
        _completeQuestButton.interactable = false;
        if (_chosenQuest.QuestType == "OD_Material")
        {
            _chosenQuest.ReqiredMaterial.RemoveMat(_chosenQuest.ReqiredCount);
        }
        _chosenQuest = null;
        setupText();
        currentItemCount = 0;
    }
}
