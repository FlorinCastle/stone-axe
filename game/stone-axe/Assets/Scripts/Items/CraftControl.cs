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

    [Header("UI")]
    [SerializeField] GameObject _itemCraftingUI;
    [SerializeField] GameObject _partCraftingUI;
    [SerializeField] Dropdown _recipeDropdown;
    [SerializeField] Button _craftButton;
    [SerializeField] Text _part1Discription;
    [SerializeField] Text _part2Discription;
    [SerializeField] Text _part3Discription;
    [SerializeField] Text _partRecipeStats1;
    [SerializeField] Text _partRecipeStats2;
    [SerializeField] Text _matDiscription;
    [SerializeField] Text _finalStatsText1;
    [SerializeField] Text _finalStatsText2;
    [SerializeField] Text _partStatsText1;
    [SerializeField] Text _partStatsText2;

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

    [SerializeField, HideInInspector] private List<string> recipeDropOptions;
    [SerializeField, HideInInspector] private List<string> itemRecipeOptions;
    [SerializeField, HideInInspector] private List<string> partRecipeOptions;

    [Header("Prefabs")]
    [SerializeField] private GameObject _itemDataStoragePrefab;
    [SerializeField] private GameObject _partDataStoragePrefab;

    private void Awake()
    {
        setupRecipeDropdown();
        updateFinalStatsText();
        _itemCraftingUI.SetActive(false);
        _partCraftingUI.SetActive(false);
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

    private void clearItemCraftingUI()
    {
        _chosenPart1 = null;
        _part1Discription.text = "choose part";
        _chosenPart2 = null;
        _part2Discription.text = "choose part";
        _chosenPart3 = null;
        _part3Discription.text = "choose part";

        _finalStatsText1.text = "select [part1, part2, part3]";
        _finalStatsText2.text = "";
    }

    private void clearPartCraftingUI()
    {
        _partRecipeStats1.text = "placeholder";
        _partRecipeStats2.text = "";
        _matDiscription.text = "choose material";

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

    public void Craft()
    {
        if (_chosenItemRecipe != null)
            CraftItem();
        else if (_chosenPartRecipe != null)
            CraftPart();
        
        else
            Debug.LogWarning("No Recipe Selected");
    }

    private void CraftItem()
    {
        //Debug.Log("crafting");
        itemDataStorageTemp = Instantiate(_itemDataStoragePrefab);
        itemDataStorageTemp.transform.parent = _inventoryControlReference.gameObject.transform;

        itemDataStorageRef = itemDataStorageTemp.GetComponent<ItemDataStorage>();
        // stats
        itemDataStorageTemp.name = _chosenItemRecipe.ItemName;
        itemDataStorageRef.setTotalValue(_part1DataRef.Value + _part2DataRef.Value + _part3DataRef.Value);
        itemDataStorageRef.setTotalStrenght(_part1DataRef.PartStr + _part2DataRef.PartStr + _part3DataRef.PartStr);
        itemDataStorageRef.setTotalDex(_part1DataRef.PartDex + _part2DataRef.PartDex + _part3DataRef.PartDex);
        itemDataStorageRef.setTotalInt(_part1DataRef.PartInt + _part2DataRef.PartInt + _part3DataRef.PartInt);

        _chosenPart1.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart1(_chosenPart1.GetComponent<PartDataStorage>());
        _chosenPart2.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart2(_chosenPart2.GetComponent<PartDataStorage>());
        _chosenPart3.transform.parent = itemDataStorageTemp.transform;
        itemDataStorageRef.setPart3(_chosenPart3.GetComponent<PartDataStorage>());

        _chosenItemRecipe = null;
        itemDataStorageTemp = null;
        itemDataStorageRef = null;
        _chosenPart1 = null;
        _part1DataRef = null;
        _chosenPart2 = null;
        _part2DataRef = null;
        _chosenPart3 = null;
        _part3DataRef = null;

        clearItemCraftingUI();
        _recipeDropdown.value = 0;
    }

    private void CraftPart()
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

        // clear crafting components
        _chosenPartRecipe = null;
        partDataStorageTemp = null;
        partDataStorageRef = null;
        _chosenPartMaterial = null;

        clearPartCraftingUI();
        _recipeDropdown.value = 0;
    }

    private void setupDiscription(int i, GameObject part)
    {
        PartDataStorage data = part.GetComponent<PartDataStorage>();
        if (i == 1)
        {
            _part1Discription.text = data.PartName + "\nPart Strenght: " + data.PartStr + "\nPart Dextartity: " + data.PartDex + "\nPart Intelegence: " + data.PartInt;
        }
        else if (i == 2)
        {
            _part2Discription.text = data.PartName + "\nPart Strenght: " + data.PartStr + "\nPart Dextartity: " + data.PartDex + "\nPart Intelegence: " + data.PartInt;
        }
        else if (i == 3)
        {
            _part3Discription.text = data.PartName + "\nPart Strenght: " + data.PartStr + "\nPart Dextartity: " + data.PartDex + "\nPart Intelegence: " + data.PartInt;
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

    private string finalStatsString;

    private string finalStatsString1;
    private string finalStatsString2;

    private string _itemName;
    private string _materials;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;

    private string _partName;
    private string _partMat;
    private string _partStrength;
    private string _partDex;
    private string _partInt;
    private string _partValue;
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
                _totalValue = "\n\nValue: " + (_part1DataRef.Value + _part2DataRef.Value + _part3DataRef.Value).ToString();

                finalStatsString1 = _itemName + "\nStats" + _totalStrength + _totalDex + _totalInt + _totalValue;
                finalStatsString2 = _materials;
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

                finalStatsString1 = _partName + "\nStats" + _partStrength + _partDex + _partInt + _partValue;
                finalStatsString2 = _partMat;

                _partStatsText1.text = finalStatsString1;
                _partStatsText2.text = finalStatsString2;

                _craftButton.interactable = true;
            }
            else
            {
                finalStatsString1 = "select [material]";
                _partStatsText1.text = finalStatsString1;
                _craftButton.interactable = false;
            }
        }
        else
            Debug.LogWarning("No Recipe selected!");
    }

    public ItemData checkItemRecipe()
    {

        return _chosenItemRecipe;
    }

    public PartData checkPartRecipe()
    {
        return _chosenPartRecipe;
    }
}
