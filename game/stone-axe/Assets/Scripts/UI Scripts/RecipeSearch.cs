using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeSearch : MonoBehaviour
{
    [SerializeField] private RecipeBook recipeBookRef;
    [SerializeField] private TMP_InputField textInput;

    public void updateRecipes()
    {
        if (recipeBookRef != null)
        {
            if (textInput.text.Length > 2)
                recipeBookRef.setupSearchedRecipeGrid(textInput.text);
            else
                recipeBookRef.setupRecipeGrid();
        }
        else
            Debug.LogError("RecipeSearch.updateRecipes(): recipeBookRef on GameObject " + gameObject.name + " is NULL! Assign recipeBookRef in the Inspector!");
    }
}
