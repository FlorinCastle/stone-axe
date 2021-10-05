using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    [SerializeField] private List<ItemData> itemRecipes;
    [SerializeField] private List<PartData> partRecipes;
    [SerializeField, HideInInspector] private List<string> itemRecipeName;
    [SerializeField, HideInInspector] private List<string> partRecipeName;
    [SerializeField, HideInInspector] private List<GameObject> recipeButtons;
    [SerializeField] private ItemData _selectedItemRecipe;
    [SerializeField] private PartData _selectedPartRecipe;
    [Header("UI References")]
    [SerializeField] private Text _recipeText;
    [SerializeField] private Button _recipeSelectButton;
    [SerializeField] private GameObject _contentRef;
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemRecipeInfoPrefab;
    [SerializeField] private GameObject _partRecipeInfoPrefab;

    private void Awake()
    {
        _recipeSelectButton.interactable = false; // temp till I get all the code set up
        recipeButtons = new List<GameObject>();
        setupRecipeGrid();
    }

    public void disableRecipeSelectButton()
    {
        _recipeSelectButton.interactable = false;
    }

    public void prepToSelectRecipe()
    {
        _recipeSelectButton.interactable = true;
    }

    public void selectRecipe()
    {
        if (anyRecipeSelected())
        {

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
                    _selectedPartRecipe = partRecipe;

                    _recipeText.text = partRecipe.PartName + "\nValid Material Types:\n";
                    foreach (string matName in partRecipe.ValidMaterials)
                        _recipeText.text += matName + "\n";
                }
            }
        }
    }

    public void clearSelectedRecipe()
    {
        _selectedItemRecipe = null;
        _selectedPartRecipe = null;
    }

    private GameObject tempButton;
    public void setupRecipeGrid()
    {
        clearRecipeGrid();
        int r = 0;
        foreach (ItemData itemRecipe in itemRecipes)
        {
            if (itemRecipe != null)
            {
                // instantiate the button prefab
                tempButton = Instantiate(_itemRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(itemRecipe.ItemName);
                // set up button text
                Text t = tempButton.GetComponentInChildren<Text>();
                t.text = itemRecipe.ItemName + " Recipe";
                tempButton.name = itemRecipe.ItemName + " Recipe";
                // add button to list
                InsertButton(tempButton, r);
            }
            r++;
        }

        foreach (PartData partRecipe in partRecipes)
        {
            if (partRecipe != null)
            {
                tempButton = Instantiate(_partRecipeInfoPrefab);
                tempButton.transform.SetParent(_contentRef.transform, false);
                tempButton.GetComponent<RecipeButton>().setRecipeName(partRecipe.PartName);

                Text t = tempButton.GetComponentInChildren<Text>();
                t.text = partRecipe.PartName + " Recipe";
                tempButton.name = partRecipe.PartName + " Recipe";
                // add button to list
                InsertButton(tempButton, r);
            }
            r++;
        }
    }

    private void clearRecipeGrid()
    {
        foreach (GameObject go in recipeButtons)
            Destroy(go);

        for (int r = 0; r < recipeButtons.Count; r++)
            recipeButtons.RemoveAt(r);
    }

    private int InsertButton(GameObject button, int index)
    {
        recipeButtons.Add(button);
        button.GetComponent<RecipeButton>().setMyIndex(index);
        return index;
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
}