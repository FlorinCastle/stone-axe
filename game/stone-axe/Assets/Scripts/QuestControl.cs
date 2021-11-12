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

    [SerializeField] private QuestSheet _questSheet1;
    [SerializeField] private QuestSheet _questSheet2;
    [SerializeField] private QuestSheet _questSheet3;

    private QuestSheet _selectedSheet;

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
    }

    private void Start()
    {
        setupQuest1();
        setupQuest2();
        setupQuest3();
    }

    private void FixedUpdate()
    {
        if (_chosenQuest != null && shut)
            if (_chosenQuest.QuestType == "OD_Material")
                updateQuestProgress(_chosenQuest.ReqiredMaterial);
    }

    public void chooseQuestSheet(QuestSheet input)
    {
        if (input == _questSheet1)
        {
            _selectedSheet = _questSheet1;
        }
        else if (input == _questSheet2)
        {
            _selectedSheet = _questSheet2;
        }
        else if (input == _questSheet3)
        {
            _selectedSheet = _questSheet3;
        }
    }

    public void setupQuest1()
    {
        if (_questSheet1 != null)
        {
            int i = Random.Range(0, _repeatableQuests.Count);
            _questSheet1.Quest = _repeatableQuests[i];
            _questSheet1.setQuestDetails();

        }
        else Debug.LogWarning("Quest sheet 1 is not assigned");
    }
    public void setupQuest2()
    {
        if (_questSheet2 != null)
        {
            int j = Random.Range(0, _repeatableQuests.Count);
            _questSheet2.Quest = _repeatableQuests[j];
            _questSheet2.setQuestDetails();

        }
        else Debug.LogWarning("Quest sheet 2 is not assigned");
    }
    public void setupQuest3()
    {
        if (_questSheet3 != null)
        {
            int k = Random.Range(0, _repeatableQuests.Count);
            _questSheet3.Quest = _repeatableQuests[k];
            _questSheet3.setQuestDetails();

        }
        else Debug.LogWarning("Quest sheet 3 is not assigned");
    }

    public void acceptQuest()
    {
        setupQuest();
        setupText();
    }

    public void rerollQuest()
    {
        if (_selectedSheet == _questSheet1)
            setupQuest1();
        else if (_selectedSheet == _questSheet2)
            setupQuest2();
        else if (_selectedSheet == _questSheet3)
            setupQuest3();
        else
            Debug.LogWarning("no quest sheet selected!");
    }

    public void setupText()
    {
        if (_chosenQuest != null)
        {
            _questName.text = _chosenQuest.QuestName;
            _questText.text = _chosenQuest.QuestDiscription;
            if (_chosenQuest.QuestType == "OCC_Item" || _chosenQuest.QuestType == "OCC_TotalCrafted")
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
        if (_selectedSheet != null)
        {
            _chosenQuest = _selectedSheet.Quest;
            _selectedSheet.confirmQuest();
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

    public bool questChosen()
    {
        if (_chosenQuest != null)
            return true;
        else
            return false;
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
        rerollQuest();
        _selectedSheet = null;
    }
}