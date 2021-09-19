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

    public void checkSelection()
    {
        if (_recipeDropdown.options[_recipeDropdown.value].text == "Choose Item")
        {
            _itemCraftingUI.SetActive(false);
            _partCraftingUI.SetActive(false);
        }
        else
        {
            
        }
    }
}
