using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] CFT_ReduceMaterialCost materialSkill;
    private GameMaster _gameMasterRef;

    [Header("UI")]
    [SerializeField] GameObject _itemCraftingUI;
    [SerializeField] GameObject _partCraftingUI;
    [SerializeField] Dropdown _recipeDropdown;
    [SerializeField] Button _craftButton;
    // item crafting
    [Space(5)]
    [SerializeField] Text _selectedRecipeText;
    [SerializeField] Text _part1Discription;
    [SerializeField] Text _part2Discription;
    [SerializeField] Text _part3Discription;
    [SerializeField] Text _finalStatsText1;
    [SerializeField] Text _finalStatsText2;
    // part crafting
    [Space(5)]
    [SerializeField] Text _partRecipeStats1;
    [SerializeField] Text _partRecipeStats2;
    [SerializeField] Text _matDiscription;
    [SerializeField] Text _partStatsText1;
    [SerializeField] Text _partStatsText2;
    [SerializeField] Text _encDiscription;

    [Header("Item Crafting")]
    [SerializeField] ItemData _chosenItemRecipe;
    [SerializeField] GameObject _chosenPart1;
    [SerializeField] GameObject _chosenPart2;
    [SerializeField] GameObject _chosenPart3;
    // [SerializeField] MaterialData _chosenMat;
    private PartDataStorage _part1DataRef;
    private PartDataStorage _part2DataRef;
    private PartDataStorage _part3DataRef;
    private GameObject itemDataStorageTemp;
    private ItemDataStorage itemDataStorageRef;
    private GameObject partDataStorageTemp;
    private PartDataStorage partDataStorageRef;
    [Header("Part Crafting")]
    [SerializeField] PartData _chosenPartRecipe;
    [SerializeField] MaterialData _chosenPartMaterial;
    [SerializeField] GameObject _optionalChosenEnchant;

    [SerializeField, HideInInspector] private List<string> recipeDropOptions;
    [SerializeField, HideInInspector] private List<string> itemRecipeOptions;
    [SerializeField, HideInInspector] private List<string> partRecipeOptions;

    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;

    private void Awake()
    {
        _gameMasterRef = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        setupRecipeDropdown();
        updateFinalStatsText();
        _itemCraftingUI.SetActive(false);
        _partCraftingUI.SetActive(false);
        _selectedRecipeText.text = "selected:\nnone";
    }

    public void checkSelection()
    {
        string selection = _recipeDropdown.options[_recipeDropdown.value].text;
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

    public void setChosenRecipe()
    {
        if (_recipeBookRef.getSelectedItemRecipe() != null)
        {
            _chosenItemRecipe = _recipeBookRef.getSelectedItemRecipe();
            _chosenPartRecipe = null;

            _itemCraftingUI.SetActive(true);
            clearItemCraftingUI();
            _partCraftingUI.SetActive(false);
            clearPartCraftingUI();

            _selectedRecipeText.text = "selected:\n" + _chosenItemRecipe.ItemName;
        }
        else if (_recipeBookRef.getSeletedPartRecipe() != null)
        {
            _chosenItemRecipe = null;
            _chosenPartRecipe = _recipeBookRef.getSeletedPartRecipe();

            _itemCraftingUI.SetActive(false);
            clearItemCraftingUI();
            _partCraftingUI.SetActive(true);
            clearPartCraftingUI();
            setupPartRecipeStats();

            _selectedRecipeText.text = "selected:\n" + _chosenPartRecipe.PartName;
        }
    }

    private void clearItemCraftingUI()
    {
        _chosenPart1 = null;
        _part1Discription.text = "choose part";
        _chosenPart2 = null;
        _part2Discription.text = "choose part";
        _chosenPart3 = null;
        _part3Discription.text = "choose part";

        _selectedRecipeText.text = "selected:\nnone";

        _finalStatsText1.text = "select [part1, part2, part3]";
        _finalStatsText2.text = "";
    }

    private void clearPartCraftingUI()
    {
        _partRecipeStats1.text = "placeholder";
        _partRecipeStats2.text = "";
        _matDiscription.text = "choose material";

        _selectedRecipeText.text = "selected:\nnone";

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

    public void invPart1Setup()
    {
        _inventoryControlReference.setupPartInventory(true, 3);
    }

    public void invPart2Setup()
    {
        _inventoryControlReference.setupPartInventory(true, 4);
    }

    public void invPart3Setup()
    {
        _inventoryControlReference.setupPartInventory(true, 5);
    }

    public void invMatSetup()
    {
        _inventoryControlReference.setupMatInventory(true, 6);
    }

    public void invEnchSetup()
    {
        _inventoryControlReference.setupEnchantInventory(true, 7);
    }

    public void SelectPart1()
    {
        _chosenPart1 = _inventoryControlReference.getSelectedPart();
        if (_chosenPart1 != null)
        {
            // setup discription
            setupDiscription(1, _chosenPart1);
        }
        else
        {
            Debug.LogWarning("No Part 1 Selected!");
        }
    }

    public void SelectPart2()
    {
        _chosenPart2 = _inventoryControlReference.getSelectedPart();
        if (_chosenPart2 != null)
        {
            // setup discription
            setupDiscription(2, _chosenPart2);
        }
        else
        {
            Debug.LogWarning("No Part 2 Selected!");
        }
    }

    public void SelectPart3()
    {
        _chosenPart3 = _inventoryControlReference.getSelectedPart();
        if (_chosenPart3 != null)
        {
            // setup discription
            setupDiscription(3, _chosenPart3);
        }
        else
        {
            Debug.LogWarning("No Part 3 Selected!");
        }
    }

    public void SelectMat()
    {
        _chosenPartMaterial = _inventoryControlReference.getSelectedMat();
        if (_chosenPartMaterial != null)
        {
            // setup discription
            setupMatDiscription(_chosenPartMaterial);
        }
        else
            Debug.LogWarning("No Mat Selected");
    }

    public void SelectEnchant()
    {
        _optionalChosenEnchant = _inventoryControlReference.getSelectedEnchant();
        if (_optionalChosenEnchant != null)
        {
            setupEnchDiscription(_optionalChosenEnchant);
        }
        else
            Debug.LogWarning("No Enchant Selected!");
    }

    public void Craft()
    {
        if (_chosenItemRecipe != null)
            CraftItem();
        else if (_chosenPartRecipe != null)
            CraftPart();
        
        else
            Debug.LogWarning("No Recipe Selected");
    }


    private EnchantDataStorage enc;
    private bool encSelected;
    private void CraftItem()
    {
        //Debug.Log("crafting");
        itemDataStorageTemp = Instantiate(_itemDataStoragePrefab);
        itemDataStorageTemp.transform.parent = _inventoryControlReference.gameObject.transform;

        itemDataStorageRef = itemDataStorageTemp.GetComponent<ItemDataStorage>();
        // stats
        itemDataStorageTemp.name = _chosenItemRecipe.ItemName;
        itemDataStorageRef.setItemName(_chosenItemRecipe.ItemName);
        itemDataStorageRef.setTotalValue(_part1DataRef.Value + _part2DataRef.Value + _part3DataRef.Value);
        itemDataStorageRef.setTotalStrenght(_part1DataRef.PartStr + _part2DataRef.PartStr + _part3DataRef.PartStr);
        itemDataStorageRef.setTotalDex(_part1DataRef.PartDex + _part2DataRef.PartDex + _part3DataRef.PartDex);
        itemDataStorageRef.setTotalInt(_part1DataRef.PartInt + _part2DataRef.PartInt + _part3DataRef.PartInt);

        // (re)move parts
        _chosenPart1.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart1(_chosenPart1.GetComponent<PartDataStorage>());
        //_inventoryControlReference.RemovePart(_part1DataRef);

        _chosenPart2.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart2(_chosenPart2.GetComponent<PartDataStorage>());
        //_inventoryControlReference.RemovePart(_part2DataRef);

        _chosenPart3.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart3(_chosenPart3.GetComponent<PartDataStorage>());
        //_inventoryControlReference.RemovePart(_part3DataRef);

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
        /*
        if (_questControlRef.CurrentQuest.QuestType == "OCC_Item" || _questControlRef.CurrentQuest.QuestType == "OCC_TotalCrafted")
            _questControlRef.updateQuestProgress(_chosenItemRecipe);
        else if (_questControlRef.CurrentQuest.QuestType == "Tutorial" || _questControlRef.CurrentQuest.QuestType == "Story")
            _questControlRef.updateQuestProgress();
        */

        _inventoryControlReference.InsertItem(itemDataStorageTemp);

        // clear selected crafting componets
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

        clearItemCraftingUI();
        _recipeDropdown.value = 0;

        if (_gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest != null && _gameMasterRef.gameObject.GetComponent<QuestControl>().CurrentQuest.QuestType == "Tutorial")
        {
            Debug.LogWarning("Quest Notif - Craft Done");
            _gameMasterRef.gameObject.GetComponent<QuestControl>().nextStage();

        }
    }

    private void CraftPart()
    {
        if (_chosenPartMaterial.CanRemoveAmount(_chosenPartRecipe.UnitsOfMaterialNeeded))
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
            _chosenPartMaterial.RemoveMat(Mathf.RoundToInt(_chosenPartRecipe.UnitsOfMaterialNeeded * materialSkill.getModifiedMatAmount()));

            // insert crafted part into inventory script
            _inventoryControlReference.InsertPart(partDataStorageTemp);

            // add experience
            GameObject.FindGameObjectWithTag("GameMaster").GetComponent<ExperienceManager>().addExperience(3);

            // clear selected crafting components
            _chosenPartRecipe = null;
            partDataStorageTemp = null;
            partDataStorageRef = null;
            _chosenPartMaterial = null;

            clearPartCraftingUI();
            _recipeDropdown.value = 0;

        }
    }

    private void setupDiscription(int i, GameObject part)
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

        updateFinalStatsText();
    }

    private void setupMatDiscription(MaterialData mat)
    {
        _matDiscription.text = mat.Material + "\nType - " + mat.MaterialType + "\nAdded Strength: " + mat.AddedStrength + "\nAdded Dextarity: " + mat.AddedDextarity + "\nAdded Intelegence: " + mat.AddedIntelligence;

        updateFinalStatsText();
    }

    private void setupEnchDiscription(GameObject enchant)
    {
        EnchantDataStorage data = enchant.GetComponent<EnchantDataStorage>();

        _encDiscription.text = "Type: " + data.EnchantName + "+" + data.AmountOfBuff;

        updateFinalStatsText();
    }

    private string finalStatsString;

    private string finalStatsString1;
    private string finalStatsString2;

    private string _itemName;
    private string _materials;
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
                _materials = "Materials\n"
                    + _part1DataRef.MaterialName + "\n" + _part2DataRef.MaterialName + "\n" + _part3DataRef.MaterialName;
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
                finalStatsString2 = _materials + _finalEnchant;
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
        //else
            //Debug.LogWarning("No Recipe selected!");
    }

    public ItemData checkItemRecipe()
    {

        return _chosenItemRecipe;
    }

    public PartData checkPartRecipe()
    {
        return _chosenPartRecipe;
    }

    private bool checkIfAnyPartEnchanted()
    {
        if (_chosenPart1.GetComponent<PartDataStorage>().IsHoldingEnchant)
            return true;
        else if (_chosenPart2.GetComponent<PartDataStorage>().IsHoldingEnchant)
            return true;
        else if (_chosenPart3.GetComponent<PartDataStorage>().IsHoldingEnchant)
            return true;

        return false;
    }
}
