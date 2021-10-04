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

    public void setItemRecipeInfo(int index)
    {
        // get gameobject from recipeButton list at input index
        if (index == -1)
        {
            _recipeText.text = "placeholder";
        }
        else
        {
            GameObject button = recipeButtons[index];
            foreach (string itemRecipe in itemRecipeName)
            {
                if (itemRecipe == button.GetComponent<RecipeButton>().GetRecipeName)
                {
                    _recipeText.text = "";
                }
            }

        }
    }

    public void setPartRecipeInfo(int index)
    {

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
