using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MaterialScriptableObject", order = 1)]
[System.Serializable]
public class MaterialScriptableObject : ScriptableObject
{
    public materials mats;
}

[System.Serializable]
public class materials
{
    
}