using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFilterData", menuName = "FilterData", order = 58)]
public class FilterData : ScriptableObject
{
    [SerializeField] private string _filterName;

    public string FilterName { get => _filterName; }
}
