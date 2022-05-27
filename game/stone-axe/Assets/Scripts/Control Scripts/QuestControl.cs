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
    private InventoryScript _invControlRef;
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
    private TextMeshProUGUI _questText;
    [SerializeField]
    private Button _newQuestButton;
    [SerializeField]
    private Button _completeQuestButton;

    [Header("Quest Organization")]
    [SerializeField] private List<QuestData> _repeatableQuests;
    [SerializeField] private List<QuestData> _unlockedQuests;
    [SerializeField] private List<GameObject> _questStarterGOs;
    [SerializeField] private QuestData _enableBuyOnComplete;
    [SerializeField] private QuestData _enableSellOnComplete;
    [SerializeField] private QuestData _enableDisassembleOnComplete;
    [SerializeField] private QuestData _enableCraftOnComplete;
    [SerializeField] private QuestData _enableAdventurersOnComplete;

    private int reqItemCount = 0;
    private int currentItemCount = 0;
    private bool shut = false;
    private bool star = false;

    private void Awake()
    {
        _repeatableQuests = _questRef.getRepeatableQuests();
        _invDataRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>();
        _invControlRef = _invDataRef.gameObject.GetComponent<InventoryScript>();
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

        if (gameObject.GetComponent<UIControl>().ShopUIActive == true && star == false)
        {
            //Debug.Log("QuestControl.FixedUpdate(): setting up quests for debug");
            setupStoryQuests();
            star = true;
        }
    }

    public void setStar() { star = true; }

    [SerializeField] private QuestData starterRef;
    private GameObject p;
    public void setupStoryQuests()
    {
        //Debug.Log("QuestControl.setupStoryQuests() - setting up story quests");
        if (_unlockedQuests.Count == 0)
            _unlockedQuests.Add(_questRef.getTutorialQuests()[0]);
        if (_chosenQuest == null)
        {
            //Debug.Log("QuestControl.setupStoryQuests() - _chosenQuest is null");

            gameObject.GetComponent<GameMaster>().allTabsAccessable(false);

            foreach (QuestData tutQuest in _questRef.getTutorialQuests())
            {
                //Debug.Log("QuestControl.setupStoryQuests() - tutQuest is " + tutQuest.QuestName);                
                if (tutQuest == _enableBuyOnComplete && _enableBuyOnComplete.StoryQuestComplete)
                {
                    //Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableBuyOnComplete");
                    gameObject.GetComponent<GameMaster>().buyAccessable(true);
                }
                if (tutQuest == _enableDisassembleOnComplete && _enableDisassembleOnComplete.StoryQuestComplete)
                {
                    //Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableDisassembleOnComplete");
                    gameObject.GetComponent<GameMaster>().disassembleAccessable(true);
                }
                if (tutQuest == _enableCraftOnComplete && _enableCraftOnComplete.StoryQuestComplete)
                {
                    //Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableCraftOnComplete");
                    gameObject.GetComponent<GameMaster>().craftAccessable(true);
                }
                if (tutQuest == _enableSellOnComplete && _enableSellOnComplete.StoryQuestComplete)
                {
                    //Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableSellOnComplete");
                    gameObject.GetComponent<GameMaster>().sellAccessable(true);
                }
                if (tutQuest == _enableAdventurersOnComplete && _enableAdventurersOnComplete.StoryQuestComplete)
                {
                    //Debug.Log("QuestControl.setupStoryQuests() - enabling adventurers");
                    gameObject.GetComponent<GameMaster>().toggleAdventurers(true);
                    // ui control, enable to market button
                    gameObject.GetComponent<GameMaster>().marketAccessable(true);
                }
                

                if (starterRef == null)
                {
                    //Debug.Log("QuestControl.setupStoryQuests() - starterRef is null");
                    if (tutQuest.StoryQuestComplete == false && _unlockedQuests.Contains(tutQuest))
                    {
                        //Debug.Log("asigning starter tutorial");
                        starterRef = tutQuest;
                        break;
                    }
                    if (tutQuest.QuestUnlocks.Count > 0 && tutQuest.StoryQuestComplete == true)
                        foreach (QuestData temp in tutQuest.QuestUnlocks)
                            if (_unlockedQuests.Contains(temp) == false)
                            {
                                //Debug.Log(tutQuest.QuestName + " - Adding Unlocked Quest: " + temp.QuestName);
                                _unlockedQuests.Add(temp);
                            }
                }
                //else
                    //break;
            }
            foreach(QuestData storyQuest in _questRef.getStoryQuests())
            {
                if (starterRef == null)
                {
                    //Debug.Log("story: starter ref is not assigned!");
                    if (storyQuest.StoryQuestComplete == false && _unlockedQuests.Contains(storyQuest))
                    {
                        //Debug.Log("asigning starter story");
                        starterRef = storyQuest;
                        break;
                    }
                    if (storyQuest.QuestUnlocks.Count > 0 && storyQuest.StoryQuestComplete == true)
                        foreach (QuestData temp in storyQuest.QuestUnlocks)
                            if (_unlockedQuests.Contains(temp) == false)
                            {
                                //Debug.Log(storyQuest.QuestName + " - Adding Unlocked Quest: " + temp.QuestName);
                                _unlockedQuests.Add(temp);
                            }
                }
                else
                    break;
            }
            setupStarter();
        }
        else if (_chosenQuest.QuestType == "Tutorial" || _chosenQuest.QuestType == "Story")
        {
            //Debug.Log("QuestControl.setupStoryQuests() - _chosenQuest is either Tutorial or Story");
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
        if ((_chosenQuest != null && _chosenQuest.StoryQuestComplete == false)|| (starterRef != null && starterRef.StoryQuestComplete == false))
        {
            p = Instantiate(_questStarterPrefab, _storyQuestPopupParent.transform);
            if (_chosenQuest == null)
                if (starterRef.StoryQuestComplete == false)
                    p.GetComponent<StoryQuestStarter>().QuestRef = starterRef;
            else
            {
                if (_chosenQuest.StoryQuestComplete == false)
                    p.GetComponent<StoryQuestStarter>().QuestRef = _chosenQuest;
            }

            //Debug.Log("QuestControl.setupStarter(): p.QuestRef = " + p.GetComponent<StoryQuestStarter>().QuestRef.QuestName);
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

            //Debug.Log("QuestControl.setupStarter(QuestData qes): p.QuestRef = " + p.GetComponent<StoryQuestStarter>().QuestRef.QuestName);

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
                    //Debug.Log("QuestControl _questStarterGOs contains starter!");
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
        Debug.Log("QuestControl.resetAllQuests() - resetting quests");
        foreach (QuestData tutQuest in _questRef.getTutorialQuests())
            tutQuest.StoryQuestComplete = false;
        foreach (QuestData storyQuest in _questRef.getStoryQuests())
            storyQuest.StoryQuestComplete = false;

        _unlockedQuests.Clear();
        _unlockedQuests.Add(_questRef.getTutorialQuests()[0]);
    }
    public void skipTutorialQuests()
    {
        Debug.Log("QuestControl.skipTutorialQuests() - skipping tutorial quests");
        foreach (QuestData tutQuest in _questRef.getTutorialQuests())
        {
            tutQuest.StoryQuestComplete = true;
            _unlockedQuests.Add(tutQuest);
        }
        foreach (QuestData storyQuest in _questRef.getStoryQuests())
            storyQuest.StoryQuestComplete = false;

        _unlockedQuests.Clear();
        _unlockedQuests.Add(_questRef.getTutorialQuests()[_questRef.getTutorialQuests().Count - 1].QuestUnlocks[0]);
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
                    {
                        tutQuest.StoryQuestComplete = true;
                    }
                    if (tutQuest == _enableBuyOnComplete)
                    {
                        Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableBuyOnComplete");
                    }
                    if (tutQuest == _enableDisassembleOnComplete)
                    {
                        Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableDisassembleOnComplete");
                    }
                    if (tutQuest == _enableCraftOnComplete)
                    {
                        Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableCraftOnComplete");
                    }
                    if (tutQuest == _enableSellOnComplete)
                    {
                        Debug.LogWarning("QuestControl.LoadQuests() TODO code for _enableSellOnComplete");
                    }
                    if (tutQuest == _enableAdventurersOnComplete)
                    {
                        Debug.Log("QuestControl() toggling adventurers - DEBUG");
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
            do
            {
                i = Random.Range(0, _repeatableQuests.Count);
            } while (_repeatableQuests[i].RequiredPlayerLevel > gameObject.GetComponent<GameMaster>().GetLevel);
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
            do
            {
                j = Random.Range(0, _repeatableQuests.Count);
            } while (_repeatableQuests[j].RequiredPlayerLevel > gameObject.GetComponent<GameMaster>().GetLevel);
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
            do
            {
                k = Random.Range(0, _repeatableQuests.Count);
            } while (_repeatableQuests[k].RequiredPlayerLevel > gameObject.GetComponent<GameMaster>().GetLevel);
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
            else if (_chosenQuest.QuestType == "Story")
            {
                _questText.text += "\n\nCurrent step: ";
                Debug.Log("QuestControl.setupText() CurrentStage == " + CurrentStage.StageType.ToString());
                if (CurrentStage.StageType.ToString() == "Dialogue")
                    _questText.text += "Chat with " + CurrentStage.DialogueSpeaker;
                else if (CurrentStage.StageType.ToString() == "Craft_Item")
                    _questText.text += "Craft " + CurrentStage.ItemToGet.ItemName;
                else if (CurrentStage.StageType.ToString() == "Sell_Item")
                    _questText.text += "Sell " + CurrentStage.ItemToGet.ItemName;
                else if (CurrentStage.StageType.ToString() == "Buy_Item")
                    _questText.text += "Buy " + CurrentStage.ItemToGet.ItemName;
                else if (CurrentStage.StageType.ToString() == "Disassemble_Item")
                    _questText.text += "Disassemble Item";
                else if (CurrentStage.StageType.ToString() == "Have_Currency")
                    _questText.text += "Have Currency value of " + CurrentStage.CurrencyValue; 
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

    private void distributeQuestRewards()
    {
        if (_chosenQuest.RewardedCurrency > 0)
            gameObject.GetComponent<GameMaster>().addCurrency(_chosenQuest.RewardedCurrency);
        if (_chosenQuest.RewardedEXP > 0)
            gameObject.GetComponent<ExperienceManager>().addExperience(_chosenQuest.RewardedEXP);
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
            Debug.LogError("This quest starts with a non-Dialogue stage!\nNote to Dev: Implement this quest's start in code!\nLine: 485 Method: startStoryQuest(QuestData questInput)");
    }
    public void nextStage()
    {
        _currStageIndex++;
        if (_currStageIndex < CurrentQuest.QuestStages.Count)
        {
            gameObject.GetComponent<DialogueControl>().setupDialogueLine();
            updateQuestProgress(_chosenQuest, _chosenQuest.QuestStages[_currStageIndex]);
        }
        else if (_currStageIndex >= CurrentQuest.QuestStages.Count)
        {
            gameObject.GetComponent<DialogueControl>().dialogeQuestEnd();
            updateQuestProgress(_chosenQuest, true);
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
                        //Debug.Log("TODO: quest can be completed");
                        _completeQuestButton.interactable = true; 
                    else 
                        _completeQuestButton.interactable = false; 
                }
            }
            else if (_chosenQuest.QuestType == "Tutorial" || _chosenQuest.QuestType == "Story")
            {
                //Debug.LogWarning("PH");
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

            //if (isComplete)
        }
        //Debug.Log("QuestControl().updateQuestProgress(quest, isComplete) quest: " + quest.QuestName + " setting up story quests"); 
        setupStoryQuests();
    }
    public void updateQuestProgress(QuestData quest, QuestStage currStage)
    {
        if (currStage.StageType == "Buy_Item")
        {
            Debug.Log("Quest Stage: Buy item!");
            // code should work
            if (quest.QuestType == "Tutorial")
            {
                //gameObject.GetComponent<UIControl>().shopBuyAccessableOnly();
            }
        }
        else if (currStage.StageType == "Sell_Item")
        {
            Debug.Log("Quest Stage: Sell item!");
            if (quest.QuestType == "Tutorial")
            {
                //gameObject.GetComponent<UIControl>().shopSellAccessableOnly();
            }
        }
        else if (currStage.StageType == "Disassemble_Item")
        {
            Debug.Log("Quest Stage: Disassemble item!");
            // code should work
            if (quest.QuestType == "Tutorial")
            {
                //gameObject.GetComponent<UIControl>().shopDisassembleAccessableOnly();
            }
        }
        else if (currStage.StageType == "Craft_Item")
        {
            Debug.Log("Quest Stage: Craft item!");
            if (quest.QuestType == "Tutorial")
            {
                //gameObject.GetComponent<UIControl>().shopCraftAccessableOnly();
            }
        }
        else if (currStage.StageType == "Have_Currency")
        {
            Debug.Log("Quest Stage: Have currency!");
        }
        else if (currStage.StageType == "Force_Event")
        {
            Debug.Log("Quest Stage: Force Event!");
            if (currStage.QuestEvent == "Summon_Adventurer")
            {
                Debug.Log("Quest Event: Summon Adventurer");
                this.gameObject.GetComponent<AdventurerMaster>().spawnAdventurer();
            }
            else if (currStage.QuestEvent == "Summon_NPC")
            {
                Debug.Log("Quest Event: Summon Story NPC");
                if (currStage.NPCRef != null) this.gameObject.GetComponent<NPC_Master>().spawnNPC(currStage.NPCRef);
                else Debug.LogError("NPC Ref for " + quest.QuestName + " Stage: " + currStage.name + " is not asigned!");
            }
            else if (currStage.QuestEvent == "Dismiss_Quest_NPC")
            {
                Debug.Log("Quest Event: Dismiss Story NPCs");
                this.gameObject.GetComponent<NPC_Master>().dismissNPCs();
            }
            else if (currStage.QuestEvent == "Get_Item")
            {
                Debug.Log("Quest Event: Get Item");
                this.gameObject.GetComponent<GenerateItem>().GeneratePresetItem(currStage.ItemToGet, currStage.Part1Mat, currStage.Part2Mat, currStage.Part3Mat, true);
                nextStage();
            }
            else if (currStage.QuestEvent == "Force_For_Sale")
            {
                Debug.Log("Quest Event: Force For Sale");
                gameObject.GetComponent<GenerateItem>().GeneratePresetItem(currStage.ItemToGet, currStage.Part1Mat, currStage.Part2Mat, currStage.Part3Mat, false);
                nextStage();
            }
            else if (currStage.QuestEvent == "Get_Currency")
            {
                Debug.Log("Quest Event: Get Currency");
                this.gameObject.GetComponent<GameMaster>().addCurrency(currStage.CurrencyValue);
                nextStage();
            }
            else if (currStage.QuestEvent == "Remove_Currency")
            {
                Debug.Log("Quest Event: Remove Currency");
                this.gameObject.GetComponent<GameMaster>().removeCurrency(currStage.CurrencyValue);
            }
            else if (currStage.QuestEvent == "Force_Open_UI")
            {
                //Debug.LogWarning("This Quest Event has not been fully implemented");
                if (currStage.ForcedUI == "Buy")
                    if (quest.QuestType == "Tutorial")
                        gameObject.GetComponent<UIControl>().shopBuyAccessableOnly();
                if (currStage.ForcedUI == "Sell")
                    if (quest.QuestType == "Tutorial")
                        gameObject.GetComponent<UIControl>().shopSellAccessableOnly();
                if (currStage.ForcedUI == "Disassemble")
                    if (quest.QuestType == "Tutorial")
                        gameObject.GetComponent<UIControl>().shopDisassembleAccessableOnly();
                if (currStage.ForcedUI == "Craft")
                    if (quest.QuestType == "Tutorial")
                        gameObject.GetComponent<UIControl>().shopCraftAccessableOnly();
            }
        }
        else if (currStage.StageType == "Have_UI_Open")
        {
            Debug.Log("Quest Stage: Have UI Open!");
            //Debug.LogWarning("This StageType has not been fully implemented");
            if (currStage.RequiredUI == "MiniGame_UI")
                Debug.Log("Waiting for MiniGame_UI UI to be open");
        }
    }

    public void completeQuest()
    {
        _completeQuestButton.interactable = false;
        if (_chosenQuest.QuestType == "OD_Material")
        {
            _invControlRef.RemoveMatAmount(_invDataRef.getMaterial(_chosenQuest.ReqiredMaterial.Material), _chosenQuest.ReqiredCount);
        }
        else if (_chosenQuest.QuestType == "OCC_Item" || _chosenQuest.QuestType == "OCC_TotalCrafted" || _chosenQuest.QuestType == "OCC_QuestItem")
        {
            _invControlRef.RemoveQuestItems();
        }
        distributeQuestRewards();

        _chosenQuest = null;
        setupText();
        currentItemCount = 0;
        rerollQuest();
        _selectedSheet = null;
    }

    public bool BuyQuestComplete { get => _enableBuyOnComplete.StoryQuestComplete; }
    public bool DisassembleQuestComplete { get => _enableDisassembleOnComplete.StoryQuestComplete; }
    public bool CraftQuestComplete { get => _enableCraftOnComplete.StoryQuestComplete; }
    public bool SellQuestComplte { get => _enableSellOnComplete.StoryQuestComplete; }

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
