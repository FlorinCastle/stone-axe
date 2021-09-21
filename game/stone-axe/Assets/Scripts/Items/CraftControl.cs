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
    [SerializeField] Text _part1Discription;
    [SerializeField] Text _part2Discription;
    [SerializeField] Text _part3Discription;
    [SerializeField] Text _finalStatsText;

    [Header("Item Crafting")]
    [SerializeField] ItemData _chosenItemRecipe;
    [SerializeField] GameObject _chosenPart1;
    [SerializeField] GameObject _chosenPart2;
    [SerializeField] GameObject _chosenPart3;
    [Header("Part Crafting")]
    [SerializeField] PartData _chosenPartRecipe;
    [SerializeField] MaterialData _chosenPartMaterial;

    [SerializeField, HideInInspector] private List<string> recipeDropOptions;
    [SerializeField, HideInInspector] private List<string> itemRecipeOptions;
    [SerializeField, HideInInspector] private List<string> partRecipeOptions;

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

                    _itemCraftingUI.SetActive(true);
                    clearItemCraftingUI();
                    _partCraftingUI.SetActive(false);
                    clearPartCraftingUI();
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
    }

    private void clearPartCraftingUI()
    {

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

    private void clearDiscription()
    {

    }

    private string finalStatsString;
    private void updateFinalStatsText()
    {
        finalStatsString = "";
        finalStatsString += "select ";
        if (_chosenPart1 == null)
            finalStatsString += "part1 ";
        if (_chosenPart2 == null)
            finalStatsString += "part2 ";
        if (_chosenPart3 == null)
            finalStatsString += "part3";

        _finalStatsText.text = finalStatsString;
    }

    public ItemData checkItemRecipe()
    {

        return _chosenItemRecipe;
    }
}
