using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFilterData", menuName = "ScriptableObjects/FilterData", order = 58)]
public class FilterData : ScriptableObject
{
    public string _filterName;
    public bool _hasSubFilters;
    public List<FilterData> _subFilters;

    public string FilterName { get => _filterName; }
    public bool HasSubFilters { get => _hasSubFilters; }
    public List<FilterData> SubFilters { get => _subFilters; }
}
