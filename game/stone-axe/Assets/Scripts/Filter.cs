using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{
    [SerializeField] private FilterData _filterDataRef;
    [SerializeField] private Image _selectedCheck;
    [SerializeField] private Text _filterName;
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
