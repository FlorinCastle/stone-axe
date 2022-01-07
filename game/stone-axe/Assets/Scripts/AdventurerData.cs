using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAdventurerData", menuName = "ScriptableObjects/AdventurererScriptableObject", order = 57)]
[System.Serializable]
public class AdventurerData : ScriptableObject
{
    [SerializeField]
    private string _adventurerSpecies;
    [SerializeField]
    private GameObject _adventuererHead;
    private enum statInclination { Strength, Dextarity, Intelligence };
    [SerializeField]
    private statInclination _statInclination;
    /*
    [SerializeField]
    private List<Vector4> _adventurerMats;
    */

    public string AdventurerSpecies { get => _adventurerSpecies; }
    public GameObject AdventurerHead { get => _adventuererHead; }
    public string AdventurerStatInclination { get => _statInclination.ToString(); }
    //public List<Vector4> AdventurerColors { get => _adventurerMats; }
}
