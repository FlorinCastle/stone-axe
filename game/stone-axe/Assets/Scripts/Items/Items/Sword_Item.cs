using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Item : MonoBehaviour, IItem
{
    public bool isFinalProduct { get; set; }
    public string itemName { get; set; }

    public void Initialize()
    {
        isFinalProduct = true;
    }

    public void Craft(IItem[] parts)
    {
        foreach (IItem i in parts)
            i.DeleteItem();
    }

    public void RemoveFromInventory(IItem[] inventory, int position)
    {

    }

    public void DeleteItem()
    {
        Destroy(this.gameObject);
    }
}
