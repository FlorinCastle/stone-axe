using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.AddressableAssets.Build.BuildPipelineTasks.GenerateLocationListsTask;
using static UnityEditor.Progress;

public class RecipeBook : MonoBehaviour
{
    //[SerializeField] private List<ItemData> itemRecipes;
    [SerializeField] private List<TextAsset> itemJsonRecipes;
    [SerializeField] private List<ItemJsonData> itemJsonData;
    //[SerializeField] private List<PartData> partRecipes;
    [SerializeField] private List<TextAsset> partJsonRecipes;
    [SerializeField] private List<PartJsonData> partJsonData;
    [SerializeField] private List<QuestItemData> questItemRecipes;
    [SerializeField, HideInInspector] private List<string> itemRecipeName;
    [SerializeField, HideInInspector] private List<string> partRecipeName;
    [SerializeField, HideInInspector] private List<GameObject> recipeButtons;
    [SerializeField] private List<GameObject> upcomingRecipeButtons;
    //[SerializeField, HideInInspector] private ItemData _selectedItemRecipe;
    [SerializeField, HideInInspector] private ItemJsonData _selectedJsonItemRecipe;
    [SerializeField, HideInInspector] private QuestItemData _selectedQuestItemRecipe;
    //[SerializeField, HideInInspector] private PartData _selectedPartRecipe;
    [SerializeField, HideInInspector] private PartJsonData _selectedJsonPartRecipe;
    [SerializeField] private TextAsset _craftQuestJson;
    //[SerializeField] private QuestData _craftQuest;
    [Header("Filters")]
    [SerializeField] private List<FilterData> filterData;
    [SerializeField, HideInInspector] private List<GameObject> filterButtons;
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _recipeText;
    [SerializeField] private Button _recipeSelectButton;
    [SerializeField] private GameObject _contentRef;
    [SerializeField] private GameObject _filterUI;
    [SerializeField] private GameObject _filterParent;
    [SerializeField] private GameObject _upcomingRecipesButton;
    [SerializeField] private GameObject _upRecButtonCheck;

    [Header("Prefabs")]
    [SerializeField] private GameObject _itemRecipeInfoPrefab;
    [SerializeField] private GameObject _questItemRecipeInfoPrefab;
    [SerializeField] private GameObject _partRecipeInfoPrefab;
    [SerializeField] private GameObject _upcomingItemRecipePrefab;
    [SerializeField] private GameObject _upcomingPartRecipePrefab;
    [SerializeField] private GameObject _filterPrefab;

    private GameMaster _gameMasterRef;
    private Quest questRef;
    private QuestControl questControl;

    [SerializeField, HideInInspector] private List<TextAsset> processedJsons;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        questRef = GameObject.FindGameObjectWithTag("QuestMaster").GetComponent<Quest>();
        questControl = _gameMasterRef.gameObject.GetComponent<QuestControl>();

        processRecipes();

