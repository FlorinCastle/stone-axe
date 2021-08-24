using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    // variables
    bool isFinalProduct { get; set; } // is the final product
    string itemName { get; set; }

    // functions
    void Initialize();
    void Craft(IItem[] parts);
    void DeleteItem();
    void RemoveFromInventory(IItem[] inventory, int position);
}
