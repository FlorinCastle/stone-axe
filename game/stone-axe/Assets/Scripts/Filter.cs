using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{
    [SerializeField] private FilterData _filterDataRef;
    [SerializeField] private Image _selectedCheck;
    [SerializeField] private Text _filterName;

    private bool filterEnabled;
    public void toggleFilter()
    {
        filterEnabled = !filterEnabled;
        _selectedCheck.enabled = filterEnabled;
    }

    public void setupFilter()
    {
        filterEnabled = false;
        _selectedCheck.enabled = false;
        _filterName.text = _filterDataRef.FilterName;
    }

    public FilterData FilterDataRef { get => _filterDataRef; set => _filterDataRef = value; }
}
