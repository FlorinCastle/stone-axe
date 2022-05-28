using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeButton : MonoBehaviour
{
    [SerializeField, HideInInspector] private RecipeBook _recipeControl;
    [SerializeField, HideInInspector] private SoundMaster _soundControl;
    [SerializeField] private int _myButtonIndex;
    [SerializeField, HideInInspector] private string _recipeName;
    private bool _canCraft;

    private void Awake()
    {
        _recipeControl = GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>();
        _soundControl = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
    }

    public void playMouseOverSound() { _soundControl.playButtonHoverSound(); }
    public void playButtonClickSound() { _soundControl.playButtonClickSound(); }

    public void setItemRecipeInfoText()
    {
        _recipeControl.setItemRecipeInfo(_myButtonIndex);
        if (_canCraft == true)
            setRecipe();
    }
    public void setQuestItemReceipeInfoText()
    {
        _recipeControl.setQuestItemRecipeInfo(_myButtonIndex);
        setRecipe();
    }
    public void setPartRecipeInfoText()
    {
        _recipeControl.setPartRecipeInfo(_myButtonIndex);
        if (_canCraft == true)
            setRecipe();
    }
    public void setUpcomingRecipeInfoText()
    {
        _recipeControl.setUpcomingRecipeInfo(_myButtonIndex);
    }

    public void setRecipe()
    {
        _recipeControl.selectRecipe();
    }
    public void setRecipeName(string name) { _recipeName = name; }
    public string GetRecipeName { get => _recipeName; }

    public void setMyIndex(int value) { _myButtonIndex = value; }
    public int GetMyIndex { get => _myButtonIndex; }

    public bool CanCraft { set => _canCraft = value; }
}
