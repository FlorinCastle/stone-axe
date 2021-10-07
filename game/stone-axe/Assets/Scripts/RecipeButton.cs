using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeButton : MonoBehaviour
{
    [SerializeField] private RecipeBook _recipeControl;
    [SerializeField, HideInInspector] private int _myButtonIndex;
    [SerializeField, HideInInspector] private string _recipeName;

    private void Awake()
    {
        _recipeControl = GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>();
    }

    public void setItemRecipeInfoText()
    {
        _recipeControl.setItemRecipeInfo(_myButtonIndex);
    }

    public void setPartRecipeInfoText()
    {
        _recipeControl.setPartRecipeInfo(_myButtonIndex);
    }
    /*
    public void setSelectedRecipe()
    {
        _recipeControl.setSelectedRecipe(_myButtonIndex);
    }
    */
    public void setRecipeName(string name) { _recipeName = name; }
    public string GetRecipeName { get => _recipeName; }

    public void setMyIndex(int value) { _myButtonIndex = value; }
    public int GetMyIndex { get => _myButtonIndex; }
}
