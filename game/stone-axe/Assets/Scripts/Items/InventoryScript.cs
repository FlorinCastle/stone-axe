using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] private Inventory _inventoryResourceRef;
    [SerializeField] private GameObject _itemInfoPrefab;
    [SerializeField] private GameObject _inventoryParent;

    [SerializeField] private List<ItemData> itemInventory;
    [SerializeField] private List<GameObject> _itemButtonList;
    [SerializeField] private List<PartData> partInventory;
    [SerializeField] private List<GameObject> _partButtonList;
    [SerializeField] private List<MaterialData> materialInventory;
    [SerializeField] private List<GameObject> _materialButtonList;

    [Header("UI References")]
    [SerializeField] private Text _descriptionText;

    private Vector3 prevButtonPos;
    private GameObject tempButtonList;
    public void setupInventory()
    {
        prevButtonPos = _inventoryParent.transform.position;
        clearItemButtonList();

        foreach (ItemData item in itemInventory)
        {
            if (item != null)
            {
                // instantiate the button prefab
                tempButtonList = Instantiate(_itemInfoPrefab);
                tempButtonList.transform.SetParent(_inventoryParent.transform, false);

                // set up button text
                Text t = tempButtonList.GetComponentInChildren<Text>();
                t.text = item.ItemName;

                // set button position
                prevButtonPos.y -= 53f;
                tempButtonList.transform.position = prevButtonPos;

                // add button to list
                InsertItemButton(tempButtonList);
            }
        }
    }

    private string _itemName;
    private string _materials;
    private string _totalStrength;
    private string _totalDex;
    private string _totalInt;
    private string _totalValue;

    public void setItemDetailText(int index)
    {
        if (itemInventory[index] != null)
        {
            ItemData itemRef = itemInventory[index];
            _itemName = "Item - " + itemInventory[index].ItemName;
            _materials = "\n\nMaterials\n" + itemRef.Part1.Material.Material
                + "\n" + itemRef.Part2.Material.Material
                + "\n" + itemRef.Part3.Material.Material;
            _totalStrength = "\nStrenght: " + itemRef.TotalStrength;
            _totalDex = "\nDextarity: " + itemRef.TotalDextarity;
            _totalInt = "\nIntelegence: " + itemRef.TotalIntelegence;
            _totalValue = "\n\nValue: " + itemRef.TotalValue;

            // set the text
            _descriptionText.text = _itemName +
                "\nStats" + _totalStrength
                + _totalDex
                + _totalInt
                + _materials
                + _totalValue;
        }
        else
            _descriptionText.text = "new text";
        //Debug.Log("Item Detail Text set for - item index: " + index);
    }



    public bool ItemSlotEmpty(int index)
    {
        if (itemInventory[index] == null)
            return true;

        return false;
    }

    private bool ItemButtonSlotEmpty(int index)
    {
        if (_itemButtonList[index] == null)
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
        for (int i = 0; i < itemInventory.Count; i++)
        {
            if (ItemSlotEmpty(i))
            {
                itemInventory[i] = item;
                return i;
            }
        }
        return -1;
    }

    private int InsertItemButton(GameObject item)
    {
        for (int i = 0; i < _itemButtonList.Count; i++)
            if (ItemButtonSlotEmpty(i))
            {
                _itemButtonList[i] = item;
                return i;
            }
        return -1;
    }

    public int InsertPart(PartData part)
    {
        for (int p = 0; p < partInventory.Count; p++)
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
        for (int m = 0; m < materialInventory.Count; m++)
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
        get => itemInventory.Count;
    }

    public int PartInventorySize
    {
        get => partInventory.Count;
    }

    public int MaterialInventorySize
    {
        get => materialInventory.Count;
    }

    // private int CIB_index;
    private void clearItemButtonList()
    {
        foreach (GameObject go in _itemButtonList)
            Destroy(go);

        for (int j = 0; j < _itemButtonList.Count; j++)
            _itemButtonList[j] = null;
    }
}