        _recipeSelectButton.interactable = false; // temp till I get all the code set up
        recipeButtons = new List<GameObject>();
        setupRecipeGrid();
        _recipeText.text = "";
        setupFilterUI();
        _filterUI.SetActive(false);
    }

    private void processRecipes()
    {
        Debug.Log("RecipeBook.processRecipes() processing recipes");
        foreach(TextAsset item in itemJsonRecipes)
            if (processedJsons.Contains(item) == false)
            {
                ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(item).Replace("Assets", "")));
                itemJsonData.Add(itemJson);
                processedJsons.Add(item);
            }
        foreach (TextAsset part in partJsonRecipes)
            if (processedJsons.Contains(part) == false)
            {
                PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(part).Replace("Assets", "")));
                partJsonData.Add(partJson);
                processedJsons.Add(part);
            }
    }

    public void disableRecipeSelectButton()
    {
        _recipeSelectButton.interactable = false;
        _filterUI.SetActive(false);
    }
    public void prepToSelectRecipe()
    {
        _recipeSelectButton.interactable = true;
    }
    public void selectRecipe()
    {
        if (anyRecipeSelected() && (GameObject.FindGameObjectWithTag("QuestMaster").GetComponent<Quest>().isLongQuestComplete(_craftQuestJson) || GameObject.FindGameObjectWithTag("GameMaster").GetComponent<QuestControl>().CurrentQuest == _craftQuestJson))
        {
            CraftControl CCRef = GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>();

            CCRef.setChosenRecipe();
        }
    }
    /*GameObject.FindGameObjectWithTag("GameMaster").GetComponent<QuestControl>().CraftQuestComplete || */

    /*public void setItemRecipeInfo(int index)
    {
        // get gameobject from recipeButton list at input index
        if (index == -1)
            _recipeText.text = "placeholder";
        else
        {
            GameObject button = recipeButtons[index];
            //Debug.LogError("REPLACE ITEMDATA WITH ITEMJSON/TEXTFILE");
            foreach (ItemData itemRecipe in itemRecipes)
            {
                if (itemRecipe.ItemName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _selectedItemRecipe = itemRecipe;
                    _selectedQuestItemRecipe = null;
                    _selectedPartRecipe = null;

                    _recipeText.text = itemRecipe.ItemName + "\nParts:\nValid Part 1: ";
                    foreach (PartData valid1 in itemRecipe.ValidParts1) 
                        _recipeText.text += valid1.PartName + " ";

                    _recipeText.text += "\nValid Part 2: ";
                    foreach (PartData valid2 in itemRecipe.ValidParts2)
                        _recipeText.text += valid2.PartName + " ";

                    _recipeText.text += "\nValid Part 3: ";
                    foreach (PartData valid3 in itemRecipe.ValidParts3)
                        _recipeText.text += valid3.PartName + " ";
                }
            }
        }
    } */
    public void setItemRecipeInfo(TextAsset jsonText)
    {
        if (jsonText != null)
        {
            _selectedJsonItemRecipe = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonText).Replace("Assets", "")));
            _selectedJsonPartRecipe = null;

            //Debug.Log(_selectedJsonItemRecipe.itemName);
            _recipeText.text = _selectedJsonItemRecipe.itemName + " Json\nParts:";
            foreach (string partName in _selectedJsonItemRecipe.requiredParts)
                _recipeText.text += "\nValid Json Part " + (_selectedJsonItemRecipe.requiredParts.IndexOf(partName) + 1) + ": " + partName;
        }
        else
            Debug.LogError("RecipeBook.setItemRecipeInfo(TextAsset jsonText): jsonText is Null!");
    }
    public void setQuestItemRecipeInfo(int index)
    {
        //Debug.LogWarning("settting up quest recipe Info");
        Debug.LogError("RecipeBook.setQuestItemRecipeInfo(int index): set this up to use json");
        if (index == -1)
            _recipeText.text = "placeholder";
        else
        {
            GameObject button = recipeButtons[index];
            foreach (QuestItemData questRecipe in questItemRecipes)
            {
                if (questRecipe.QuestItemName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    //_selectedItemRecipe = null;
                    _selectedQuestItemRecipe = questRecipe;
                    //_selectedPartRecipe = null;

                    _recipeText.text = questRecipe.QuestItemName + "\nParts:\nValid Part 1: ";
                    foreach (string itemName in itemRecipeName)
                        if (questRecipe.ItemPart1.name == itemName)
                            _recipeText.text += itemName + " ";
                    foreach (string partName in partRecipeName)
                        if (questRecipe.ItemPart1.name == partName)
                            _recipeText.text += partName + " ";

                    _recipeText.text += "\nValid Part 2: ";
                    foreach (string itemName in itemRecipeName)
                        if (questRecipe.ItemPart2.name == itemName)
                            _recipeText.text += itemName + " ";
                    foreach (string partName in partRecipeName)
                        if (questRecipe.ItemPart2.name == partName)
                            _recipeText.text += partName + " ";

                    _recipeText.text += "\nValid Part 3: ";
                    foreach (string itemName in itemRecipeName)
                        if (questRecipe.ItemPart3.name == itemName)
                            _recipeText.text += itemName + " ";
                    foreach (string partName in partRecipeName)
                        if (questRecipe.ItemPart3.name == partName)
                            _recipeText.text += partName + " ";
                }
            }
        }
    }
    /*public void setPartRecipeInfo(int index)
    {
        if (index == -1)
            _recipeText.text = "placeholder";
        else
        {
            GameObject button = recipeButtons[index];
            foreach (PartData partRecipe in partRecipes)
            {
                if (partRecipe.PartName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _selectedItemRecipe = null;
                    _selectedQuestItemRecipe = null;
                    _selectedPartRecipe = partRecipe;

                    _recipeText.text = partRecipe.PartName + "\nValid Material Types:\n";
                    foreach (string matName in partRecipe.ValidMaterials)
                        _recipeText.text += matName + "\n";
                }
            }
        }
    } */
    public void setPartRecipeInfo(TextAsset jsonText)
    {
        if (jsonText != null)
        {
            _selectedJsonItemRecipe = null;
            _selectedJsonPartRecipe = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonText).Replace("Assets", "")));
            _recipeText.text = _selectedJsonPartRecipe.partName + " Json\nValid Material Types:\n";
            foreach (string matType in _selectedJsonPartRecipe.validMaterialTypes)
                _recipeText.text += matType + "\n";
        }
        else
            Debug.LogError("RecipeBook.setItemRecipeInfo(TextAsset jsonText): jsonText is Null!");
    }

    /*public void setUpcomingRecipeInfo (int index)
    {
        if (index == -1)
            _recipeText.text = "placeholder";
        else
        {
            GameObject button = upcomingRecipeButtons[index];
            //Debug.LogError("REPLACE ITEMDATA WITH ITEMJSON/TEXTFILE");
            foreach (ItemJsonData itemRecipe in itemJsonData)
            {
                if (itemRecipe.itemName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _recipeText.text = itemRecipe.itemName + "\nParts:";
                    for(int i = 1; i < itemRecipe.requiredParts.Count; i++)
                    {
                        _recipeText.text += "\nValid Part " + i + ": " + itemRecipe.requiredParts[i-1];
                    }
                    /*foreach (PartData valid1 in itemRecipe.requiredParts[0])
                        _recipeText.text += valid1.PartName + " ";

                    _recipeText.text += "\nValid Part 2: ";
                    foreach (PartData valid2 in itemRecipe.ValidParts2)
                        _recipeText.text += valid2.PartName + " ";

                    _recipeText.text += "\nValid Part 3: ";
                    foreach (PartData valid3 in itemRecipe.ValidParts3)
                        _recipeText.text += valid3.PartName + " "; 
                }
            }

            /*foreach (ItemData itemRecipe in itemRecipes)
            {
                if (itemRecipe.ItemName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _recipeText.text = itemRecipe.ItemName + "\nParts:\nValid Part 1: ";
                    foreach (PartData valid1 in itemRecipe.ValidParts1)
                        _recipeText.text += valid1.PartName + " ";

                    _recipeText.text += "\nValid Part 2: ";
                    foreach (PartData valid2 in itemRecipe.ValidParts2)
                        _recipeText.text += valid2.PartName + " ";

                    _recipeText.text += "\nValid Part 3: ";
                    foreach (PartData valid3 in itemRecipe.ValidParts3)
                        _recipeText.text += valid3.PartName + " ";
                }
            }
            foreach (PartData partRecipe in partRecipes)
            {
                if (partRecipe.PartName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _recipeText.text = partRecipe.PartName + "\nValid Material Types:\n";
                    foreach (string matName in partRecipe.ValidMaterials)
                        _recipeText.text += matName + "\n";
                }
            }
        }
    } */
    public void setUpcomingRecipeInfo(TextAsset jsonText)
    {
        if (jsonText != null)
        {
            if (itemJsonRecipes.Contains(jsonText))
            {
                ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonText).Replace("Assets", "")));
                _recipeText.text = itemJson.itemName + " Json\nParts:";
                foreach (string partName in itemJson.requiredParts)
                    _recipeText.text += "\nValid Json Part " + (itemJson.requiredParts.IndexOf(partName) + 1) + ": " + partName;
            }
            else if (partJsonRecipes.Contains(jsonText))
            {
                PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonText).Replace("Assets", "")));
                _recipeText.text = partJson.partName + " Json\nValid Material Types:\n";
                foreach (string matType in partJson.validMaterialTypes)
                    _recipeText.text += matType + "\n";
            }
            else
                Debug.LogError("RecipeBook.setUpcomingRecipeInfo(TextAsset jsonText): itemJsonRecipes nor partJsonRecipes contain item called " + jsonText.name + "!!");
        }
        else
            Debug.LogError("RecipeBook.setItemRecipeInfo(TextAsset jsonText): jsonText is Null!");
    }
    public void clearSelectedRecipe()
    {
        //_selectedItemRecipe = null;
        _selectedJsonItemRecipe = null;
        //_selectedPartRecipe = null;
        _selectedJsonPartRecipe = null;
        _recipeText.text = "";
    }

    private GameObject tempButton;

    // should be setup for level locking
    public void setupQuestRecipeGrid()
    {
        //QuestData currQuest = _gameMasterRef.GetComponent<QuestControl>().CurrentQuest;
        BaseQuestJsonData currQuest = questRef.LoadQuestData(questControl.CurrentQuest);
        if (currQuest != null)
        {
            if (currQuest.questType == "Tutorial" || currQuest.questType == "Story")
            {
                Debug.LogWarning("RecipeBook: quest type - Tutorail or Story");
                setupStoryQuestRecipeGrid();
            }
            else if (currQuest.questType == "OCC_QuestItem")
            {
                Debug.LogWarning("RecipeBook: quest type - OCC_QuestItem");
                setupSpecialQuestRecipeGrid();
            }
        }
    }

    //[SerializeField, HideInInspector] private List<ItemData> levelLockedItems;
    [SerializeField, HideInInspector] private List<TextAsset> levelLockedJsonItems;
    //[SerializeField, HideInInspector] private List<PartData> levelLockedParts;
    [SerializeField, HideInInspector] private List<TextAsset> levelLockedJsonParts;

    // setup for level locking
    public void setupRecipeGrid()
    {
        clearRecipeGrid();
        clearUpcomingRecipesLists();
        int r = 0;
        /*foreach (ItemData itemRecipe in itemRecipes)
        {
            if (itemRecipe != null && (itemRecipe.ItemLevel <= _gameMasterRef.GetLevel))
            {
                // instantiate the button prefab
                tempButton = Instantiate(_itemRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;
                // set up button text
                TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = itemRecipe.ItemName + " Recipe";
                tempButton.name = itemRecipe.ItemName + " Recipe";
                // add button to list
                InsertButton(tempButton);
            }
            else
                levelLockedItems.Add(itemRecipe);
            r++;
        } */
        foreach (TextAsset itemJsonFile in itemJsonRecipes)
        {
            if (itemJsonFile != null)
            {
                ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(itemJsonFile).Replace("Assets", "")));
                if (itemJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(itemJson, itemJsonFile);
                    /*// instantiate the button prefab
                    tempButton = Instantiate(_itemRecipeInfoPrefab);
                    tempButton.transform.SetParent(_contentRef.transform, false);
                    tempButton.GetComponent<RecipeButton>().setRecipeName(itemJson.itemName);
                    tempButton.GetComponent<RecipeButton>().CanCraft = true;
                    // set up button text
                    TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                    t.text = itemJson.itemName + " Recipe";
                    tempButton.name = itemJson.itemName + " Recipe";
                    // add button to list
                    InsertButton(tempButton); */
                }
                else
                    levelLockedJsonItems.Add(itemJsonFile); //levelLockedJsonItems.Add(itemJson);
            }
            r++;
        }
        /*foreach (PartData partRecipe in partRecipes)
        {
            if (partRecipe != null && (partRecipe.PartLevelReq <= _gameMasterRef.GetLevel))
            {
                tempButton = Instantiate(_partRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;

                TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = partRecipe.PartName + " Recipe";
                tempButton.name = partRecipe.PartName + " Recipe";
                // add button to list
                InsertButton(tempButton);
            }
            else
                levelLockedParts.Add(partRecipe);
            r++;
        } */
        foreach (TextAsset partJsonFile in partJsonRecipes)
        {
            if (partJsonFile != null)
            {
                PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(partJsonFile).Replace("Assets", "")));
                if (partJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(partJson, partJsonFile);
                }
                else
                    levelLockedJsonParts.Add(partJsonFile);
            }
            r++;
        }
    }
    // setup for level locking
    public void setupFilteredGrid()
    {
        clearRecipeGrid();
        clearUpcomingRecipesLists();
        foreach (TextAsset itemJsonFile in itemJsonRecipes)
        {
            if (itemJsonFile != null)
            {
                ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(itemJsonFile).Replace("Assets", "")));
                if (checkIfEnabledFiltersValid(itemJson) && enabledFilters.Count != 0 && itemJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(itemJson, itemJsonFile);
                }
                else
                    levelLockedJsonItems.Add(itemJsonFile); //levelLockedJsonItems.Add(itemJson);
            }
        }
        foreach (TextAsset partJsonFile in partJsonRecipes)
        {
            if (partJsonFile != null)
            {
                PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(partJsonFile).Replace("Assets", "")));
                if (partJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(partJson, partJsonFile);
                }
                else
                    levelLockedJsonParts.Add(partJsonFile);
            }
        }
        if (recipeButtons.Count == 0 && enabledFilters.Count == 0)
        {
            setupRecipeGrid();
        }
    }
        //int r = 0;
        //Debug.LogError("REPLACE ITEMDATA WITH ITEMJSON/TEXTFILE");
        /*foreach(ItemData itemRecipe in itemRecipes)
        {
            if (checkIfEnabledFiltersValid(itemRecipe) && enabledFilters.Count != 0 && (itemRecipe.ItemLevel <= _gameMasterRef.GetLevel))
            {
                // instantiate the button prefab
                tempButton = Instantiate(_itemRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;
                // set up button text
                TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = itemRecipe.ItemName + " Recipe";
                tempButton.name = itemRecipe.ItemName + " Recipe";

                //Debug.Log(itemRecipe.ItemName);
                // add button to list
                InsertButton(tempButton);
                //break;
            }
        }*/
        /*foreach (PartData partRecipe in partRecipes)
        {
            if (checkIfEnabledFiltersValid(partRecipe) && enabledFilters.Count != 0 && (partRecipe.PartLevelReq <= _gameMasterRef.GetLevel))
            {
                tempButton = Instantiate(_partRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;

                TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = partRecipe.PartName + " Recipe";
                tempButton.name = partRecipe.PartName + " Recipe";

                //Debug.Log(partRecipe.PartName);
                // add button to list
                InsertButton(tempButton);
                //break;
            }
        }*/

    // should be setup for level locking
    public void setupSpecialQuestRecipeGrid()
    {
        //QuestData currQuest = _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest;
        CraftQuestItemQuest currQuest = questRef.LoadCraftQuestItemQuest(questControl.CurrentQuest);

        QuestItemData questItem = getQuestItemRecipe(currQuest.requiredQuestItem);

        clearRecipeGrid();
        clearUpcomingRecipesLists();
        int r = 0;
        
        if (questItem != null)
        {
            // instantiate the button prefab
            tempButton = Instantiate(_questItemRecipeInfoPrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(questItem.QuestItemName);
            // set up button text
            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = questItem.QuestItemName + " Recipe";
            tempButton.name = questItem.QuestItemName + " Recipe";
            // add button to list
            InsertButton(tempButton);
        }
        r++;
        //Debug.LogError("REPLACE ITEMDATA WITH ITEMJSON/TEXTFILE");
        /*foreach (ItemData itemRecipe in itemRecipes)
        {
            if (itemRecipe != null && (itemRecipe.ItemLevel <= _gameMasterRef.GetLevel))
            {
                // instantiate the button prefab
                tempButton = Instantiate(_itemRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;
                // set up button text
                TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = itemRecipe.ItemName + " Recipe";
                tempButton.name = itemRecipe.ItemName + " Recipe";
                // add button to list
                InsertButton(tempButton);
            }
            else
                levelLockedItems.Add(itemRecipe);
            r++;
        }*/
        foreach (TextAsset itemJsonFile in itemJsonRecipes)
        {
            if (itemJsonFile != null)
            {
                ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(itemJsonFile).Replace("Assets", "")));
                if (itemJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(itemJson, itemJsonFile);
                }
                else
                    levelLockedJsonItems.Add(itemJsonFile); //levelLockedJsonItems.Add(itemJson);
            }
            r++;
        }
        /*foreach (PartData partRecipe in partRecipes)
        {
            if (partRecipe != null && (partRecipe.PartLevelReq <= _gameMasterRef.GetLevel))
            {
                tempButton = Instantiate(_partRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;

                TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
                t.text = partRecipe.PartName + " Recipe";
                tempButton.name = partRecipe.PartName + " Recipe";
                // add button to list
                InsertButton(tempButton);
            }
            else
                levelLockedParts.Add(partRecipe);
            r++;
        }*/
        foreach (TextAsset partJsonFile in partJsonRecipes)
        {
            if (partJsonFile != null)
            {
                PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(partJsonFile).Replace("Assets", "")));
                if (partJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(partJson, partJsonFile);
                }
                else
                    levelLockedJsonParts.Add(partJsonFile);
            }
            r++;
        }

    }
    // shold be setup for level locking
    public void setupStoryQuestRecipeGrid()
    {
        Debug.LogError("TODO Fix this");

        //QuestData currQuest = _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest;
        //ItemData reqItem = null; //currQuest.RequiredItem;

        clearRecipeGrid();
        clearUpcomingRecipesLists();
        int r = 0;

        /*if (reqItem != null)
        {
            // instantiate the button prefab
            tempButton = Instantiate(_itemRecipeInfoPrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(reqItem.ItemName);
            // set up button text
            Text t = tempButton.GetComponentInChildren<Text>();
            t.text = reqItem.ItemName + " Recipe";
            tempButton.name = reqItem.ItemName + " Recipe";
            // add button to list
            InsertButton(tempButton);
        }*/
        r++;
        Debug.LogError("REPLACE ITEMDATA WITH ITEMJSON/TEXTFILE");
        /*foreach (ItemData itemRecipe in itemRecipes)
        {
            if (itemRecipe != null && itemRecipe != reqItem && (itemRecipe.ItemLevel <= _gameMasterRef.GetLevel))
            {
                // instantiate the button prefab
                tempButton = Instantiate(_itemRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;
                // set up button text
                Text t = tempButton.GetComponentInChildren<Text>();
                t.text = itemRecipe.ItemName + " Recipe";
                tempButton.name = itemRecipe.ItemName + " Recipe";
                // add button to list
                InsertButton(tempButton);
            }
            r++;
        }*/
        foreach (TextAsset itemJsonFile in itemJsonRecipes)
        {
            if (itemJsonFile != null)
            {
                ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(itemJsonFile).Replace("Assets", "")));
                if (itemJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(itemJson, itemJsonFile);
                }
                else
                    levelLockedJsonItems.Add(itemJsonFile); //levelLockedJsonItems.Add(itemJson);
            }
            r++;
        }
        /*foreach (PartData partRecipe in partRecipes)
        {
            if (partRecipe != null && (partRecipe.PartLevelReq <= _gameMasterRef.GetLevel))
            {
                tempButton = Instantiate(_partRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);
                tempButton.GetComponent<RecipeButton>().CanCraft = true;

                Text t = tempButton.GetComponentInChildren<Text>();
                t.text = partRecipe.PartName + " Recipe";
                tempButton.name = partRecipe.PartName + " Recipe";
                // add button to list
                InsertButton(tempButton);
            }
            r++;
        }*/
        foreach (TextAsset partJsonFile in partJsonRecipes)
        {
            if (partJsonFile != null)
            {
                PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(partJsonFile).Replace("Assets", "")));
                if (partJson.levelRequirement <= _gameMasterRef.GetLevel)
                {
                    setupButtonFromJson(partJson, partJsonFile);
                }
                else
                    levelLockedJsonParts.Add(partJsonFile);
            }
            r++;
        }
    }
    //
    public void setupSearchedRecipeGrid(string input)
    {
        //if (recipeGridCoroutine != null)
        //    StopCoroutine(recipeGridCoroutine);
        Debug.Log(input);
        clearRecipeGrid();
        clearUpcomingRecipesLists();

        //recipeGridCoroutine = StartCoroutine(setupSearchedRecipeGridElements(input));
    }

    /*private Coroutine recipeGridCoroutine;
    private IEnumerator setupSearchedRecipeGridElements(string input)
    {
        foreach (ItemJsonData itemJsonFile in itemJsonData)
        {
            if (itemJsonFile != null)
                if (itemJsonFile.itemName.Contains(input))
                {
                    //Debug.Log(itemJsonFile.itemName);
                    setupButtonFromJson(itemJsonFile);
                }
        }
        foreach (PartJsonData partJsonFile in partJsonData)
        {
            if (partJsonFile != null)
                if (partJsonFile.partName.Contains(input))
                {
                    //Debug.Log(partJsonFile.partName);
                    setupButtonFromJson(partJsonFile);
                }
        }
        yield return null;
    }*/

    private async void setupSearchedRecipeGridElements(string input)
    {

    }


    private void setupUpcomingRecipes()
    {
        /*foreach(ItemData itemRecipe in levelLockedItems)
        {
            tempButton = Instantiate(_upcomingItemRecipePrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
            tempButton.GetComponent<RecipeButton>().CanCraft = false;

            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = itemRecipe.ItemName + " Recipe";
            tempButton.name = itemRecipe.ItemName + " Recipe";
            InsertUpcomingRecipeButton(tempButton);
        }*/
        foreach(TextAsset itemJsonFile in levelLockedJsonItems)
        {
            // instantiate the button prefab
            ItemJsonData itemJson = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(itemJsonFile).Replace("Assets", "")));
            tempButton = Instantiate(_upcomingItemRecipePrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(itemJson.itemName);
            tempButton.GetComponent<RecipeButton>().RecipeJson = itemJsonFile;
            tempButton.GetComponent<RecipeButton>().CanCraft = false;
            // set up button text
            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = itemJson.itemName + " Json Recipe";
            tempButton.name = itemJson.itemName + " Recipe";
            // add button to list
            InsertUpcomingRecipeButton(tempButton);
        }
        /*foreach (PartData partRecipe in levelLockedParts)
        {
            tempButton = Instantiate(_upcomingPartRecipePrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);
            tempButton.GetComponent<RecipeButton>().CanCraft = false;

            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = partRecipe.PartName + " Recipe";
            tempButton.name = partRecipe.PartName + " Recipe";
            InsertUpcomingRecipeButton(tempButton);
        }*/
        foreach(TextAsset partJsonFile in levelLockedJsonParts)
        {
            // instantiate the button prefab
            PartJsonData partJson = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(partJsonFile).Replace("Assets", "")));
            tempButton = Instantiate(_upcomingItemRecipePrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(partJson.partName);
            tempButton.GetComponent<RecipeButton>().RecipeJson = partJsonFile;
            tempButton.GetComponent<RecipeButton>().CanCraft = false;
            // set up button text
            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = partJson.partName + " Json Recipe";
            tempButton.name = partJson.partName + " Recipe";
            // add button to list
            InsertUpcomingRecipeButton(tempButton);

        }
    }
    private bool upcomingRecipesBool = false;
    public void toggleUpcomingRecipes()
    {
        Debug.LogWarning("swap this to json");
        clearUpcomingRecipesLists();
        upcomingRecipesBool = !upcomingRecipesBool;

        _upRecButtonCheck.SetActive(upcomingRecipesBool);

        if (upcomingRecipesBool == false)
        {
            if (enabledFilters.Count > 0)
                setupFilteredGrid();
            else
                setupRecipeGrid();
        }
        else if (upcomingRecipesBool == true)
        {
            //Debug.Log("placeholder");
            if (enabledFilters.Count > 0)
                setupFilteredGrid();
            else
                setupRecipeGrid();
            setupUpcomingRecipes();
        }
            
    }

    private void clearRecipeGrid()
    {
        foreach (GameObject go in recipeButtons)
            Destroy(go);
        foreach (GameObject fgo in upcomingRecipeButtons)
            Destroy(fgo);
        recipeButtons.Clear();
        upcomingRecipeButtons.Clear();
    }
    private void clearUpcomingRecipesLists()
    {
        /*if (levelLockedItems.Count > 0)
            levelLockedItems.Clear();
        if (levelLockedParts.Count > 0)
            levelLockedParts.Clear(); */

        if (levelLockedJsonItems.Count > 0)
            levelLockedJsonItems.Clear();
        if (levelLockedJsonParts.Count > 0)
            levelLockedJsonParts.Clear();
    }

    private int InsertButton(GameObject button)
    {
        recipeButtons.Add(button);
        int i = recipeButtons.IndexOf(button);
        button.GetComponent<RecipeButton>().setMyIndex(i);
        return i;
    }

    private void setupButtonFromJson(ItemJsonData itemJson)
    {
        foreach(TextAsset jsonFile in itemJsonRecipes)
            if (JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonFile).Replace("Assets", ""))).itemName == itemJson.itemName)
            {
                setupButtonFromJson(itemJson, jsonFile);
                break;
            }

    }
    private void setupButtonFromJson(ItemJsonData itemJson, TextAsset jsonFile)
    {
        // instantiate the button prefab
        tempButton = Instantiate(_itemRecipeInfoPrefab);
        tempButton.transform.SetParent(_contentRef.transform, false);
        tempButton.GetComponent<RecipeButton>().setRecipeName(itemJson.itemName);
        tempButton.GetComponent<RecipeButton>().RecipeJson = jsonFile;
        tempButton.GetComponent<RecipeButton>().CanCraft = true;
        // set up button text
        TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
        t.text = itemJson.itemName + " Recipe";
        tempButton.name = itemJson.itemName + " Recipe";
        // add button to list
        InsertButton(tempButton);

    }
    private void setupButtonFromJson(PartJsonData partJson)
    {
        foreach(TextAsset jsonFile in partJsonRecipes)
            if (JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonFile).Replace("Assets", ""))).partName == partJson.partName)
            {
                setupButtonFromJson(partJson, jsonFile);
                break;
            }
    }
    private void setupButtonFromJson(PartJsonData partJson, TextAsset jsonFile)
    {
        // instantiate the button prefab
        tempButton = Instantiate(_partRecipeInfoPrefab);
        tempButton.transform.SetParent(_contentRef.transform, false);
        tempButton.GetComponent<RecipeButton>().setRecipeName(partJson.partName);
        tempButton.GetComponent<RecipeButton>().RecipeJson = jsonFile;
        tempButton.GetComponent<RecipeButton>().CanCraft = true;
        // set up button text
        TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
        t.text = partJson.partName + " Recipe";
        tempButton.name = partJson.partName + " Recipe";
        // add button to list
        InsertButton(tempButton);
    }

    private int InsertUpcomingRecipeButton(GameObject button)
    {
        upcomingRecipeButtons.Add(button);
        int i = upcomingRecipeButtons.IndexOf(button);
        button.GetComponent<RecipeButton>().setMyIndex(i);
        return i;
    }

    private bool anyRecipeSelected()
    {
        //if (_selectedItemRecipe != null || _selectedPartRecipe != null || _selectedQuestItemRecipe != null)
            //return true;
        if (_selectedJsonItemRecipe != null ||
            _selectedJsonPartRecipe != null ||
            _selectedQuestItemRecipe != null)
            return true;

        return false;
    }

    /*public List<string> itemRecipesNames()
    {
        itemRecipeName.Clear();
        foreach (ItemData item in itemRecipes)
            itemRecipeName.Add(item.ItemName);
        return itemRecipeName;
    }

    public List<string> partRecipesNames()
    {
        partRecipeName.Clear();
        foreach (PartData part in partRecipes)
            partRecipeName.Add(part.PartName);
        return partRecipeName;
    } */

    /*public ItemData getItemRecipe(int i)
    {
        return itemRecipes[i];
    }
    public ItemData getItemRecipe(string value)
    {
        foreach (ItemData item in itemRecipes)
            if (item.ItemName == value)
                return item;
        return null;
    }*/
    public ItemJsonData getItemJsonRecipe(string value)
    {
        ItemJsonData itemJsonData = new ItemJsonData();
        foreach (TextAsset item in itemJsonRecipes)
        {
            string path = Application.dataPath + AssetDatabase.GetAssetPath(item).Replace("Assets", "");
            ItemJsonData temp = JsonUtility.FromJson<ItemJsonData>(File.ReadAllText(path));
            //Debug.Log(temp.itemName);
            if (temp.itemName == value)
            {
                itemJsonData = temp;
                break;
            }
        }
        return itemJsonData;
    }

    public QuestItemData getQuestItemRecipe(string value)
    {
        foreach (QuestItemData questItem in questItemRecipes)
            if (questItem.QuestItemName == value)
                return questItem;
        return null;
    }

    /*public PartData getPartRecipe(int i)
    {
        return partRecipes[i];
    }
    public PartData getPartRecipe(string value)
    {
        foreach (PartData part in partRecipes)
            if (part.PartName == value)
                return part;
            //else Debug.LogWarning("Can not find recipe for: " + name);
        return null;
    }*/
    public PartJsonData getPartJsonRecipe(string value)
    {
        PartJsonData partJsonData = new PartJsonData();
        foreach (TextAsset part in partJsonRecipes)
        {
            string path = Application.dataPath + AssetDatabase.GetAssetPath(part).Replace("Assets", "");
            PartJsonData temp = JsonUtility.FromJson<PartJsonData>(File.ReadAllText(path));
            //Debug.Log(temp.partName);
            if (temp.partName == value)
            {
                partJsonData = temp;
                break;
            }
        }
        return partJsonData;
    }

    //public ItemData getSelectedItemRecipe() { return _selectedItemRecipe; }
    public ItemJsonData SelectedItemRecipe { get => _selectedJsonItemRecipe; }
    //public PartData getSeletedPartRecipe() { return _selectedPartRecipe; }
    public PartJsonData SelectedPartRecipe { get => _selectedJsonPartRecipe; }
    public QuestItemData getSelectedQuestRecipe() { return _selectedQuestItemRecipe; }

    public void setupFilterUI()
    {
        foreach(FilterData filter in filterData)
        {
            GameObject filterPlaceholder = Instantiate(_filterPrefab);
            filterPlaceholder.transform.SetParent(_filterParent.transform, false);
            filterPlaceholder.GetComponent<Filter>().FilterDataRef = filter;
            filterPlaceholder.GetComponent<Filter>().setupFilter();
            filterPlaceholder.name = filter.FilterName + " filter";
            filterButtons.Add(filterPlaceholder);
        }
    }

    public void toggleFilterUI()
    {
        _filterUI.SetActive(!_filterUI.activeInHierarchy);
    }

    [SerializeField] private List<FilterData> enabledFilters;
    public void addFilterToEnabled(FilterData filter)
    {
        if (enabledFilters.Contains(filter) == false)
        {
            enabledFilters.Add(filter);
        }
    }
    public void removeFilterFromEnabled(FilterData filter)
    {
        if (enabledFilters.Contains(filter) == true)
        {
            enabledFilters.Remove(filter);
        }

    }
    /*private bool checkIfEnabledFiltersValid(ItemData itemRecipe)
    {
        foreach(FilterData enabledFilter in enabledFilters)
            if (itemRecipe.ValidFilters.Contains(enabledFilter) == false)
                return false;
        return true;
    } */
    private bool checkIfEnabledFiltersValid(ItemJsonData itemJson)
    {
        foreach (FilterData enabledFilter in enabledFilters)
            if (itemJson.filters.Contains(enabledFilter.FilterName) == false)
                return false;
        return true;
    }
    /*private bool checkIfEnabledFiltersValid(PartData partRecipe)
    {
        foreach (FilterData enabledFilter in enabledFilters)
            if (partRecipe.ValidFilters.Contains(enabledFilter) == false)
                return false;
        return true;
    }*/
}
