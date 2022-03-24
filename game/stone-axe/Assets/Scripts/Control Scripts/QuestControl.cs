using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestControl : MonoBehaviour
{
    [SerializeField]
    private Quest _questRef;
    private InventoryData _invDataRef;
    [SerializeField]
    private QuestData _chosenQuest;
    [SerializeField]
    private int _currStageIndex = 0;

    [SerializeField] private QuestSheet _questSheet1;
    [SerializeField] private QuestSheet _questSheet2;
    [SerializeField] private QuestSheet _questSheet3;

    private QuestSheet _selectedSheet;
    [SerializeField]
    private GameObject _storyQuestPopupParent;
    [SerializeField]
    private GameObject _questStarterPrefab;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI _questName;
    [SerializeField]
    private Text _questText;
    [SerializeField]
    private Button _newQuestButton;
    [SerializeField]
    private Button _completeQuestButton;

    [Header("Quest Organization")]
    [SerializeField] private List<QuestData> _repeatableQuests;
    [SerializeField, HideInInspector] private List<QuestData> _unlockedQuests;
    [SerializeField, HideInInspector] private List<GameObject> _questStarterGOs;
    [SerializeField] private QuestData _enableAdventurersOnComplete;

    private int reqItemCount = 0;
    private int currentItemCount = 0;
    private bool shut = false;
    private bool star = false;

    private void Awake()
    {
        _repeatableQuests = _questRef.getRepeatableQuests();
        _invDataRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>();
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

        if (this.gameObject.GetComponent<UIControl>().ShopUIActive == true && star == false)
        {
            setupStoryQuests();
            star = true;
        }
    }

    [SerializeField] private QuestData starterRef;
    private GameObject p;
    public void setupStoryQuests()
    {
        //Debug.Log("setting up story quests!");
        if (_chosenQuest == null)
        {
            foreach(QuestData tutQuest in _questRef.getTutorialQuests())
            {
                if (starterRef == null)
                {
                    //Debug.Log("tutorial: starter ref is not assigned!");
                    if (tutQuest.StoryQuestComplete == false)
                    {
                        //Debug.Log("asigning starter tutorial");
                        starterRef = tutQuest;
                        break;
                    }
                }
                else
                    break;
            }
            foreach(QuestData storyQuest in _questRef.getStoryQuests())
            {
                if (starterRef == null)
                {
                    //Debug.Log("story: starter ref is not assigned!");
                    if (storyQuest.StoryQuestComplete == false)
                    {
                        //Debug.Log("asigning starter story");
                        starterRef = storyQuest;
                        break;
                    }
                }
                else
                    break;
            }
            setupStarter();
        }
        else if (_chosenQuest.QuestType == "Tutorial" || _chosenQuest.QuestType == "Story")
        {
            //Destroy(p);
            //p = null;
            if (_chosenQuest.StoryQuestComplete == false)
                setupStarter();
            else
            {
                //Debug.Log("TODO: set up code to set up starters for unlocked and not complete quests");
                foreach (QuestData unlockQuest in _unlockedQuests)
                    if (unlockQuest.StoryQuestComplete == false)
                    {
                        //Debug.Log(unlockQuest.QuestName + " is not complete!");
                        setupStarter(unlockQuest);
                    }
            }
        }
    }

    public void setupStarter()
    {
        if (_chosenQuest != null || starterRef != null)
        {
            p = Instantiate(_questStarterPrefab, _storyQuestPopupParent.transform);
            if (_chosenQuest == null)
                p.GetComponent<StoryQuestStarter>().QuestRef = starterRef;
            else
                p.GetComponent<StoryQuestStarter>().QuestRef = _chosenQuest;

            _questStarterGOs.Add(p);
            p.GetComponent<StoryQuestStarter>().setupText();
        }
    }
    public void setupStarter(QuestData qes)
    {
        p = Instantiate(_questStarterPrefab, _storyQuestPopupParent.transform);
        if (qes != null)
        {
            p.GetComponent<StoryQuestStarter>().QuestRef = qes;

            _questStarterGOs.Add(p);
            p.GetComponent<StoryQuestStarter>().setupText();
        }
        else
            Debug.LogError("QuestControl.setupStarter(QuestData qes) - qes is Null!");
    }

    public void removeStarter()
    {
        if (p != null)
        {
            Destroy(p);
            p = null;
        }
        else if (p == null)
            Debug.LogWarning("QuestControl - Quest Starter Game Object (variable p) is null!");
    }
    public void removeStarter(GameObject g)
    {
        if (g != null)
        {
            foreach(GameObject go in _questStarterGOs)
                if (g.gameObject == go.gameObject)
                {
                    Debug.Log("QuestControl _questStarterGOs contains starter!");
                    int i = _questStarterGOs.IndexOf(go);
                    Destroy(go);
                    _questStarterGOs.RemoveAt(i);
                    break;
                }
        }
        else if (g == null)
            Debug.LogWarning("QuestControl> removeStater(GameObject g) - (variable g) is null!");
    }

    public void resetAllQuests()
    {
        foreach (QuestData tutQuest in _questRef.getTutorialQuests())
            tutQuest.StoryQuestComplete = false;
        foreach (QuestData storyQuest in _questRef.getStoryQuests())
            storyQuest.StoryQuestComplete = false;
    }

    public SaveQuestsObject saveQuests()
    {

        List<QuestObject> completedQuestList = new List<QuestObject>();
        List<QuestObject> unlockedQuestList = new List<QuestObject>(); 
        foreach(QuestData tutQuest in _questRef.getTutorialQuests())
            if (tutQuest.StoryQuestComplete == true)
                completedQuestList.Add(_questRef.saveQuest(tutQuest));

        foreach (QuestData stryQuest in _questRef.getStoryQuests())
            if (stryQuest.StoryQuestComplete == true)
                completedQuestList.Add(_questRef.saveQuest(stryQuest));

        foreach (QuestData unlockQuest in _unlockedQuests)
            unlockedQuestList.Add(_questRef.saveQuest(unlockQuest));

        SaveQuestsObject questObject = new SaveQuestsObject
        {
            currentQuest = _questRef.saveQuest(_chosenQuest),
            currentQuestStage = _currStageIndex,
            completedQuests = completedQuestList,
            unlockedQuests = unlockedQuestList,
        };

        return questObject;
    }
    public void LoadQuests(SaveQuestsObject questsSave)
    {
        if (questsSave.currentQuest != null)
        {
            foreach (QuestData quest in _questRef.getAllQuests())
                if (quest.QuestName == questsSave.currentQuest.questName)
                {
                    _chosenQuest = quest;
                    _currStageIndex = questsSave.currentQuestStage;
                }
            setupText();
        }
        foreach (QuestObject quest in questsSave.completedQuests)
        {
            if (quest.questType == "Tutorial")
            {
                foreach (QuestData tutQuest in _questRef.getTutorialQuests())
                {
                    if (tutQuest.QuestName == quest.questName)
                        tutQuest.StoryQuestComplete = true;
                    if (tutQuest == _enableAdventurersOnComplete)
                    {
                        gameObject.GetComponent<GameMaster>().toggleAdventurers(true);
                        // ui control, enable to market button
                        gameObject.GetComponent<GameMaster>().marketAccessable(true);
                    }
                }
            }
            else if (quest.questType == "Story")
            {
                foreach (QuestData storyQuest in _questRef.getStoryQuests())
                    if (storyQuest.QuestName == quest.questName)
                        storyQuest.StoryQuestComplete = true;
            }
        }
        foreach (QuestObject quest in questsSave.unlockedQuests)
        {
            if (quest.questType == "Tutorial")
            {
                foreach (QuestData tutQuest in _questRef.getTutorialQuests())
                    if (tutQuest.QuestName == quest.questName)
                        _unlockedQuests.Add(tutQuest);
            }
            else if (quest.questType == "Story")
            {
                foreach (QuestData storyQuest in _questRef.getStoryQuests())
                    if (storyQuest.QuestName == quest.questName)
                        _unlockedQuests.Add(storyQuest);
            }
        }

        setupStoryQuests();
    }

    public void forceSetQuest(QuestData quest)
    {
        _chosenQuest = quest;
        Debug.Log("chosen quest is: " + quest.QuestName);
        setupText();
        if (quest.QuestType == "Tutorial" || quest.QuestType == "Story")
        {
            _currStageIndex = 0;
            //startStoryQuest(quest);
        }
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
                //_questName.text += " (" + _chosenQuest.ReqiredMaterial.MaterialCount + "/" + reqItemCount + ")";
                _questName.text += " (" + _invDataRef.getMaterialCount(_chosenQuest.ReqiredMaterial) + "/" + reqItemCount + ")";
                _questText.text += ": " + _chosenQuest.ReqiredMaterial.Material;
                shut = true;
            }
            else if (_chosenQuest.QuestType == "Tutorial")
            {
                _questName.text += "";
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
                GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>().setupQuestRecipeGrid();
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

    public void startStoryQuest(QuestData questInput)
    {
        _chosenQuest = questInput;
        _currStageIndex = 0;
        setupText();
        this.gameObject.GetComponent<DialogueControl>().CurrentQuest = questInput;

        if (questInput.QuestStages[_currStageIndex].StageType == "Dialogue")
            this.gameObject.GetComponent<DialogueControl>().startDialogue(_currStageIndex);
        else
            Debug.LogError("This quest starts with a non-Dialogue stage!\nNote to Dev: Implement this quest's start in code!\nLine: 411 Method: startStoryQuest(QuestData questInput)");
    }
    public void nextStage()
    {
        _currStageIndex++;
        if (_currStageIndex < CurrentQuest.QuestStages.Count)
            this.gameObject.GetComponent<DialogueControl>().setupDialogueLine();
        else if (_currStageIndex >= CurrentQuest.QuestStages.Count)
            this.gameObject.GetComponent<DialogueControl>().dialogeQuestEnd();

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
                        //Debug.Log("TODO: quest can be completed");
                        _completeQuestButton.interactable = true; 
                    else 
                        _completeQuestButton.interactable = false; 
                }
            }
            else if (_chosenQuest.QuestType == "Tutorial" || _chosenQuest.QuestType == "Story")
            {
                Debug.LogWarning("PH");
                updateQuestProgress(_chosenQuest, _chosenQuest.QuestStages[_currStageIndex]);
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
                    //if (materialData.MaterialCount >= _chosenQuest.ReqiredCount)
                    if (_invDataRef.getMaterialCount(materialData) >= _chosenQuest.ReqiredCount)
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

    // overload 4 (story & tutorial quest)
    public void updateQuestProgress(QuestData quest, bool isComplete)
    {
        //Debug.Log("updating quest progress");
        if (quest.QuestType == "Tutorial")
        {
            quest.StoryQuestComplete = isComplete;
            if (quest == _enableAdventurersOnComplete)
            {
                gameObject.GetComponent<GameMaster>().toggleAdventurers(true);
                gameObject.GetComponent<GameMaster>().marketAccessable(true);
                gameObject.GetComponent<UIControl>().shopAllTabsAccessable();
            }

            if (quest.NextQuest != null && isComplete == true)
            {
                Debug.Log("this quest is complete; next quest is not null");
                //forceSetQuest(quest.NextQuest);
                foreach (QuestData unlockedQ in quest.QuestUnlocks)
                    _unlockedQuests.Add(unlockedQ);
                setupStoryQuests();
                _chosenQuest = null;
            }
            if (quest.NextQuest == null && isComplete == true)
            {
                foreach (QuestData unlockedQ in quest.QuestUnlocks)
                    _unlockedQuests.Add(unlockedQ);
                setupStoryQuests();
                _chosenQuest = null;
            }
        }
    }
    public void updateQuestProgress(QuestData quest, QuestStage currStage)
    {
        if (currStage.StageType == "Buy_Item")
        {
            Debug.LogWarning("Quest Stage: Buy item!");
            // code should work
            if (quest.QuestType == "Tutorial")
            {
                gameObject.GetComponent<UIControl>().shopBuyAccessableOnly();
            }
        }
        else if (currStage.StageType == "Sell_Item")
        {
            Debug.LogWarning("Quest Stage: Sell item!");
            if (quest.QuestType == "Tutorial")
            {
                gameObject.GetComponent<UIControl>().shopSellAccessableOnly();
            }
        }
        else if (currStage.StageType == "Disassemble_Item")
        {
            Debug.LogWarning("Quest Stage: Disassemble item!");
            // code should work
            if (quest.QuestType == "Tutorial")
            {
                gameObject.GetComponent<UIControl>().shopDisassembleAccessableOnly();
            }
        }
        else if (currStage.StageType == "Craft_Item")
        {
            Debug.LogWarning("Quest Stage: Craft item!");
            if (quest.QuestType == "Tutorial")
            {
                gameObject.GetComponent<UIControl>().shopCraftAccessableOnly();
            }
        }
        else if (currStage.StageType == "Have_Currency")
        {
            Debug.LogWarning("Quest Stage: Have currency!");
        }
        else if (currStage.StageType == "Force_Event")
        {
            Debug.LogWarning("Quest Stage: Force Event!");
            if (currStage.QuestEvent == "Summon_Adventurer")
            {
                Debug.LogWarning("Quest Event: Summon Adventurer");
                this.gameObject.GetComponent<AdventurerMaster>().spawnAdventurer();
            }
            else if (currStage.QuestEvent == "Summon_NPC")
            {
                Debug.LogWarning("Quest Event: Summon Story NPC");
                if (currStage.NPCRef != null) this.gameObject.GetComponent<NPC_Master>().spawnNPC(currStage.NPCRef);
                else Debug.LogError("NPC Ref for " + quest.QuestName + " Stage: " + currStage.name + " is not asigned!");
            }
            else if (currStage.QuestEvent == "Dismiss_Quest_NPC")
            {
                Debug.LogWarning("Quest Event: Dismiss Story NPCs");
                this.gameObject.GetComponent<NPC_Master>().dismissNPCs();
            }
            else if (currStage.QuestEvent == "Get_Item")
            {
                Debug.LogWarning("Quest Event: Get Item");
                this.gameObject.GetComponent<GenerateItem>().GeneratePresetItem(currStage.ItemToGet, currStage.Part1Mat, currStage.Part2Mat, currStage.Part3Mat, true);
                nextStage();
            }
            else if (currStage.QuestEvent == "Force_For_Sale")
            {
                Debug.LogWarning("Quest Event: Force For Sale");
                this.gameObject.GetComponent<GenerateItem>().GeneratePresetItem(currStage.ItemToGet, currStage.Part1Mat, currStage.Part2Mat, currStage.Part3Mat, false);
                nextStage();
            }
            else if (currStage.QuestEvent == "Get_Currency")
            {
                Debug.LogWarning("Quest Event: Get Currency");
                this.gameObject.GetComponent<GameMaster>().addCurrency(currStage.CurrencyValue);
                nextStage();
            }
            else if (currStage.QuestEvent == "Remove_Currency")
            {
                Debug.LogWarning("Quest Event: Remove Currency");
                this.gameObject.GetComponent<GameMaster>().removeCurrency(currStage.CurrencyValue);
            }
        }
    }

    public void completeQuest()
    {
        _completeQuestButton.interactable = false;
        if (_chosenQuest.QuestType == "OD_Material")
        {
            //_chosenQuest.ReqiredMaterial.RemoveMat(_chosenQuest.ReqiredCount);
            _invDataRef.getMaterial(_chosenQuest.ReqiredMaterial.Material).RemoveMat(_chosenQuest.ReqiredCount);
        }
        _chosenQuest = null;
        setupText();
        currentItemCount = 0;
        rerollQuest();
        _selectedSheet = null;
    }

    public QuestData CurrentQuest { get => _chosenQuest; }
    public QuestStage CurrentStage { get => _chosenQuest.QuestStages[_currStageIndex]; }
    public int CurrentStageIndex { get => _currStageIndex; set => _currStageIndex = value; }
}
[System.Serializable]
public class SaveQuestsObject
{
    public QuestObject currentQuest;
    public int currentQuestStage;
    public List<QuestObject> completedQuests;
    public List<QuestObject> unlockedQuests;
}
