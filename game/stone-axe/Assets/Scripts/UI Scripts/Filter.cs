using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{
    [SerializeField] private FilterData _filterDataRef;
    [SerializeField] private Image _selectedCheck;
    [SerializeField] private TextMeshProUGUI _filterName;
    [SerializeField] private RecipeBook _recipeBookRef;

    private void Awake()
    {
        _recipeBookRef = GameObject.FindGameObjectWithTag("RecipeBookControl").GetComponent<RecipeBook>();
    }

    private bool filterEnabled;
    public void toggleFilter()
    {
        filterEnabled = !filterEnabled;
        _selectedCheck.enabled = filterEnabled;
        if (filterEnabled == true) _recipeBookRef.addFilterToEnabled(_filterDataRef);
        else if (filterEnabled == false) _recipeBookRef.removeFilterFromEnabled(_filterDataRef);

        _recipeBookRef.setupFilteredGrid();
    }

    public void setupFilter()
    {
        filterEnabled = false;
        _selectedCheck.enabled = false;
        _filterName.text = _filterDataRef.FilterName;
    }

    public FilterData FilterDataRef { get => _filterDataRef; set => _filterDataRef = value; }
    public bool FilterEnabled { get => filterEnabled; }
}
