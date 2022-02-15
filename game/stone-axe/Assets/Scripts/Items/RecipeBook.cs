using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    [SerializeField] private List<ItemData> itemRecipes;
    [SerializeField] private List<PartData> partRecipes;
    [SerializeField] private List<QuestItemData> questItemRecipes;
    [SerializeField, HideInInspector] private List<string> itemRecipeName;
    [SerializeField, HideInInspector] private List<string> partRecipeName;
    [SerializeField, HideInInspector] private List<GameObject> recipeButtons;
    [SerializeField, HideInInspector] private List<GameObject> upcomingRecipeButtons;
    [SerializeField, HideInInspector] private ItemData _selectedItemRecipe;
    [SerializeField, HideInInspector] private QuestItemData _selectedQuestItemRecipe;
    [SerializeField, HideInInspector] private PartData _selectedPartRecipe;
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
    [SerializeField] private GameObject _filterPrefab;

    private GameMaster _gameMasterRef;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();

        _recipeSelectButton.interactable = false; // temp till I get all the code set up
        recipeButtons = new List<GameObject>();
        setupRecipeGrid();
        _recipeText.text = "";
        setupFilterUI();
        _filterUI.SetActive(false);
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
        if (anyRecipeSelected())
        {
            CraftControl CCRef = GameObject.FindGameObjectWithTag("CraftControl").GetComponent<CraftControl>();

            CCRef.setChosenRecipe();
        }
    }

    public void setItemRecipeInfo(int index)
    {
        // get gameobject from recipeButton list at input index
        if (index == -1)
            _recipeText.text = "placeholder";
        else
        {
            GameObject button = recipeButtons[index];
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
    }
    public void setQuestItemRecipeInfo(int index)
    {
        //Debug.LogWarning("settting up quest recipe Info");
        if (index == -1)
            _recipeText.text = "placeholder";
        else
        {
            GameObject button = recipeButtons[index];
            foreach (QuestItemData questRecipe in questItemRecipes)
            {
                if (questRecipe.QuestItemName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _selectedItemRecipe = null;
                    _selectedQuestItemRecipe = questRecipe;
                    _selectedPartRecipe = null;

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
    public void setPartRecipeInfo(int index)
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
    }
    /*
    public void setSelectedRecipe(int index)
    {
        if (index != -1)
        {
            GameObject button = recipeButtons[index];
            foreach (ItemData itemRecipe in itemRecipes)
            {
                if (itemRecipe.ItemName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    Debug.Log("Selected recipe is an Item");
                }
            }
            foreach (PartData partRecipe in partRecipes)
            {
                if (partRecipe.PartName == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    Debug.Log("Selected recipe is a Part");
                }
            }
        }
    }
    */
    public void clearSelectedRecipe()
    {
        _selectedItemRecipe = null;
        _selectedPartRecipe = null;
        _recipeText.text = "";
    }

    private GameObject tempButton;

    // should be setup for level locking
    public void setupQuestRecipeGrid()
    {
        QuestData currQuest = _gameMasterRef.GetComponent<QuestControl>().CurrentQuest;
        if (currQuest != null)
        {
            if (currQuest.QuestType == "Tutorial" || currQuest.QuestType == "Story")
            {
                Debug.LogWarning("RecipeBook: quest type - Tutorail or Story");
                setupStoryQuestRecipeGrid();
            }
            else if (currQuest.QuestType == "OCC_QuestItem")
            {
                Debug.LogWarning("RecipeBook: quest type - OCC_QuestItem");
                setupSpecialQuestRecipeGrid();
            }
        }
    }

    [SerializeField, HideInInspector] private List<ItemData> levelLockedItems;
    [SerializeField, HideInInspector] private List<PartData> levelLockedParts;

    // should be setup for level locking
    public void setupSpecialQuestRecipeGrid()
    {
        QuestData currQuest = _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest;

        QuestItemData questItem = currQuest.RequiredQuestItem;

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
            Text t = tempButton.GetComponentInChildren<Text>();
            t.text = questItem.QuestItemName + " Recipe";
            tempButton.name = questItem.QuestItemName + " Recipe";
            // add button to list
            InsertButton(tempButton);
        }
        r++;
        foreach (ItemData itemRecipe in itemRecipes)
        {
            if (itemRecipe != null && (itemRecipe.ItemLevel <= _gameMasterRef.GetLevel))
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
            else
                levelLockedItems.Add(itemRecipe);
            r++;
        }
        foreach (PartData partRecipe in partRecipes)
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
            else
                levelLockedParts.Add(partRecipe);
            r++;
        }

    }
    // shold be setup for level locking
    public void setupStoryQuestRecipeGrid()
    {
        QuestData currQuest = _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest;

        ItemData reqItem = currQuest.RequiredItem;
        clearRecipeGrid();
        clearUpcomingRecipesLists();
        int r = 0;

        if (reqItem != null)
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
        }
        r++;
        foreach (ItemData itemRecipe in itemRecipes)
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
        }
        foreach (PartData partRecipe in partRecipes)
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
        }
    }

    // setup for level locking
    public void setupRecipeGrid()
    {
        clearRecipeGrid();
        clearUpcomingRecipesLists();
        int r = 0;
        foreach (ItemData itemRecipe in itemRecipes)
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
        }
        foreach (PartData partRecipe in partRecipes)
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
        }
    }

    // setup for level locking
    public void setupFilteredGrid()
    {
        clearRecipeGrid();
        clearUpcomingRecipesLists();
        //int r = 0;
        foreach(ItemData itemRecipe in itemRecipes)
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
        }
        foreach(PartData partRecipe in partRecipes)
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
        }
        if (recipeButtons.Count == 0 && enabledFilters.Count == 0)
        {
            setupRecipeGrid();
        }
    }
    private void setupUpcomingRecipes()
    {
        foreach(ItemData itemRecipe in levelLockedItems)
        {
            tempButton = Instantiate(_itemRecipeInfoPrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
            tempButton.GetComponent<RecipeButton>().CanCraft = false;

            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = itemRecipe.ItemName + " Recipe";
            tempButton.name = itemRecipe.ItemName + " Recipe";
            InsertUpcomingRecipeButton(tempButton);
        }
        foreach(PartData partRecipe in levelLockedParts)
        {
            tempButton = Instantiate(_partRecipeInfoPrefab);
            tempButton.transform.SetParent(_contentRef.transform, false);
            tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);
            tempButton.GetComponent<RecipeButton>().CanCraft = false;

            TextMeshProUGUI t = tempButton.GetComponentInChildren<TextMeshProUGUI>();
            t.text = partRecipe.PartName + " Recipe";
            tempButton.name = partRecipe.PartName + " Recipe";
            InsertUpcomingRecipeButton(tempButton);
        }
    }
    private bool upcomingRecipesBool = false;
    public void toggleUpcomingRecipes()
    {
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
            Debug.Log("placeholder");
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
        recipeButtons.Clear();
    }
    private void clearUpcomingRecipesLists()
    {
        if (levelLockedItems.Count > 0)
            levelLockedItems.Clear();
        if (levelLockedParts.Count > 0)
            levelLockedParts.Clear();
    }

    private int InsertButton(GameObject button)
    {
        recipeButtons.Add(button);
        int i = recipeButtons.IndexOf(button);
        button.GetComponent<RecipeButton>().setMyIndex(i);
        return i;
    }

    private int InsertUpcomingRecipeButton(GameObject button)
    {
        upcomingRecipeButtons.Add(button);
        int i = recipeButtons.IndexOf(button);
        button.GetComponent<RecipeButton>().setMyIndex(i);
        return i;
    }

    private bool anyRecipeSelected()
    {
        if (_selectedItemRecipe != null || _selectedPartRecipe != null)
            return true;

        return false;
    }

    public List<string> itemRecipesNames()
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
    }

    public ItemData getItemRecipe(int i)
    {
        return itemRecipes[i];
    }

    public PartData getPartRecipe(int i)
    {
        return partRecipes[i];
    }
    public PartData getPartRecipe(string name)
    {
        foreach (PartData part in partRecipes)
            if (part.PartName == name)
                return part;
            //else Debug.LogWarning("Can not find recipe for: " + name);
        return null;
    }

    public ItemData getSelectedItemRecipe()
    {
        return _selectedItemRecipe;
    }

    public PartData getSeletedPartRecipe()
    {
        return _selectedPartRecipe;
    }

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
    private bool checkIfEnabledFiltersValid(ItemData itemRecipe)
    {
        foreach(FilterData enabledFilter in enabledFilters)
            if (itemRecipe.ValidFilters.Contains(enabledFilter) == false)
                return false;
        return true;
    }
    private bool checkIfEnabledFiltersValid(PartData partRecipe)
    {
        foreach (FilterData enabledFilter in enabledFilters)
            if (partRecipe.ValidFilters.Contains(enabledFilter) == false)
                return false;
        return true;
    }
}
