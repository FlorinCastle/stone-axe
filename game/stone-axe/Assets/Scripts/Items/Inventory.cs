using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory", fileName = "Inventory.asset")]
[System.Serializable]
public class Inventory : ScriptableObject
{
    private static Inventory _instance;
    // might not need this chunk of code
    public static Inventory Instance
    {
        get
        {
            if (!_instance)
            {
                Inventory[] tmp = Resources.FindObjectsOfTypeAll<Inventory>();
                if (tmp.Length > 0)
                {
                    _instance = tmp[0];
                    Debug.Log("Found inventory as: " + _instance);
                }
                else
                {
                    Debug.Log("Did not find inventory. Create one");
                }
            }
            return _instance;
        }
    }

    // inventory stuff
    public ItemData[] itemInventory;
    public PartData[] partInventory;
    public MaterialData[] materialInventory;

    public bool ItemSlotEmpty(int index)
    {
        if (itemInventory[index] == null)
            return true;

        return false;
    }

    public bool PartSlotEmpty(int index)
    {
        if (partInventory[index] == null)
            return true;

        return false;
    }

    public bool MaterialSlotEmpty(int index)
    {
        if (materialInventory[index] == null || materialInventory[index].MaterialCount <= 0)
            return true;

        return false;
    }

    // remove item/part/material
    public bool GetItem(int index, out ItemData item)
    {
        if (ItemSlotEmpty(index))
        {
            item = null;
            return false;
        }
        item = itemInventory[index];
        return true;
    }

    public bool GetPart(int index, out PartData part)
    {
        if (PartSlotEmpty(index))
        {
            part = null;
            return false;
        }
        part = partInventory[index];
        return true;
    }
    // may or may not need this code
    public bool GetMaterial(int index, out MaterialData mat)
    {
        if (MaterialSlotEmpty(index))
        {
            mat = null;
            return false;
        }
        mat = materialInventory[index];
        return true;
    }

    // inset an item/part/material, return the index where it was inserted. -1 if error.
    public int InsertItem(ItemData item)
    {
        for (int i = 0; i < itemInventory.Length; i++)
        {
            if (ItemSlotEmpty(i))
            {
                itemInventory[i] = item;
                return i;
            }
        }
        return -1;
    }

    public int InsertPart(PartData part)
    {
        for (int p = 0; p < partInventory.Length; p++)
        {
            if (PartSlotEmpty(p))
            {
                partInventory[p] = part;
                return p;
            }
        }
        return -1;
    }

    public int InsertMaterial(MaterialData mat)
    {
        for (int m = 0; m < materialInventory.Length; m++)
        {
            if (MaterialSlotEmpty(m))
            {
                materialInventory[m] = mat;
                return m;
            }
        }
        return -1;
    }

    public int ItemInventorySize
    {
        get => itemInventory.Length;
    }

    public int PartInventorySize
    {
        get => partInventory.Length;
    }

    public int MaterialInventorySize
    {
        get => materialInventory.Length;
    }
}
