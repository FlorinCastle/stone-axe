using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftControl : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Item _itemScriptReference;
    [SerializeField] Part _partScriptReference;
    [SerializeField] Material _materialScriptReference;
    [SerializeField] InventoryScript _inventoryControlReference;
    [SerializeField] RecipeBook _recipeBookRef;
    [SerializeField] QuestControl _questControlRef;
    [SerializeField] UIControl _UIControlRef;
    [SerializeField] CFT_ReduceMaterialCost materialSkill;
    private GameMaster _gameMasterRef;
    private InventoryData _invDataRef;

    [Header("UI")]
    [SerializeField] GameObject _itemCraftingUI;
    [SerializeField] GameObject _partCraftingUI;
    //[SerializeField] Dropdown _recipeDropdown;
    [SerializeField] Button _craftButton;
    [SerializeField] Button _cancelCraftButton;
    // item crafting
    [Space(5)]
    [SerializeField] TextMeshProUGUI _selectedRecipeText;
    [SerializeField] TextMeshProUGUI _part1Discription;
    [SerializeField] TextMeshProUGUI _part2Discription;
    [SerializeField] TextMeshProUGUI _part3Discription;
    [SerializeField] TextMeshProUGUI _finalStatsText1;
    [SerializeField] TextMeshProUGUI _finalStatsText2;
    // part crafting
    [Space(5)]
    [SerializeField] TextMeshProUGUI _partRecipeStats1;
    [SerializeField] TextMeshProUGUI _partRecipeStats2;
    [SerializeField] TextMeshProUGUI _matDiscription;
    [SerializeField] TextMeshProUGUI _partStatsText1;
    [SerializeField] TextMeshProUGUI _partStatsText2;
    [SerializeField] TextMeshProUGUI _encDiscription;

    [Header("Item Crafting")]
    [SerializeField] ItemData _chosenItemRecipe;
    [SerializeField] GameObject _chosenPart1;
    [SerializeField] GameObject _chosenPart2;
    [SerializeField] GameObject _chosenPart3;
    [Space(5)]
    [SerializeField] TextMeshProUGUI _part1Name;
    [SerializeField] TextMeshProUGUI _part2Name;
    [SerializeField] TextMeshProUGUI _part3Name;
    // [SerializeField] MaterialData _chosenMat;
    private PartDataStorage _part1DataRef;
    private PartDataStorage _part2DataRef;
    private PartDataStorage _part3DataRef;
    private ItemDataStorage _qPart1DataRef;
    private ItemDataStorage _qPart2DataRef;
    private ItemDataStorage _qPart3DataRef;
    private GameObject itemDataStorageTemp;
    private ItemDataStorage itemDataStorageRef;
    private GameObject partDataStorageTemp;
    private PartDataStorage partDataStorageRef;
    [Header("Part Crafting")]
    [SerializeField] PartData _chosenPartRecipe;
    [SerializeField] MaterialData _chosenPartMaterial;
    [SerializeField] GameObject _optionalChosenEnchant;
    [Header("Quest Item Crafting")]
    [SerializeField] QuestItemData _chosenQuestRecipe;

    [SerializeField, HideInInspector] private List<string> recipeDropOptions;
    [SerializeField, HideInInspector] private List<string> itemRecipeOptions;
    [SerializeField, HideInInspector] private List<string> partRecipeOptions;

    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        _invDataRef = GameObject.FindGameObjectWithTag("InventoryControl").GetComponent<InventoryData>();

        //setupRecipeDropdown();
        updateFinalStatsText();
        _itemCraftingUI.SetActive(false);
        _partCraftingUI.SetActive(false);
        _selectedRecipeText.text = "none";
    }
    /*
    public void checkSelection()
    {
        //string selection = _recipeDropdown.options[_recipeDropdown.value].text;
        int index = 0;
        if (selection == "Choose Recipe")
        {
            _itemCraftingUI.SetActive(false);
            clearItemCraftingUI();
            _partCraftingUI.SetActive(false);
            clearPartCraftingUI();
        }
        else
        {
            // Debug.Log(_recipeDropdown.options[_recipeDropdown.value].text);
            index = 0;
            foreach (string itemRecipe in itemRecipeOptions)
            {
                if (selection == itemRecipe)
                {
                    //Debug.Log("Recipe is item - " + selection);
                    _chosenItemRecipe = _recipeBookRef.getItemRecipe(index);
                    _chosenPartRecipe = null;

                    _itemCraftingUI.SetActive(true);
                    clearItemCraftingUI();
                    _partCraftingUI.SetActive(false);
                    clearPartCraftingUI();
                }
                index++;
            }
            index = 0;
            foreach (string partRecipe in partRecipeOptions)
            {
                if (selection == partRecipe)
                {
                    //Debug.Log("Recipe is part - " + selection);

                    _chosenItemRecipe = null;
                    _chosenPartRecipe = _recipeBookRef.getPartRecipe(index);

                    _itemCraftingUI.SetActive(false);
                    clearItemCraftingUI();
                    _partCraftingUI.SetActive(true);
                    clearPartCraftingUI();
                    setupPartRecipeStats();
                }
                index++;
            }
        }
    }
    */
    // add code to this to set up parts text
    public void setChosenRecipe()
    {
        if (_UIControlRef.ShopCraftUIEnabled == false)
        {
            _UIControlRef.ShopEcoUIEnabled = false;
            _UIControlRef.ShopDisUIEnabled = false;
            _UIControlRef.ShopCraftUIEnabled = true;
            _gameMasterRef.updatePlayerPosition();
        }

        if (_recipeBookRef.getSelectedItemRecipe() != null)
        {
            _chosenItemRecipe = _recipeBookRef.getSelectedItemRecipe();
            _chosenPartRecipe = null;
            _chosenQuestRecipe = null;

            _itemCraftingUI.SetActive(true);
            clearItemCraftingUI();
            _partCraftingUI.SetActive(false);
            clearPartCraftingUI();

            _selectedRecipeText.text = _chosenItemRecipe.ItemName;

            _part1Name.text = _chosenItemRecipe.Part1.PartName;
            _part2Name.text = _chosenItemRecipe.Part2.PartName;
            _part3Name.text = _chosenItemRecipe.Part3.PartName;

            _cancelCraftButton.interactable = true;
        }
        else if (_recipeBookRef.getSeletedPartRecipe() != null)
        {
            _chosenItemRecipe = null;
            _chosenPartRecipe = _recipeBookRef.getSeletedPartRecipe();
            _chosenQuestRecipe = null;

            _itemCraftingUI.SetActive(false);
            clearItemCraftingUI();
            _partCraftingUI.SetActive(true);
            clearPartCraftingUI();
            setupPartRecipeStats();

            _selectedRecipeText.text = _chosenPartRecipe.PartName;

            _cancelCraftButton.interactable = true;
        }
        else if (_recipeBookRef.getSelectedQuestRecipe() != null)
        {
            _chosenItemRecipe = null;
            _chosenPartRecipe = null;
            _chosenQuestRecipe = _recipeBookRef.getSelectedQuestRecipe();

            _itemCraftingUI.SetActive(true);
            clearItemCraftingUI();
            _partCraftingUI.SetActive(false);
            clearPartCraftingUI();

            _selectedRecipeText.text = _chosenQuestRecipe.QuestItemName;

            _cancelCraftButton.interactable = true;
        }
    }

    public void clearCraftingUI()
    {
        if (anyRecipeSelected() && (_chosenPart1 != null || _chosenPart2 != null || _chosenPart3 != null || _chosenPartMaterial != null || _optionalChosenEnchant != null))
        {
            Debug.Log("CraftControl.clearCraftingUI(): clearing selected parts/mat/enchant");
            clearItemCraftingUI();
            clearPartCraftingUI();
            _craftButton.interactable = false;
        }
        else
        {
            Debug.Log("CraftControl.clearCraftingUI(): clearing entire ui");
            clearItemCraftingUI();
            clearPartCraftingUI();
            _cancelCraftButton.interactable = false;
            _craftButton.interactable = false;

            _UIControlRef.itemCraftMenuEnabled(false);
            _UIControlRef.partCraftMenuEnabled(false);
            _UIControlRef.craftMenuEnabled(false);
        }
    }

    private void clearItemCraftingUI()
    {
        _chosenPart1 = null;
        _part1Discription.text = "choose part";
        if (_chosenItemRecipe == null && _chosenPartRecipe == null && _chosenQuestRecipe == null)
            _part1Name.text = "part 1";
        _chosenPart2 = null;
        _part2Discription.text = "choose part";
        if (_chosenItemRecipe == null && _chosenPartRecipe == null && _chosenQuestRecipe == null)
            _part2Name.text = "part 2";
        _chosenPart3 = null;
        _part3Discription.text = "choose part";
        if (_chosenItemRecipe == null && _chosenPartRecipe == null && _chosenQuestRecipe == null)
            _part3Name.text = "part 3";

        if (_chosenItemRecipe == null && _chosenPartRecipe == null && _chosenQuestRecipe == null)
            _selectedRecipeText.text = "none";

        _finalStatsText1.text = "select [part1, part2, part3]";
        _finalStatsText2.text = "";
    }

    private void clearPartCraftingUI()
    {
        //_partRecipeStats1.text = "placeholder";
        //_partRecipeStats2.text = "";
        _chosenPartMaterial = null;
        _optionalChosenEnchant = null;
        _matDiscription.text = "choose material";

        if (_chosenItemRecipe == null && _chosenPartRecipe == null && _chosenQuestRecipe == null)
            _selectedRecipeText.text = "none";

        _partStatsText1.text = "select [material]";
        _partStatsText2.text = "";
    }

    private void setupPartRecipeStats()
    {
        _partRecipeStats1.text = "Valid Material Types\n";
        foreach(string matType in _chosenPartRecipe.ValidMaterials)
        {
            _partRecipeStats1.text += matType + "\n";
        }

        _partRecipeStats1.text += "\nUnits of Material Required: " + _chosenPartRecipe.UnitsOfMaterialNeeded;

        _partRecipeStats2.text = "Base Stats\nBase Strenght: " + _chosenPartRecipe.BaseStrenght + "\nBase Intellegence: " + _chosenPartRecipe.BaseIntelligence + "\nBase Dextarity: " + _chosenPartRecipe.BaseDextarity;
    }

    /* old code
    private void setupRecipeDropdown()
    {
        _recipeDropdown.ClearOptions();
        itemRecipeOptions = _recipeBookRef.itemRecipesNames();
        partRecipeOptions = _recipeBookRef.partRecipesNames();

        recipeDropOptions.Add("Choose Recipe");

        foreach (string itemRecipe in itemRecipeOptions)
            recipeDropOptions.Add(itemRecipe);

        foreach (string partRecipe in partRecipeOptions)
            recipeDropOptions.Add(partRecipe);

        _recipeDropdown.AddOptions(recipeDropOptions);
    }
    */

    /* Old useless code
    public void invPart1Setup()
    {
        //_inventoryControlReference.setupPartInventory(true, 3);
        _inventoryControlReference.setupPartInventory();
    }

    public void invPart2Setup()
    {
        //_inventoryControlReference.setupPartInventory(true, 4);
        _inventoryControlReference.setupPartInventory();
    }

    public void invPart3Setup()
    {
        //_inventoryControlReference.setupPartInventory(true, 5);
        _inventoryControlReference.setupPartInventory();
    }

    public void invMatSetup() { _inventoryControlReference.setupMatInventory(true, 6); }
    public void invEnchSetup() { _inventoryControlReference.setupEnchantInventory(true, 7); }
    */

    public void SelectPart1()
    {
        if (_inventoryControlReference.getSelectedPart() != _chosenPart2 && _inventoryControlReference.getSelectedPart() != _chosenPart3)
            _chosenPart1 = _inventoryControlReference.getSelectedPart();

        if (_chosenPart1 != null)
            setupDiscription(1, _chosenPart1);
        else
            Debug.LogWarning("No Part 1 Selected!");

        _cancelCraftButton.interactable = true;
    }
    public void SelectPart2()
    {
        if (_inventoryControlReference.getSelectedPart() != _chosenPart1 && _inventoryControlReference.getSelectedPart() != _chosenPart3)
            _chosenPart2 = _inventoryControlReference.getSelectedPart();

        if (_chosenPart2 != null)
            setupDiscription(2, _chosenPart2);
        else
            Debug.LogWarning("No Part 2 Selected!");

        _cancelCraftButton.interactable = true;
    }
    public void SelectPart3()
    {
        if (_inventoryControlReference.getSelectedPart() != _chosenPart1 && _inventoryControlReference.getSelectedPart() != _chosenPart2)
            _chosenPart3 = _inventoryControlReference.getSelectedPart();

        if (_chosenPart3 != null)
            setupDiscription(3, _chosenPart3);
        else
            Debug.LogWarning("No Part 3 Selected!");

        _cancelCraftButton.interactable = true;
    }
    public void SelectQPart1()
    {
        if (_inventoryControlReference.getSelectedItem() != null)
            _chosenPart1 = _inventoryControlReference.getSelectedItem();
        else if (_inventoryControlReference.getSelectedPart() != null)
        {
            if (_inventoryControlReference.getSelectedPart() != _chosenPart2 && _inventoryControlReference.getSelectedPart() != _chosenPart3)
                _chosenPart1 = _inventoryControlReference.getSelectedPart();
        }

        if (_chosenPart1 != null)
            setupDiscription(1, _chosenPart1);
        else
            Debug.LogWarning("No Part 1 Selected!");

        _cancelCraftButton.interactable = true;
    }
    public void SelectQPart2()
    {
        if (_inventoryControlReference.getSelectedItem() != null)
            _chosenPart2 = _inventoryControlReference.getSelectedItem();
        else if (_inventoryControlReference.getSelectedPart() != null)
        {
            if (_inventoryControlReference.getSelectedPart() != _chosenPart1 && _inventoryControlReference.getSelectedPart() != _chosenPart3)
                _chosenPart2 = _inventoryControlReference.getSelectedPart();
        }

        if (_chosenPart2 != null)
            setupDiscription(2, _chosenPart2);
        else
            Debug.LogWarning("No Part 2 Selected!");

        _cancelCraftButton.interactable = true;
    }
    public void SelectQPart3()
    {
        if (_inventoryControlReference.getSelectedItem() != null)
            _chosenPart3 = _inventoryControlReference.getSelectedItem();
        else if (_inventoryControlReference.getSelectedPart() != null)
        {
            if (_inventoryControlReference.getSelectedPart() != _chosenPart1 && _inventoryControlReference.getSelectedPart() != _chosenPart2)
                _chosenPart3 = _inventoryControlReference.getSelectedPart();
        }

        if (_chosenPart3 != null)
            setupDiscription(3, _chosenPart3);
        else
            Debug.LogWarning("No Part 3 Selected!");

        _cancelCraftButton.interactable = true;
    }

    public void SelectMat()
    {
        _chosenPartMaterial = _inventoryControlReference.getSelectedMat();
        if (_chosenPartMaterial != null && _chosenPartRecipe.ValidMaterialData.Contains(_chosenPartMaterial))
        //if (_chosenPartMaterial != null)
        {
            // setup discription
            setupMatDiscription(_chosenPartMaterial);
        }
        else
            Debug.LogWarning("No Mat Selected");

        _cancelCraftButton.interactable = true;
    }

    public void SelectEnchant()
    {
        _optionalChosenEnchant = _inventoryControlReference.getSelectedEnchant();
        if (_optionalChosenEnchant != null)
        {
            setupEnchDiscription(_optionalChosenEnchant);
            _cancelCraftButton.interactable = true;
        }
        else if (_optionalChosenEnchant == null)
        {
            Debug.LogWarning("No Enchant Selected!");
            setupEnchDiscription();
            if (_chosenPartMaterial != null)
                _cancelCraftButton.interactable = true;
        }
        //_cancelCraftButton.interactable = true;
    }

    public void Craft()
    {
        //_gameMasterRef.gameObject.GetComponent<MiniGameControl>().resetHitPoints();
        if (_chosenItemRecipe != null)
            CraftItem();
        else if (_chosenPartRecipe != null)
            CraftPart();
        else if (_chosenQuestRecipe != null)
            CraftQuestItem();

        else
            Debug.LogWarning("No Recipe Selected");
    }

    private EnchantDataStorage enc;
    private bool encSelected;
    private int craftCount;
    private void CraftItem()
    {
        //Debug.Log("crafting");
        itemDataStorageTemp = Instantiate(_itemDataStoragePrefab);
        itemDataStorageTemp.transform.parent = _inventoryControlReference.gameObject.transform;

        itemDataStorageRef = itemDataStorageTemp.GetComponent<ItemDataStorage>();
        // stats
        itemDataStorageTemp.name = _chosenItemRecipe.ItemName;
        itemDataStorageRef.setItemName(_chosenItemRecipe.ItemName);
        itemDataStorageRef.ItemRecipeRef = _chosenItemRecipe;
        itemDataStorageRef.setTotalValue(_part1DataRef.Value + _part2DataRef.Value + _part3DataRef.Value);
        itemDataStorageRef.setTotalStrenght(_part1DataRef.PartStr + _part2DataRef.PartStr + _part3DataRef.PartStr);
        itemDataStorageRef.setTotalDex(_part1DataRef.PartDex + _part2DataRef.PartDex + _part3DataRef.PartDex);
        itemDataStorageRef.setTotalInt(_part1DataRef.PartInt + _part2DataRef.PartInt + _part3DataRef.PartInt);

        // (re)move parts
        _chosenPart1.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart1(_chosenPart1.GetComponent<PartDataStorage>());

        _chosenPart2.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart2(_chosenPart2.GetComponent<PartDataStorage>());

        _chosenPart3.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart3(_chosenPart3.GetComponent<PartDataStorage>());

        // check if any chosen parts are enchanted
        // if so, move enchant to main item
        encSelected = false;
        if (checkIfAnyPartEnchanted() == true)
        {
            if (_chosenPart1.GetComponent<PartDataStorage>().IsHoldingEnchant == true)
            {
                enc = _chosenPart1.GetComponent<PartDataStorage>().Enchantment;
                encSelected = true;
                _chosenPart1.GetComponent<PartDataStorage>().setEnchantment(null);
            }
            if (_chosenPart2.GetComponent<PartDataStorage>().IsHoldingEnchant == true)
            {
                if (encSelected == false)
                {
                    enc = _chosenPart2.GetComponent<PartDataStorage>().Enchantment;
                    encSelected = true;
                    _chosenPart2.GetComponent<PartDataStorage>().setEnchantment(null);
                }
                else if (enc.EnchantType == _chosenPart2.GetComponent<PartDataStorage>().Enchantment.EnchantType)
                {
                    enc.setAmountOfBuff(enc.AmountOfBuff + _chosenPart2.GetComponent<PartDataStorage>().Enchantment.AmountOfBuff);
                    enc.setValueOfEnchant(enc.AddedValueOfEnchant + _chosenPart2.GetComponent<PartDataStorage>().Enchantment.AddedValueOfEnchant);
                }

            }
            if (_chosenPart3.GetComponent<PartDataStorage>().IsHoldingEnchant == true)
            {
                if (encSelected == false)
                {
                    enc = _chosenPart3.GetComponent<PartDataStorage>().Enchantment;
                    encSelected = true;
                    _chosenPart3.GetComponent<PartDataStorage>().setEnchantment(null);
                }
                else if (enc.EnchantType == _chosenPart3.GetComponent<PartDataStorage>().Enchantment.EnchantType)
                {
                    enc.setAmountOfBuff(enc.AmountOfBuff + _chosenPart3.GetComponent<PartDataStorage>().Enchantment.AmountOfBuff);
                    enc.setValueOfEnchant(enc.AddedValueOfEnchant + _chosenPart3.GetComponent<PartDataStorage>().Enchantment.AddedValueOfEnchant);
                }
            }
            itemDataStorageRef.setTotalValue(itemDataStorageRef.TotalValue + enc.AddedValueOfEnchant);

            itemDataStorageRef.setIsEnchanted(true);
            enc.gameObject.transform.parent = itemDataStorageTemp.transform;
            itemDataStorageRef.setEnchantment(enc);
        }

        // add experience
        GameObject.FindGameObjectWithTag("GameMaster").GetComponent<ExperienceManager>().addExperience(4);

        // check quest
        _questControlRef.updateQuestProgress(_chosenItemRecipe);

        _inventoryControlReference.InsertItem(itemDataStorageTemp);

        // quest checks
        if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest != null)
        {
            if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial")
            {
                if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.StageType == "Craft_Item")
                {
                    if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.ItemToGet != null)
                    {
                        if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.ItemToGet == _chosenItemRecipe)
                        {
                            craftCount++;

                            if (craftCount >= _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.CountToGet)
                            {
                                Debug.Log("Quest Notif - Craft(s) Done");
                                _gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();
                                craftCount = 0;
                            }
                        }
                    }
                    else if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.ItemToGet == null)
                    {
                        Debug.LogWarning("Verify if quest has required recipe assigned");
                        _gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();
                    }
                }
            }
            else if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Story")
            {
                if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.StageType == "Craft_Item")
                {
                    if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.ItemToGet != null)
                    {
                        if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.ItemToGet == _chosenItemRecipe)
                        {
                            itemDataStorageRef.IsForQuest = true;
                            craftCount++;

                            if (craftCount >= _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.CountToGet)
                            {
                                Debug.Log("Quest Notif - Craft(s) Done");
                                _gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();
                                craftCount = 0;
                            }
                        }
                    }
                    else if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentStage.ItemToGet == null)
                    {
                        Debug.LogWarning("Verify if quest has required recipe assigned");
                        _gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();
                    }
                }
            }
            else if(_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "OCC_Item" || _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "OCC_TotalCrafted" || _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "OCC_QuestItem")
            {
                if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.RequiredItem == _chosenItemRecipe)
                {
                    Debug.LogWarning("Quest Notif - Craft Done");
                    itemDataStorageRef.IsForQuest = true;
                }
                else if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.RequiredItem == null)
                {
                    Debug.LogWarning("Verify if quest has required item recipe assigned.\nQuest name: " + _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestName);
                }
            }
        }

        // clear selected crafting componet variables
        _chosenItemRecipe = null;
        itemDataStorageTemp = null;
        itemDataStorageRef = null;
        _inventoryControlReference.RemovePart(_chosenPart1, false);
        _chosenPart1 = null;
        _part1DataRef = null;
        _inventoryControlReference.RemovePart(_chosenPart2, false);
        _chosenPart2 = null;
        _part2DataRef = null;
        _inventoryControlReference.RemovePart(_chosenPart3, false);
        _chosenPart3 = null;
        _part3DataRef = null;

        _inventoryControlReference.setupItemInventory();
        _inventoryControlReference.setupPartInventory();

        clearItemCraftingUI();
        //_recipeDropdown.value = 0;
        _gameMasterRef.gameObject.GetComponent<MiniGameControl>().stopCraftingMiniGame();

    }
    private void CraftPart()
    {
        if (_invDataRef.getMaterial(_chosenPartMaterial.Material).CanRemoveAmount(_chosenPartRecipe.UnitsOfMaterialNeeded))
        {
            partDataStorageTemp = Instantiate(_partDataStoragePrefab);
            partDataStorageTemp.transform.parent = _inventoryControlReference.gameObject.transform;

            partDataStorageRef = partDataStorageTemp.GetComponent<PartDataStorage>();
            // stats
            partDataStorageTemp.name = _chosenPartMaterial.Material + " " + _chosenPartRecipe.PartName;
            partDataStorageRef.setPartName(_chosenPartRecipe.PartName);
            partDataStorageRef.setMaterial(_chosenPartMaterial);
            partDataStorageRef.setValue(_chosenPartRecipe.BaseCost + _chosenPartMaterial.BaseCostPerUnit);
            partDataStorageRef.setPartStr(_chosenPartRecipe.BaseStrenght + _chosenPartMaterial.AddedStrength);
            partDataStorageRef.setPartDex(_chosenPartRecipe.BaseDextarity + _chosenPartMaterial.AddedDextarity);
            partDataStorageRef.setPartInt(_chosenPartRecipe.BaseIntelligence + _chosenPartMaterial.AddedIntelligence);
            partDataStorageRef.setRecipeData(_chosenPartRecipe);
            
            if (_optionalChosenEnchant != null)
            {
                _optionalChosenEnchant.transform.parent = partDataStorageRef.gameObject.transform;
                partDataStorageRef.setEnchantment(_optionalChosenEnchant.GetComponent<EnchantDataStorage>());
                partDataStorageRef.setIsHoldingEnchanted(true);
            }

            // remove right amount of materials
            _invDataRef.getMaterial(_chosenPartMaterial.Material).RemoveMat(Mathf.RoundToInt(_chosenPartRecipe.UnitsOfMaterialNeeded * materialSkill.getModifiedMatAmount()));
            _inventoryControlReference.setupMatInventory();

            // insert crafted part into inventory script
            _inventoryControlReference.InsertPart(partDataStorageTemp);

            // add experience
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<ExperienceManager>().addExperience(3);

            // clear selected crafting components
            _chosenPartRecipe = null;
            partDataStorageTemp = null;
            partDataStorageRef = null;
            _chosenPartMaterial = null;

            _inventoryControlReference.setupPartInventory();

            clearPartCraftingUI();
            //_recipeDropdown.value = 0;
            Debug.LogWarning("crafting part success");
            _gameMasterRef.gameObject.GetComponent<MiniGameControl>().stopCraftingMiniGame();

        }
    }
    private void CraftQuestItem()
    {
        // remove parts
        if (_chosenPart1.GetComponent<PartDataStorage>() != null)
            _inventoryControlReference.RemovePart(_chosenPart1, true);
        else if (_chosenPart1.GetComponent<ItemDataStorage>() != null)
            _inventoryControlReference.RemoveItem(_chosenPart1);

        if (_chosenPart2.GetComponent<PartDataStorage>() != null)
            _inventoryControlReference.RemovePart(_chosenPart2, true);
        else if (_chosenPart2.GetComponent<ItemDataStorage>() != null)
            _inventoryControlReference.RemoveItem(_chosenPart2);

        if (_chosenPart3.GetComponent<PartDataStorage>() != null)
            _inventoryControlReference.RemovePart(_chosenPart3, true);
        else if (_chosenPart3.GetComponent<ItemDataStorage>() != null)
            _inventoryControlReference.RemoveItem(_chosenPart3);

        // add experience???
        //GameObject.FindGameObjectWithTag("GameMaster").GetComponent<ExperienceManager>().addExperience(4);

        // quest checks
        if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest != null && _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "OCC_QuestItem")
        {
            if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.RequiredQuestItem == _chosenQuestRecipe)
            {
                Debug.Log("Quest Notif - Quest Craft Done");
                _gameMasterRef.gameObject.GetComponent<QuestControl>().completeQuest();
            }
            else if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.RequiredQuestItem == null)
                Debug.LogError("Verify if quest has required item recipe assigned.\nQuest name: " + _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestName);
        }
        // clear selected crafting compnents
        _chosenItemRecipe = null;
        itemDataStorageTemp = null;
        itemDataStorageRef = null;
        _chosenPart1 = null;
        _part1DataRef = null;
        _chosenPart2 = null;
        _part2DataRef = null;
        _chosenPart3 = null;
        _part3DataRef = null;

        _inventoryControlReference.setupItemInventory();
        _inventoryControlReference.setupPartInventory();

        clearItemCraftingUI();
        //_recipeDropdown.value = 0;
        _gameMasterRef.gameObject.GetComponent<MiniGameControl>().stopCraftingMiniGame();
    }

    private void setupDiscription(int i, GameObject part)
    {
        Debug.Log(part.name);
        if (part.GetComponent<PartDataStorage>() != null)
        {
            PartDataStorage data = part.GetComponent<PartDataStorage>();
            if (i == 1)
            {
                _part1Discription.text = data.PartName + "\nPart Strenght: " + data.PartStr + "\nPart Dextartity: " + data.PartDex + "\nPart Intelegence: " + data.PartInt;
                if (data.IsHoldingEnchant == true)
                {
                    _part1Discription.text += "\nEnchant: " + data.Enchantment.EnchantName + " +" + data.Enchantment.AmountOfBuff;
                }
            }
            else if (i == 2)
            {
                _part2Discription.text = data.PartName + "\nPart Strenght: " + data.PartStr + "\nPart Dextartity: " + data.PartDex + "\nPart Intelegence: " + data.PartInt;
                if (data.IsHoldingEnchant == true)
                {
                    _part2Discription.text += "\nEnchant: " + data.Enchantment.EnchantName + " +" + data.Enchantment.AmountOfBuff;
                }
            }
            else if (i == 3)
            {
                _part3Discription.text = data.PartName + "\nPart Strenght: " + data.PartStr + "\nPart Dextartity: " + data.PartDex + "\nPart Intelegence: " + data.PartInt;
                if (data.IsHoldingEnchant == true)
                {
                    _part3Discription.text += "\nEnchant: " + data.Enchantment.EnchantName + " +" + data.Enchantment.AmountOfBuff;
                }
            }
            else
                Debug.LogWarning("i value is invalid!");
        }
        else
            Debug.LogError("CraftControl.setupDiscription(): GameObject part does not contain Component PartDataStorage");
               

        updateFinalStatsText();
    }

    private void setupMatDiscription(MaterialData mat)
    {
        _matDiscription.text = mat.Material + "\nType - " + mat.MaterialType + "\nAdded Strength: " + mat.AddedStrength + "\nAdded Dextarity: " + mat.AddedDextarity + "\nAdded Intelegence: " + mat.AddedIntelligence;

        updateFinalStatsText();
    }

    private void setupEnchDiscription()
    {
        _encDiscription.text = "choose optional enchant";
    }
    private void setupEnchDiscription(GameObject enchant)
    {
        if (enchant.GetComponent<EnchantDataStorage>() != null)
        {
            EnchantDataStorage data = enchant.GetComponent<EnchantDataStorage>();
            _encDiscription.text = "Type: " + data.EnchantName + "+" + data.AmountOfBuff;
        }
        updateFinalStatsText();
    }

    private string finalStatsString;

    private string finalStatsString1;
    private string finalStatsString2;

    private string _itemName;
    private string _compData;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _finalEnchant;
    private string _totalValue;

    private string _partName;
    private string _partMat;
    private string _partStrength;
    private string _partDex;
    private string _partInt;
    private string _partEnchant;
    private string _partValue;

    private bool _enchantIsChosen = false;
    private string _enchantType;
    private EnchantDataStorage encant;
    private int _value;
    private void updateFinalStatsText()
    {
        finalStatsString = "";
        if (_chosenItemRecipe != null)
        {
            if (_chosenPart1 != null && _chosenPart2 != null && _chosenPart3 != null)
            {
                _part1DataRef = _chosenPart1.GetComponent<PartDataStorage>();
                _part2DataRef = _chosenPart2.GetComponent<PartDataStorage>();
                _part3DataRef = _chosenPart3.GetComponent<PartDataStorage>();

                _itemName = "Item - " + _chosenItemRecipe.ItemName;
                _compData = "Materials\n" + _part1DataRef.MaterialName + "\n" + _part2DataRef.MaterialName + "\n" + _part3DataRef.MaterialName;
                _totalStrength = "\nStrenght: " + (_part1DataRef.PartStr + _part2DataRef.PartStr + _part3DataRef.PartStr).ToString();
                _totalDex = "\nDextarity: " + (_part1DataRef.PartDex + _part2DataRef.PartDex + _part3DataRef.PartDex).ToString();
                _totalInt = "\nIntelegence: " + (_part1DataRef.PartInt + _part2DataRef.PartInt + _part3DataRef.PartInt).ToString();
                _value = _part1DataRef.Value + _part2DataRef.Value + _part3DataRef.Value;

                _enchantIsChosen = false;
                _finalEnchant = "";
                if (checkIfAnyPartEnchanted() == true)
                {
                    _finalEnchant = "\n\nEnchantment:\n";
                    if (_part1DataRef.IsHoldingEnchant == true)
                    {
                        _finalEnchant += _part1DataRef.Enchantment.EnchantName + " +" + _part1DataRef.Enchantment.AmountOfBuff;
                        _enchantIsChosen = true;
                        _enchantType = _part1DataRef.Enchantment.EnchantType;
                        encant = _part1DataRef.Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                    if (_part2DataRef.IsHoldingEnchant == true)
                    {
                        if (_enchantIsChosen == false)
                        {
                            _finalEnchant += _part2DataRef.Enchantment.EnchantName + " +" + _part2DataRef.Enchantment.AmountOfBuff;
                            _enchantIsChosen = true;
                            _enchantType = _part2DataRef.Enchantment.EnchantType;
                            encant = _part2DataRef.Enchantment;
                            _value += encant.AddedValueOfEnchant;
                        }
                        else if (_part2DataRef.Enchantment.EnchantType == _enchantType)
                        {
                            _finalEnchant = "\n\nEnchantment:\n";
                            _finalEnchant += encant.EnchantName + " +" + (encant.AmountOfBuff + _part2DataRef.Enchantment.AmountOfBuff);
                            _value += _part2DataRef.Enchantment.AddedValueOfEnchant;
                        }
                    }
                    if (_part3DataRef.IsHoldingEnchant == true)
                    {
                        if (_enchantIsChosen == false)
                        {
                            _finalEnchant += _part3DataRef.Enchantment.EnchantName + " +" + _part3DataRef.Enchantment.AmountOfBuff;
                            _enchantIsChosen = true;
                            _enchantType = _part3DataRef.Enchantment.EnchantType;
                            encant = _part3DataRef.Enchantment;
                            _value += encant.AddedValueOfEnchant;
                        }
                        else if (_part3DataRef.Enchantment.EnchantType == _enchantType)
                        {
                            _finalEnchant = "\n\nEnchantment:\n";
                            _finalEnchant += encant.EnchantName + " +" + (encant.AmountOfBuff + _part3DataRef.Enchantment.AmountOfBuff);
                            _value += _part3DataRef.Enchantment.AddedValueOfEnchant;
                        }
                    }
                }

                _totalValue = "\n\nValue: " + _value.ToString();

                finalStatsString1 = _itemName + "\nStats" + _totalStrength + _totalDex + _totalInt + _totalValue;
                finalStatsString2 = _compData + _finalEnchant;
                _finalStatsText1.text = finalStatsString1;
                _finalStatsText2.text = finalStatsString2;
                _craftButton.interactable = true;
            }
            else
            {
                finalStatsString += "select ";
                if (_chosenPart1 == null)
                    finalStatsString += "part1 ";
                if (_chosenPart2 == null)
                    finalStatsString += "part2 ";
                if (_chosenPart3 == null)
                    finalStatsString += "part3";

                _finalStatsText1.text = finalStatsString;
                _craftButton.interactable = false;
            }

        }
        else if (_chosenPartRecipe != null)
        {
            if (_chosenPartMaterial != null)
            {
                _partName = "Part - " + _chosenPartRecipe.PartName;
                _partMat = "Material\n" + _chosenPartMaterial.Material;
                _partStrength = "\nStrenght: " + (_chosenPartRecipe.BaseStrenght + _chosenPartMaterial.AddedStrength);
                _partDex = "\nDextarity: " + (_chosenPartRecipe.BaseDextarity + _chosenPartMaterial.AddedDextarity);
                _partInt = "\nIntelegence: " + (_chosenPartRecipe.BaseIntelligence + _chosenPartMaterial.AddedIntelligence);
                _partValue = "\n\nValue: " + (_chosenPartRecipe.BaseCost + _chosenPartMaterial.BaseCostPerUnit);

                if (_optionalChosenEnchant != null)
                    _partEnchant = "\n\nEnchant:\n" + _optionalChosenEnchant.GetComponent<EnchantDataStorage>().EnchantName + "+" + _optionalChosenEnchant.GetComponent<EnchantDataStorage>().AmountOfBuff;
                else
                    _partEnchant = "";


                finalStatsString1 = _partName + "\nStats" + _partStrength + _partDex + _partInt + _partValue;
                finalStatsString2 = _partMat + _partEnchant;

                _partStatsText1.text = finalStatsString1;
                _partStatsText2.text = finalStatsString2;

                _craftButton.interactable = true;
            }
            else
            {
                finalStatsString1 = "select [material]";
                _partStatsText1.text = finalStatsString1;
                _partStatsText2.text = "";
                _craftButton.interactable = false;
            }
        }
        else if (_chosenQuestRecipe != null)
        {
            if (_chosenPart1 != null && _chosenPart2 != null && _chosenPart3 != null)
            {
                int str = 0;
                int dex = 0;
                int intel = 0;
                int value = 0;
                if (true)
                {
                    if (_chosenPart1.GetComponent<PartDataStorage>() != null) _part1DataRef = _chosenPart1.GetComponent<PartDataStorage>();
                    else if (_chosenPart1.GetComponent<ItemDataStorage>() != null) _qPart1DataRef = _chosenPart1.GetComponent<ItemDataStorage>();

                    if (_chosenPart2.GetComponent<PartDataStorage>() != null) _part2DataRef = _chosenPart2.GetComponent<PartDataStorage>();
                    else if (_chosenPart2.GetComponent<ItemDataStorage>() != null) _qPart2DataRef = _chosenPart2.GetComponent<ItemDataStorage>();

                    if (_chosenPart3.GetComponent<PartDataStorage>() != null) _part3DataRef = _chosenPart3.GetComponent<PartDataStorage>();
                    else if (_chosenPart3.GetComponent<ItemDataStorage>() != null) _qPart3DataRef = _chosenPart3.GetComponent<ItemDataStorage>();
                    // quest item composition
                    _itemName = "Item - " + _chosenQuestRecipe.QuestItemName;
                    if (_part1DataRef != null) _compData = "Part 1 Mat: " + _part1DataRef.MaterialName;
                    else if (_qPart1DataRef != null) _compData = "Item 1 Materials: " + _qPart1DataRef.Part1.MaterialName + " " + _qPart1DataRef.Part2.MaterialName + " " + _qPart1DataRef.Part3.MaterialName;

                    if (_part2DataRef != null) _compData += "\nPart 2 Mat: " + _part2DataRef.MaterialName;
                    else if (_qPart2DataRef != null) _compData += "\nItem 2 Materials: " + _qPart2DataRef.Part1.MaterialName + " " + _qPart2DataRef.Part2.MaterialName + " " + _qPart2DataRef.Part3.MaterialName;

                    if (_part3DataRef != null) _compData += "\nPart 3 Mat: " + _part3DataRef.MaterialName;
                    else if (_qPart3DataRef != null) _compData += "\nItem 3 Materials: " + _qPart3DataRef.Part1.MaterialName + " " + _qPart3DataRef.Part2.MaterialName + " " + _qPart3DataRef.Part3.MaterialName;

                    // quest item total strength
                    if (_chosenPart1.GetComponent<PartDataStorage>() != null) str += _chosenPart1.GetComponent<PartDataStorage>().PartStr;
                    else if (_chosenPart1.GetComponent<ItemDataStorage>() != null) str += _chosenPart1.GetComponent<ItemDataStorage>().TotalStrength;

                    if (_chosenPart2.GetComponent<PartDataStorage>() != null) str += _chosenPart2.GetComponent<PartDataStorage>().PartStr;
                    else if (_chosenPart2.GetComponent<ItemDataStorage>() != null) str += _chosenPart2.GetComponent<ItemDataStorage>().TotalStrength;

                    if (_chosenPart3.GetComponent<PartDataStorage>() != null) str += _chosenPart3.GetComponent<PartDataStorage>().PartStr;
                    else if (_chosenPart3.GetComponent<ItemDataStorage>() != null) str += _chosenPart3.GetComponent<ItemDataStorage>().TotalStrength;

                    // quest item total dextarity
                    if (_chosenPart1.GetComponent<PartDataStorage>() != null) dex += _chosenPart1.GetComponent<PartDataStorage>().PartDex;
                    else if (_chosenPart1.GetComponent<ItemDataStorage>() != null) dex += _chosenPart1.GetComponent<ItemDataStorage>().TotalDextarity;

                    if (_chosenPart2.GetComponent<PartDataStorage>() != null) dex += _chosenPart2.GetComponent<PartDataStorage>().PartDex;
                    else if (_chosenPart2.GetComponent<ItemDataStorage>() != null) dex += _chosenPart2.GetComponent<ItemDataStorage>().TotalDextarity;

                    if (_chosenPart3.GetComponent<PartDataStorage>() != null) dex += _chosenPart3.GetComponent<PartDataStorage>().PartDex;
                    else if (_chosenPart3.GetComponent<ItemDataStorage>() != null) dex += _chosenPart3.GetComponent<ItemDataStorage>().TotalDextarity;

                    // quest item total intelegence
                    if (_chosenPart1.GetComponent<PartDataStorage>() != null) intel += _chosenPart1.GetComponent<PartDataStorage>().PartInt;
                    else if (_chosenPart1.GetComponent<ItemDataStorage>() != null) intel += _chosenPart1.GetComponent<ItemDataStorage>().TotalIntelegence;

                    if (_chosenPart2.GetComponent<PartDataStorage>() != null) intel += _chosenPart2.GetComponent<PartDataStorage>().PartInt;
                    else if (_chosenPart2.GetComponent<ItemDataStorage>() != null) intel += _chosenPart2.GetComponent<ItemDataStorage>().TotalIntelegence;

                    if (_chosenPart3.GetComponent<PartDataStorage>() != null) intel += _chosenPart3.GetComponent<PartDataStorage>().PartInt;
                    else if (_chosenPart3.GetComponent<ItemDataStorage>() != null) intel += _chosenPart3.GetComponent<ItemDataStorage>().TotalIntelegence;

                    if (_chosenPart1.GetComponent<PartDataStorage>() != null) value += _chosenPart1.GetComponent<PartDataStorage>().Value;
                    else if (_chosenPart1.GetComponent<ItemDataStorage>() != null) value += _chosenPart1.GetComponent<ItemDataStorage>().TotalValue;

                    if (_chosenPart2.GetComponent<PartDataStorage>() != null) value += _chosenPart2.GetComponent<PartDataStorage>().Value;
                    else if (_chosenPart2.GetComponent<ItemDataStorage>() != null) value += _chosenPart2.GetComponent<ItemDataStorage>().TotalValue;

                    if (_chosenPart3.GetComponent<PartDataStorage>() != null) value += _chosenPart3.GetComponent<PartDataStorage>().Value;
                    else if (_chosenPart3.GetComponent<ItemDataStorage>() != null) value += _chosenPart3.GetComponent<ItemDataStorage>().TotalValue;

                }
                

                _finalEnchant = "";
                if (checkIfAnyPartEnchanted() == true)
                {
                    _finalEnchant = "\n\nEnchantment:\n";
                    if (_chosenPart1.GetComponent<PartDataStorage>() != null && _chosenPart1.GetComponent<PartDataStorage>().IsHoldingEnchant == true)
                    {
                        _finalEnchant += _chosenPart1.GetComponent<PartDataStorage>().Enchantment.EnchantName + " +" + _chosenPart1.GetComponent<PartDataStorage>().Enchantment.AmountOfBuff;
                        encant = _chosenPart1.GetComponent<PartDataStorage>().Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                    else if (_chosenPart1.GetComponent<ItemDataStorage>() != null && _chosenPart1.GetComponent<ItemDataStorage>().IsEnchanted == true)
                    {
                        _finalEnchant += _chosenPart1.GetComponent<ItemDataStorage>().Enchantment.EnchantName + " +" + _chosenPart1.GetComponent<ItemDataStorage>().Enchantment.AmountOfBuff;
                        encant = _chosenPart1.GetComponent<ItemDataStorage>().Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                    else if (_chosenPart2.GetComponent<PartDataStorage>() != null && _chosenPart2.GetComponent<PartDataStorage>().IsHoldingEnchant == true)
                    {
                        _finalEnchant += _chosenPart2.GetComponent<PartDataStorage>().Enchantment.EnchantName + " +" + _chosenPart2.GetComponent<PartDataStorage>().Enchantment.AmountOfBuff;
                        encant = _chosenPart2.GetComponent<PartDataStorage>().Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                    else if (_chosenPart2.GetComponent<ItemDataStorage>() != null && _chosenPart2.GetComponent<ItemDataStorage>().IsEnchanted == true)
                    {
                        _finalEnchant += _chosenPart2.GetComponent<ItemDataStorage>().Enchantment.EnchantName + " +" + _chosenPart2.GetComponent<ItemDataStorage>().Enchantment.AmountOfBuff;
                        encant = _chosenPart2.GetComponent<ItemDataStorage>().Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                    else if (_chosenPart3.GetComponent<PartDataStorage>() != null && _chosenPart3.GetComponent<PartDataStorage>().IsHoldingEnchant == true)
                    {
                        _finalEnchant += _chosenPart3.GetComponent<PartDataStorage>().Enchantment.EnchantName + " +" + _chosenPart3.GetComponent<PartDataStorage>().Enchantment.AmountOfBuff;
                        encant = _chosenPart3.GetComponent<PartDataStorage>().Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                    else if (_chosenPart3.GetComponent<ItemDataStorage>() != null && _chosenPart3.GetComponent<ItemDataStorage>().IsEnchanted == true)
                    {
                        _finalEnchant += _chosenPart3.GetComponent<ItemDataStorage>().Enchantment.EnchantName + " +" + _chosenPart3.GetComponent<ItemDataStorage>().Enchantment.AmountOfBuff;
                        encant = _chosenPart3.GetComponent<ItemDataStorage>().Enchantment;
                        _value += encant.AddedValueOfEnchant;
                    }
                }

                _totalStrength = "\nStrenght: " + str.ToString();
                // quest item total dex
                _totalDex = "\nDextarity: " + dex.ToString();
                // quest item total int
                _totalInt = "\nIntelegence: " + intel.ToString();
                // quest item total value
                _totalValue = "\n\nValue: " + value.ToString();

                finalStatsString1 = _itemName + "\nStats" + _totalStrength + _totalDex + _totalInt + _totalValue;
                finalStatsString2 = _compData + _finalEnchant;
                _finalStatsText1.text = finalStatsString1;
                _finalStatsText2.text = finalStatsString2;
                _craftButton.interactable = true;
            }
            else
            {
                finalStatsString += "select ";
                if (_chosenPart1 == null)
                    finalStatsString += "part1 ";
                if (_chosenPart2 == null)
                    finalStatsString += "part2 ";
                if (_chosenPart3 == null)
                    finalStatsString += "part3";

                _finalStatsText1.text = finalStatsString;
                _finalStatsText2.text = "";
                _craftButton.interactable = false;
            }
        }
        //else
            //Debug.LogWarning("No Recipe selected!");
    }

    public ItemData checkItemRecipe() { return _chosenItemRecipe; }
    public PartData checkPartRecipe() { return _chosenPartRecipe; }
    public QuestItemData checkQuestRecipe() { return _chosenQuestRecipe; }

    private bool checkIfAnyPartEnchanted()
    {
        if (_chosenPart1.GetComponent<PartDataStorage>() != null && _chosenPart1.GetComponent<PartDataStorage>().IsHoldingEnchant)
                return true;
        else if (_chosenPart1.GetComponent<ItemDataStorage>() != null && _chosenPart1.GetComponent<ItemDataStorage>().IsEnchanted)
                return true;
        else if (_chosenPart2.GetComponent<PartDataStorage>() != null && _chosenPart2.GetComponent<PartDataStorage>().IsHoldingEnchant)
                return true;
        else if (_chosenPart2.GetComponent<ItemDataStorage>() != null && _chosenPart2.GetComponent<ItemDataStorage>().IsEnchanted)
                return true;
        else if (_chosenPart3.GetComponent<PartDataStorage>() != null && _chosenPart3.GetComponent<PartDataStorage>().IsHoldingEnchant)
                return true;
        else if (_chosenPart3.GetComponent<ItemDataStorage>() != null && _chosenPart3.GetComponent<ItemDataStorage>().IsEnchanted)
                return true;

        return false;
    }

    public bool anyRecipeSelected()
    {
        if (_chosenItemRecipe != null)
            return true;
        else if (_chosenPartRecipe != null)
            return true;
        else if (_chosenQuestRecipe != null)
            return true;
        return false;
    }
    public bool anyItemRecipeSelected()
    {
        if (_chosenItemRecipe != null)
            return true;
        return false;
    }
    public bool anyPartRecipeSelected()
    {
        if (_chosenPartRecipe != null)
            return true;
        return false;
    }
    public bool anyQuestRecipeSelected()
    {
        if (_chosenQuestRecipe != null)
            return true;
        return false;
    }

    public bool Part1Set()
    {
        if (_chosenPart1 != null)
            return true;
        else return false;
    }
    public bool Part2Set()
    {
        if (_chosenPart2 != null)
            return true;
        else return false;
    }
    public bool Part3Set()
    {
        if (_chosenPart3 != null)
            return true;
        else return false;
    }

    public bool AllPartsSet()
    {
        if (_chosenPart1 != null && _chosenPart2 != null && _chosenPart3 != null)
            return true;
        else return false;
    }

    public string Part1Type()
    {
        if (_chosenItemRecipe != null)
            return "part";
        else if (_chosenQuestRecipe != null)
        {
            if (_chosenQuestRecipe.ItemPart1.GetType().ToString() == "ItemData")
            {
                Debug.Log("type match: item data");
                return "item";
            }
            else if (_chosenQuestRecipe.ItemPart1.GetType().ToString() == "PartData")
            {
                Debug.Log("type match: part data");
                return "part";
            }
        }
        return null;
    }
    public string Part2Type()
    {
        if (_chosenItemRecipe != null)
            return "part";
        else if (_chosenQuestRecipe != null)
            if (_chosenQuestRecipe.ItemPart2.GetType().ToString() == "ItemData")
            {
                Debug.Log("type match: item data");
                return "item";
            }
            else if (_chosenQuestRecipe.ItemPart1.GetType().ToString() == "PartData")
            {
                Debug.Log("type match: part data");
                return "part";
            }
        return null;
    }
    public string Part3Type()
    {
        if (_chosenItemRecipe != null)
            return "part";
        else if (_chosenQuestRecipe != null)
            if (_chosenQuestRecipe.ItemPart2.GetType().ToString() == "ItemData")
            {
                Debug.Log("type match: item data");
                return "item";
            }
            else if (_chosenQuestRecipe.ItemPart1.GetType().ToString() == "PartData")
            {
                Debug.Log("type match: part data");
                return "part";
            }
        return null;
    }
}
