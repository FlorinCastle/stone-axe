using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InventoryScriptableObject", order = 2)]
public class InventoryScriptableObject : ScriptableObject
{
    public inventory inv;
    public Item[] items;
}

[System.Serializable]
public class inventory
{

}
